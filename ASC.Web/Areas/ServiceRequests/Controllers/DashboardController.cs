using ASC.Business.Interfaces;
using ASC.Model;
using ASC.Model.BaseTypes;
using ASC.Model.Models;
using ASC.Utilities;
using ASC.Web.Areas.ServiceRequests.Models;
using ASC.Web.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASC.Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    public class DashboardController : Controller
    {
        private readonly IServiceRequestOperations _serviceRequestOperations;
        private readonly IMasterDataOperations _masterData;
        private readonly IMasterDataCacheOperations _masterDataCache;
        private readonly IMapper _mapper;

        public DashboardController(
            IServiceRequestOperations operations,
            IMasterDataOperations masterData,
            IMasterDataCacheOperations masterDataCache,
            IMapper mapper)
        {
            _serviceRequestOperations = operations;
            _masterData = masterData;
            _masterDataCache = masterDataCache;
            _mapper = mapper;
        }

        // Dashboard View
        public async Task<IActionResult> Dashboard()
        {
            var status = new List<string>
            {
                Status.New.ToString(),
                Status.InProgress.ToString(),
                Status.Initiated.ToString(),
                Status.RequestForInformation.ToString()
            };

            List<ServiceRequest> serviceRequests;
            var user = HttpContext.User.GetCurrentUserDetails();

            if (HttpContext.User.IsInRole(Roles.Admin.ToString()))
            {
                serviceRequests = await _serviceRequestOperations.GetServiceRequestsByRequestedDateAndStatus(
                    DateTime.UtcNow.AddDays(-7), status);
            }
            else if (HttpContext.User.IsInRole(Roles.Engineer.ToString()))
            {
                serviceRequests = await _serviceRequestOperations.GetServiceRequestsByRequestedDateAndStatus(
                    DateTime.UtcNow.AddYears(-7), status, serviceEngineerEmail: user.Email);
            }
            else
            {
                serviceRequests = await _serviceRequestOperations.GetServiceRequestsByRequestedDateAndStatus(
                    DateTime.UtcNow.AddYears(-1), email: user.Email);
            }

            return View(new DashboardViewModel
            {
                ServiceRequests = serviceRequests.OrderByDescending(p => p.RequestedDate).ToList()
            });
        }

    }
}