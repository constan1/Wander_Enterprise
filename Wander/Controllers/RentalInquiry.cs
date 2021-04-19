using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models.ViewModels;

namespace Wander.Controllers
{
    public class RentalInquiry : Controller
    {


        private readonly IPropertyRepository _propRepo;
        private readonly ILogger<HomeController> _logger;


        public RentalInquiry( IPropertyRepository propRepo, ILogger<HomeController> logger)
        {
            _propRepo = propRepo;
            _logger = logger;

        }
        public IActionResult InquirySummary(int? id)
        {

           DetailsVM detailsVm = new DetailsVM()
            {
                Property = _propRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Address"),

            };

            return View(detailsVm);
           

        }

        public IActionResult InquiryConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("InquirySummary")]
        
        public IActionResult InquirySummaryPost()
        {
            return RedirectToAction(nameof(InquiryConfirmation));
        }
    }
}
