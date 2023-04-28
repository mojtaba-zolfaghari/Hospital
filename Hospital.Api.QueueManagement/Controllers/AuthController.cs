using ElmahCore;
using Inventory.Api.DTO.Auth.Role;

using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Linq.Dynamic.Core;
using Infrastructure.Utitlities;
using System.Linq.Expressions;
using Inventory.Api.DTO.Auth.ApiUser;
using Inventory.Api.DTO.Auth.ForgetPassword;
using Inventory.Api.DTO.Auth.Login;
using Inventory.Api.DTO.Auth.RoleRoutePermission;
using Hospital.Application.Interfaces;
using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Hospital.Domain.AuthEntity;
using Hospital.Api.QueueManagement.Utilities;
using Hospital.Api.QueueManagement.DTO.Auth;
using Hospital.Api.QueueManagement.DTO.Auth.User;
using Hospital.Api.QueueManagement.DTO.Auth.Role;
using Hospital.Api.QueueManagement.DTO.Auth.RouteAccess;
using Hospital.Api.QueueManagement.DTO.Auth.RoleRoutePermission;

namespace Hospital.Api.QueueManagement.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : HospitalBaseController
    {
        /// <summary>
        /// AuthController
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="stayUnitOfWork"></param>
        public AuthController(IHttpContextAccessor httpContextAccessor, IHospitalUnitOfWork hospitalUnitOfWork) : base(httpContextAccessor, hospitalUnitOfWork)
        {
        }
        /// <summary>
        /// forget password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("ResetPassword")]
        public async Task<ServiceActionResult<string>> ResetPassword(Command_ResetPassword_Request request)
        {
            try
            {
                var existUser = _hospitalUnitOfWork.UserRepository.GetOne(c => c.UserDetail.Email == request.Email, c => c.UserDetail);
                if (existUser != null)
                {
                    if (existUser.EnForceChangePassword) return new ServiceActionResult<string>("Enforce Password change policy enabled!", HttpStatusCode.Conflict);

                    existUser.ChangePasswordTempId = Guid.NewGuid();
                    existUser.ChangePasswordTempExpireDate = DateTime.UtcNow.AddMinutes(int.Parse(Configuration.GetConfigurationManager().GetSection("ChangePasswordExpireDatePerMinute").Value));
                    //Email it to user 
                    //Extesions.SendNotification(new Post_Notification_Request
                    //{
                    //    NotifType = (NotifType)int.Parse(Configuration.GetConfigurationManager().GetSection("Notification:type").Value),
                    //    NotificationSendType = NotificationSendType.ResetPassword,
                    //    Key = existUser.ChangePasswordTempId.ToString(),
                    //    To = int.Parse(Configuration.GetConfigurationManager().GetSection("Notification:type").Value) == 1 ? existUser.UserDetail.Email : existUser.UserDetail.Phone
                    //});
                    _hospitalUnitOfWork.UserRepository.Update(existUser);
                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "Password link generated and sent to your email! . the key is : " + existUser.ChangePasswordTempId + Environment.NewLine + " please tell me to remove the key after you tested HASSAN :)");
                }
                else
                {
                    return new ServiceActionResult<string>("User not found ", HttpStatusCode.Conflict);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }
        /// <summary>
        /// forget password generated 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("ChangePassword"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> ChangePassword(Command_ChangePassword_Request request)
        {
            try
            {
                var existUser = _hospitalUnitOfWork.UserRepository.GetOne(c => c.ChangePasswordTempId == request.ResetKey);
                if (existUser != null)
                {

                    if (existUser.ChangePasswordTempExpireDate < DateTime.UtcNow)
                        return new ServiceActionResult<string>("Password key change not valid anymore!", HttpStatusCode.Conflict);

                    existUser.ChangePasswordTempId = null;
                    existUser.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                    _hospitalUnitOfWork.UserRepository.Update(existUser);
                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "Password changed successfully!");
                }
                else
                {
                    return new ServiceActionResult<string>("invalid key or key expired ", HttpStatusCode.Conflict);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// VerifyLink
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Route("VerifyLink")]
        public async Task<ServiceActionResult<Get_VerifyPassword_Response>> VerifyLink([FromQuery] Get_VerifyPassword_Request request)
        {
            try
            {
                var existUser = _hospitalUnitOfWork.UserRepository.GetExists(c => c.ChangePasswordTempId == request.ResetKey);
                if (existUser) { return new ServiceActionResult<Get_VerifyPassword_Response>(new Get_VerifyPassword_Response { IsValid = true, ResetKey = request.ResetKey }, "Link is ready to change!"); }
                else { return new ServiceActionResult<Get_VerifyPassword_Response>("Invalid Key", HttpStatusCode.BadRequest); }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<Get_VerifyPassword_Response>();
            }
        }

        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("Login")]
        public async Task<ServiceActionResult<LoginViewModel_Response>> Login([FromBody] LoginViewModel_Request request)
        {
            try
            {
                var existUser = await _hospitalUnitOfWork.UserRepository.GetOneAsync(c => c.Username == request.Username, c => c.UserRoles, c => c.UserDetail);
                if (existUser == null) return new ServiceActionResult<LoginViewModel_Response>("Username or password incorrect", HttpStatusCode.Unauthorized);

                if (!existUser.Active) return new ServiceActionResult<LoginViewModel_Response>("User disabled!", HttpStatusCode.Forbidden);



                bool verified = BCrypt.Net.BCrypt.Verify(request.Password, existUser.Password);
                if (!verified) return new ServiceActionResult<LoginViewModel_Response>("Username or password incorrect", HttpStatusCode.Unauthorized);//TODO :  this must changes to custom actionresult

                List<Claim> authClaims = new()
                {
                    new Claim("UserId", existUser.Id.ToString()),
                    //new Claim("LanguageId", request.LanguageId == Guid.Empty ? existUser.LanguageId.ToString() : request.LanguageId.ToString())
                };

                var token = Utilities.AuthUtilities.GetToken(authClaims);
                var tokenGenerated = new JwtSecurityTokenHandler().WriteToken(token);
                var refreshToken = Utilities.AuthUtilities.GenerateRefreshToken();

                existUser.RefreshToken = refreshToken;
                existUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                existUser.Token = tokenGenerated;
                LoginViewModel_Response response = new()
                {
                    Name = existUser.Name,
                    Email = existUser.UserDetail.Email,
                    UserId = existUser.Id,
                    Token = tokenGenerated,
                    Expiration = token.ValidTo,
                    RefreshToken = refreshToken,
                };
                foreach (var roleItem in existUser.UserRoles)
                {
                    response.Roles.Add(new LoginRoles_ViewModel { RoleId = roleItem.RoleId, RoleName = _hospitalUnitOfWork.RoleRepository.GetOne(c => c.Id == roleItem.RoleId).Name });
                }

                _hospitalUnitOfWork.UserRepository.Update(existUser);
                await _hospitalUnitOfWork.SaveAsync();
                return new ServiceActionResult<LoginViewModel_Response>(response, "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<LoginViewModel_Response>();
            }
        }



        /// <summary>
        /// RefreshToken and get new jwt token based on current token and refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("RefreshToken")]
        public async Task<ServiceActionResult<LoginViewModel_Response>> RefreshToken(RefreshToken_Request request)
        {
            try
            {
                //var PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                var existingUser = _hospitalUnitOfWork.UserRepository.GetOne(c => c.RefreshToken == request.RefreshToken);
                if (existingUser == null || existingUser.RefreshTokenExpiryTime < DateTime.UtcNow) return new ServiceActionResult<LoginViewModel_Response>("Invalid Token!", HttpStatusCode.Unauthorized);

                var principal = Utilities.AuthUtilities.GetPrincipalFromExpiredToken(request.Token);

                var UserIdClaimn = Guid.Parse(principal.FindFirst("UserId").Value);
                var LanguageIdClaimn = Guid.Parse(principal.FindFirst("LanguageId").Value);
                List<Claim> authClaims = new()
                {
                    new Claim("UserId", UserIdClaimn.ToString()),
                    new Claim("LanguageId", LanguageIdClaimn.ToString())
                };
                var token = Utilities.AuthUtilities.GetToken(authClaims);
                var tokenGenerated = new JwtSecurityTokenHandler().WriteToken(token);
                var refreshToken = Utilities.AuthUtilities.GenerateRefreshToken();


                existingUser.RefreshToken = refreshToken;
                existingUser.Token = tokenGenerated;
                existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                _hospitalUnitOfWork.UserRepository.Update(existingUser);
                await _hospitalUnitOfWork.SaveAsync();

                return new ServiceActionResult<LoginViewModel_Response>(new LoginViewModel_Response
                {
                    Name = existingUser.Name,
                    Token = tokenGenerated,
                    Expiration = token.ValidTo,
                    RefreshToken = refreshToken,

                });
            }
            catch (Exception ex)
            {

                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<LoginViewModel_Response>();
            }
        }

        /// <summary>
        /// Get All Controllers And Actions
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllControllersAndActions"),HospitalAuthorization]
        public async Task<ServiceActionResult<List<ControllerAndItsActions>>> GetAllControllersAndActions()
        {
            try
            {
                var allControllersAndActions = Utilities.AuthUtilities.GetAllControllersAndTheirActions();
                return new ServiceActionResult<List<ControllerAndItsActions>>(allControllersAndActions);
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<List<ControllerAndItsActions>>();
            }
        }

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddUser"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AddUser(Add_User_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);

                if (_hospitalUnitOfWork.UserRepository.GetExists(c => c.Username == request.Username))
                    return new ServiceActionResult<string>("this username already exist!", HttpStatusCode.Conflict);

                if (_hospitalUnitOfWork.UserRepository.GetOne(c => c.UserDetail.Email == request.Email, e => e.UserDetail) != null)
                    return new ServiceActionResult<string>("this email already exist!", HttpStatusCode.Conflict);

                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);
                if (hasAccessToCreateHotel)
                {
                    User user = new()
                    {
                        Username = request.Username,
                        //IPList = request.IPList,
                        //LanguageId = request.LanguageId,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        Name = request.Name + " " + request.LastName,
                        Active = true,
                        EnForceChangePassword = true,
                        UserDetail = new UserDetail
                        {
                            FirstName = request.Name,
                            LastName = request.LastName,
                            Email = request.Email
                        }
                    };

                    _hospitalUnitOfWork.UserRepository.Create(user);

                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "DONE");
                }
                else
                {
                    return new ServiceActionResult<string>("Access denied", HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Add User Role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddUserRole"),HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AddUserRole(Add_UserRole_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);

                if (!_hospitalUnitOfWork.UserRepository.GetExists(c => c.Id == request.UserId))
                    return new ServiceActionResult<string>("user does not exist!", HttpStatusCode.BadRequest);

                if (!_hospitalUnitOfWork.RoleRepository.GetExists(c => c.Id == request.RoleId))
                    return new ServiceActionResult<string>("role does not exist!", HttpStatusCode.BadRequest);

                if (_hospitalUnitOfWork.UserRoleRepository.GetExists(c => c.UserId == request.UserId && c.RoleId == request.RoleId))
                    return new ServiceActionResult<string>("user role already exist!", HttpStatusCode.Conflict);

                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);
                if (hasAccessToCreateHotel)
                {
                    UserRole userRole = new()
                    {
                        RoleId = request.RoleId,
                        UserId = request.UserId
                    };

                    _hospitalUnitOfWork.UserRoleRepository.Create(userRole);

                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "DONE");
                }
                else
                {
                    return new ServiceActionResult<string>("Access denied", HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Get User Role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Route("GetUserRole"), HospitalAuthorization]
        public async Task<ServiceActionResult<Get_UserRoleList_Response>> GetUserRole([FromHeader] Get_UserRoleList_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<Get_UserRoleList_Response>("model properties is null", HttpStatusCode.BadRequest);

                Expression buildingPredicate = request.Filters.ToPredicate<UserRole>(typeof(UserRole));
                if (request.Filters.Count > 0 && buildingPredicate == null)
                    return new ServiceActionResult<Get_UserRoleList_Response>("You have some filters which is not valid for filter algorithm. it is case sensative", HttpStatusCode.BadRequest);

                var allUserRoles = _hospitalUnitOfWork.UserRoleRepository
                      .Get((Expression<Func<UserRole, bool>>)buildingPredicate, c => c.OrderBy(request.OrderBy + " " + request.SortType), request.Skip, request.Take, c => c.Role);
                Get_UserRoleList_Response response = new()
                {
                    UserId = allUserRoles.FirstOrDefault().UserId,
                    RoleList = allUserRoles.Select(b => new RoleList { RoleId = b.RoleId, RoleName = b.Role.Name }).ToList()
                };

                return new ServiceActionResult<Get_UserRoleList_Response>(response, "DONE");

            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<Get_UserRoleList_Response>();
            }
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("EditUser"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> EditUser(Edit_User_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);

                var existUser = _hospitalUnitOfWork.UserRepository.GetOne(c => c.Id == request.Id, c => c.UserDetail);
                if (existUser == null) return new ServiceActionResult<string>("this user not exist!", HttpStatusCode.BadRequest);

                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);
                if (hasAccessToCreateHotel)
                {
                    existUser.Id = request.Id;
                    //existUser.IPList = request.IPList;
                    //existUser.LanguageId = request.LanguageId;
                    existUser.Name = request.Name + " " + request.LastName;
                    existUser.Active = true;
                    existUser.UserDetail.FirstName = request.Name;
                    existUser.UserDetail.LastName = request.LastName;
                    //existUser.UserDetail.NationalCode = request.NationalCode;

                    //if (existUser.UserDetail.NationalCode == "") return new ServiceActionResult<string>("Invalid National Code", System.Net.HttpStatusCode.BadRequest);

                    _hospitalUnitOfWork.UserRepository.Update(existUser);
                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "DONE");
                }
                else
                {
                    return new ServiceActionResult<string>("Access denied", HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Change User Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch, Route("ChangeUserStatus"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> ChangeUserStatus(Disable_User_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);

                var existUser = _hospitalUnitOfWork.UserRepository.GetOne(c => c.Id == request.UserId);
                if (existUser == null) return new ServiceActionResult<string>("this user not exist!", HttpStatusCode.BadRequest);

                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);
                if (hasAccessToCreateHotel)
                {
                    existUser.Active = existUser.Active == true ? existUser.Active = false : existUser.Active = true;
                    _hospitalUnitOfWork.UserRepository.Update(existUser);
                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "DONE");
                }
                else
                {
                    return new ServiceActionResult<string>("Access denied", HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        #region Route And Roles
        #region Role
        /// <summary>
        /// add role so we can create access list for
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddRole"),HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AddRole(Add_RoleByName_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("Model is null", HttpStatusCode.BadRequest);
                //if (_hospitalUnitOfWork.RoleRepository.GetExists(c => c.Name == request.Name)) return new ServiceActionResult<string>("the role already exist", System.Net.HttpStatusCode.Conflict);
                _hospitalUnitOfWork.RoleRepository.Create(new Role { Name = request.Name });
                await _hospitalUnitOfWork.SaveAsync();
                return new ServiceActionResult<string>(null, "Done");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                if (((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2601)
                    return new ServiceActionResult<string>("Duplicate Role Detected !", HttpStatusCode.Conflict);

                return new ServiceActionResult<string>();
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// get all roles 
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetRoles"),HospitalAuthorization]
        public async Task<ServiceActionResult<ListResult<Get_RolesWithCountOfMembers_Response>>> GetRoles()
        {
            try
            {
                var allRoles = await _hospitalUnitOfWork.RoleRepository.GetAllAsync(null, null, null, c => c.UserRoles, c => c.UserRoleActions);
                var alRolesWithMembersCount = allRoles.Select(c => new Get_RolesWithCountOfMembers_Response { Id = c.Id, Name = c.Name, MembersCount = c.UserRoles.Count.ToString(), ActionsCount = c.UserRoleActions.Count.ToString() });
                return new ServiceActionResult<ListResult<Get_RolesWithCountOfMembers_Response>>(new ListResult<Get_RolesWithCountOfMembers_Response>(alRolesWithMembersCount, _hospitalUnitOfWork.RoleRepository.GetCount()), "Done");
                //return new ServiceActionResult<IEnumerable<Get_RolesWithCountOfMembers_Response>>(alRolesWithMembersCount,"Done", _hospitalUnitOfWork.RoleRepository.GetCount());
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<ListResult<Get_RolesWithCountOfMembers_Response>>();
            }
        }

        /// <summary>
        /// delete role from user
        /// </summary>
        /// <remarks>delete role from user by user role id passed </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Detach User from role successfuly (OK)</response>
        /// <response code="400">check the parameters</response>
        /// <response code="401">you are not loged in</response>
        /// <response code="424">Failed Dependency</response>
        /// <response code="500">Internal Server Error(somthing happens)</response>
        [HttpDelete, Route("DeleteRoleFromUser"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> DeleteRoleFromUser([FromHeader] Delete_RoleFromUser_Request request)
        {
            try
            {
                if (_hospitalUnitOfWork.UserRoleRepository.GetExists(c => c.Id == request.UserRoleId) == false)
                    return new ServiceActionResult<string>("User Role Not Found !", HttpStatusCode.BadRequest);

                _hospitalUnitOfWork.UserRoleRepository.Delete(request.UserRoleId);
                await _hospitalUnitOfWork.SaveAsync();
                return new ServiceActionResult<string>(null, "Done");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }
        #endregion

        #region Route Access List
        /// <summary>
        /// Add Route Access
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddRouteAccess"),HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AddRouteAccess([FromBody] Add_RouteAcces_Request request)
        {
            try
            {
                _hospitalUnitOfWork.RouteRepository.Create(new Domain.AuthEntity.Route { ActionName = request.ActionName, ControllerName = request.ControllerName, IpLimited = request.IpLimited });
                await _hospitalUnitOfWork.SaveAsync();
                return new ServiceActionResult<string>(null, "DONE");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                if (((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2601)
                    return new ServiceActionResult<string>("Duplicate Route Detected !", HttpStatusCode.Conflict);

                return new ServiceActionResult<string>();
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Edit Access to route
        /// </summary>
        /// <param name="request.RouteAccessId"></param>
        /// <returns>ok or fail</returns>
        [HttpPut, Route("EditRouteAccess"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> EditRouteAccess(Edit_RouteAccess_Request request)
        {
            try
            {
                var existRouteAccess = _hospitalUnitOfWork.RouteRepository.GetOne(c => c.Id == request.RouteAccessId);
                if (existRouteAccess != null)
                {
                    existRouteAccess.ControllerName = request.ControllerName;
                    existRouteAccess.ActionName = request.ActionName;
                    existRouteAccess.IpLimited = request.IpLimited;
                    _hospitalUnitOfWork.RouteRepository.Update(existRouteAccess);
                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "DONE");
                }
                else
                {
                    return new ServiceActionResult<string>("Route does not exist!", HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Get Route Access List
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet, Route("GetRouteAccessList"),HospitalAuthorization]
        public async Task<ServiceActionResult<ListResult<Get_RouteList_Response>>> GetRouteAccessList([FromHeader] Get_RouteList_Request Request)
        {
            try
            {
                var allRoutes = _hospitalUnitOfWork.RouteRepository.Get(null, c => c.OrderBy(Request.OrderBy + " " + Request.SortType), Request.Skip, Request.Take);
                var totalRowCount = _hospitalUnitOfWork.RouteRepository.GetCount();
                //var allRoutes = await _hospitalUnitOfWork.RouteRepository.GetAllAsync();
                var allRoutesList = allRoutes.Select(c => new Get_RouteList_Response { Id = c.Id, ActionName = c.ActionName, ControllerName = c.ControllerName, IpLimited = c.IpLimited });
                return new ServiceActionResult<ListResult<Get_RouteList_Response>>(new ListResult<Get_RouteList_Response>(allRoutesList, totalRowCount), "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<ListResult<Get_RouteList_Response>>();
            }
        }
        #endregion

        /// <summary>
        /// Assign Role To Route
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AssignRoleToRoute"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AssignRoleToRoute(AssignRoleToRoute_Request request)
        {
            try
            {
                var existRoleRoute = _hospitalUnitOfWork.RoleRouteRepository.GetOne(c => c.RoleId == request.RoleId && c.RouteId == request.RouteId);
                if (existRoleRoute == null)
                {
                    var existRole = _hospitalUnitOfWork.RoleRepository.GetById(request.RoleId);
                    var existRoute = _hospitalUnitOfWork.RouteRepository.GetById(request.RouteId);
                    if (existRole != null || existRoute != null)
                    {
                        _hospitalUnitOfWork.RoleRouteRepository.Create(new RoleRoute { RoleId = request.RoleId, RouteId = request.RouteId });
                        await _hospitalUnitOfWork.SaveAsync();
                        return new ServiceActionResult<string>(null, "DONE");
                    }
                    else
                    {
                        return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return new ServiceActionResult<string>("model already exist!", HttpStatusCode.Conflict);
                }


            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Assign Role To Route permission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddRoleRouteToPermission"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AddRoleRouteToPermission(AssignRoleRouteToPermission_Request request)
        {
            try
            {
                var existRoleRoute = _hospitalUnitOfWork.RoleRouteRepository.GetOne(c => c.RoleId == request.RoleId && c.RouteId == request.RouteId);
                if (existRoleRoute == null)
                {
                    var existRole = _hospitalUnitOfWork.RoleRepository.GetById(request.RoleId);
                    var existRoute = _hospitalUnitOfWork.RouteRepository.GetById(request.RouteId);
                    if (existRole != null || existRoute != null)
                    {
                        foreach (var item in request.OperationsAction)
                        {
                            _hospitalUnitOfWork.RoleRoutePermissionRepository.Create(new RoleRoutePermission { RoleId = request.RoleId, RouteId = request.RouteId, OperationAction = (System.Data.Entity.Core.Metadata.Edm.OperationAction)item });
                        }

                        await _hospitalUnitOfWork.SaveAsync();
                        return new ServiceActionResult<string>(null, "DONE");
                    }
                    else
                    {
                        return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return new ServiceActionResult<string>("model already exist!", HttpStatusCode.Conflict);
                }


            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        /// <summary>
        /// Assign Role To Route permission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Route("GetRoleRouteToPermission"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> GetRoleRouteToPermission(Get_RoleRouteToPermission_Request request)
        {
            try
            {
                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var existUserRoles = _hospitalUnitOfWork.UserRoleRepository.Get(c => c.UserId == currentUserId).ToList();
                if (existUserRoles.Count > 0)
                {
                    foreach (var item in existUserRoles)
                    {
                        var existRoleRoute = _hospitalUnitOfWork.RoleRoutePermissionRepository.Get(c => c.RoleId == item.RoleId);
                    }
                }
                await _hospitalUnitOfWork.SaveAsync();
                return new ServiceActionResult<string>(null, "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }
        #endregion

        /// <summary>
        /// GetApiAccessList
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetApiAccessList"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> GetApiAccessList([FromHeader] Get_ApiUser_Request request)
        {
            try
            {
                Expression buildingPredicate = request.Filters.ToPredicate<ApiUser>(typeof(ApiUser));
                if (request.Filters.Count > 0 && buildingPredicate == null) return new ServiceActionResult<string>("you have some filters which is not valid for filter algorithm", HttpStatusCode.BadRequest);

                var apiUserList = _hospitalUnitOfWork.ApiUserRepository.Get((Expression<Func<ApiUser, bool>>)buildingPredicate, c => c
                .OrderBy(request.OrderBy + " " + request.SortType), request.Skip, request.Take);
                return new ServiceActionResult<string>("DONE", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

    }
}
