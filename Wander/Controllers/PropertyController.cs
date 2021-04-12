using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;
using Wander_Models.ViewModels;
using Wander_Utilities;

namespace Wander.Controllers
{

    public class PropertyController : Controller
    {

        private readonly IPropertyRepository _propRepo;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public string value_ { get; set; }

        public PropertyController(IPropertyRepository propRepo, IWebHostEnvironment webHostEnvironment)
        {

            _propRepo = propRepo;
            _webHostEnviroment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            IEnumerable<Property> obList = _propRepo.GetAll(includeProperties: "Address");

            return View(obList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var segmentList = new List<string>() { "Condo", "Apartment", "Vacation Home", "House" };
            PropertyVM propertyVM = new PropertyVM()
            {
                Property = new Property(),
                AddressList = _propRepo.GetAllDropDown(WC.Address),
                Type_ = segmentList

            };

            if (id == null)
            {
                return View(propertyVM);
            }
            else
            {
                propertyVM.Property = _propRepo.Find(id.GetValueOrDefault());
                if (propertyVM.Property == null)
                {
                    return NotFound();
                }
                return View(propertyVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(PropertyVM propertyVM, string value)
        {
            if (ModelState.IsValid)
            {

                if (propertyVM.Property.Id == 0)
                {
                    //create the Property object.

                    propertyVM.Property.Type = value;

                    _propRepo.Add(propertyVM.Property);

                    _propRepo.Save();

                    return RedirectToAction("Index");
                }
                else
                {
                    propertyVM.Property.Type = value;
                    _propRepo.Update(propertyVM.Property);
                }


                _propRepo.Save();
                return RedirectToAction("Index");

            }
            return View(propertyVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {


            if (id == null || id == 0)
            {
                return NotFound();
            }

            Property property = _propRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Address");

            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {

            var obj = _propRepo.Find(id.GetValueOrDefault());

            _propRepo.Remove(obj);
            _propRepo.Save();
                return RedirectToAction("Index");


          
            
        }
    }
}
    

