using Hospital.Application.Interfaces;
using Inventory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Api.QueueManagement.Utilities
{

    public abstract class HospitalBaseController : Controller
    {
        /// <summary>
        /// IHttpContextAccessor
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// IStayUnitOfWork
        /// </summary>
        protected readonly IHospitalUnitOfWork _hospitalUnitOfWork;
        /// <summary>
        /// BaseController
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="stayUnitOfWork"></param>
        public HospitalBaseController(IHttpContextAccessor httpContextAccessor, IHospitalUnitOfWork hospitalUnitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _hospitalUnitOfWork = hospitalUnitOfWork;
        }
    }
}
