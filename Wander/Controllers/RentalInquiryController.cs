using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;
using Wander_Models.ViewModels;

namespace Wander.Controllers
{
    public class RentalInquiryController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPropertyRepository _propRepo;
        private readonly IOrderDetailsRepository _orderRepo;
        private readonly ILogger<HomeController> _logger;


        public RentalInquiryController( UserManager<IdentityUser> userManager, IPropertyRepository propRepo, IOrderDetailsRepository orderRepo, ILogger<HomeController> logger)
        {

            _userManager = userManager;
            _propRepo = propRepo;
            _orderRepo = orderRepo;
            _logger = logger;

        }

        public IActionResult Index(string searchName=null, string searchEmail = null, string searchPhone = null)
        {
            OrderVM orderVm = new OrderVM()
            {
                OrderDetails = _orderRepo.GetAll(u => u.Agent_Id == _userManager.GetUserId(User), includeProperties: "Property")
        };

            if(!string.IsNullOrEmpty(searchName))
            {
                orderVm.OrderDetails = orderVm.OrderDetails.Where(u => u.Name.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                orderVm.OrderDetails = orderVm.OrderDetails.Where(u => u.Email.ToLower().Contains(searchName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                orderVm.OrderDetails = orderVm.OrderDetails.Where(u => u.PhoneNumber.ToLower().Contains(searchName.ToLower()));
            }

            return View(orderVm);
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

        public IActionResult InquirySummaryPost(string Name, string PhoneNum, string Email_, string City_, DetailsVM detailsVM)
        {
            OrderDetails orderDetails = new OrderDetails()
            {
                Name = Name,
                PhoneNumber = PhoneNum,
                Email = Email_,
                PropertyId = detailsVM.Property.Id,
                Agent_Id = detailsVM.Property.Agent_Id
               
                
            };
            _orderRepo.Add(orderDetails);
            _orderRepo.Save();
            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult Details(int? id)
        {
            return View();
        }
    }
}
