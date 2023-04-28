using Microsoft.AspNetCore.Mvc.Filters;
using ElmahCore;
using System.Security.Claims;
using Hospital.Application.Interfaces;
using Inventory.Api.Utilities;

namespace Hospital.Api.QueueManagement.Utilities
{
    /// <summary>
    /// IrsaAuthorization
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class HospitalAuthorization : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="context"></param>
        [Obsolete]
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var ActionRequested = context.RouteData.Values["action"];
                var ControllerRequested = context.RouteData.Values["controller"];

                var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IHospitalUnitOfWork>();
                if (unitOfWork == null)
                {
                    context.Result = new ServiceActionResult<string>("Access Denied on Run time!", System.Net.HttpStatusCode.Forbidden);
                    context.HttpContext.RiseError(new Exception("Access Denied on Run time!"));
                }

                var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                var UserIdClaimn = Guid.Parse(claimsIdentity.FindFirst("UserId").Value);
                //var LanguageIdClaimn = Guid.Parse(claimsIdentity.FindFirst("LanguageId").Value);
                var user = unitOfWork.UserRepository.GetOne(c => c.Id == UserIdClaimn, t => t.UserRoles);
                if (user.EnForceChangePassword) context.Result = new ServiceActionResult<string>("User needs to change password before continue using the system !", System.Net.HttpStatusCode.Locked);

                var allUserRoles = user.UserRoles.Select(c => new { c.RoleId, c.User.Active });
                if (!allUserRoles.Any()) context.Result = new ServiceActionResult<string>("User Hasn't any role Please assign role to the user !", System.Net.HttpStatusCode.Forbidden);
                var ip = context.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (allUserRoles.Any(c => c.Active == false))
                {
                    context.Result = new ServiceActionResult<string>("User Disabled!", System.Net.HttpStatusCode.Forbidden);
                    context.HttpContext.RiseError(new Exception("User Disabled! " + Environment.NewLine +
                                                "the requested ip is :" + ip + " " +
                                                Environment.NewLine + " the user id is: " + user.Id));
                }

                if (allUserRoles == null)
                {
                    context.Result = new ServiceActionResult<string>("You Have No Role", System.Net.HttpStatusCode.Forbidden);
                    context.HttpContext.RiseError(new Exception("You Have No Role! the user id is: " + user.Id));
                }
                Guid routeId = Guid.Empty;
                foreach (var item in allUserRoles)
                {
                    routeId = unitOfWork.UserRepository.hasAccessToCurrentAction(ControllerRequested.ToString(), item.RoleId, ActionRequested.ToString());
                    if (routeId != Guid.Empty)
                    {
                        var limiedByIp = unitOfWork.RouteRepository.GetOne(c => c.Id == routeId).IpLimited;
                        if (limiedByIp)
                        {
                            //bool ipValidation = GeneralUtilities.CheckIp(user.IPList, ip);
                            //if (!ipValidation)
                            //{
                            // context.Result = new ServiceActionResult<string>("IP invalid!", System.Net.HttpStatusCode.Forbidden);
                            // context.HttpContext.RiseError(new Exception("IP invalid! " + Environment.NewLine +
                            // "the requested ip is :" + ip + " " + Environment.NewLine + " the user id is: " + user.Id));
                            // break;
                            //}
                        }

                        break;
                    }
                }
                if (routeId == Guid.Empty)
                {
                    context.Result = new ServiceActionResult<string>("Access Denied!", System.Net.HttpStatusCode.Forbidden);
                    context.HttpContext.RiseError(new Exception("Access Denied! the user id is: " + user.Id));
                }
            }
            else
            {
                context.Result = new ServiceActionResult<string>("Invalid|Expired Token!", System.Net.HttpStatusCode.Unauthorized);
                context.HttpContext.RiseError(new Exception("Invalid|Expired Token!"));
            }
        }


    }
}
