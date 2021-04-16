using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;
using Wander_Models.ViewModels;
using Wander_Utilities;

namespace Wander.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyRepository _propRepo;
        private readonly IOptions<StorageAccountOptions> _optionAccessor;

        public HomeController(ILogger<HomeController> logger, IPropertyRepository propRepo, IOptions<StorageAccountOptions> optionAccessor)
        {
            _logger = logger;
            _propRepo = propRepo;
            _optionAccessor = optionAccessor;
        }

      
        public IActionResult Index()
        {
          
            HomeViewModel HomeVM = new HomeViewModel()
            {
                Properties = _propRepo.GetAll(includeProperties: "Address"),
           
            };


            return View(HomeVM);
        }

        public IActionResult About()
        {
            return View();
        }



        public IActionResult Details()
        {
            return View();
        }

  
     
    }
}
