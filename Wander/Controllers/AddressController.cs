using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;
using Wander_Utilities;

namespace Wander.Controllers
{
    public class AddressController : Controller

    {

        private readonly IAddressRepository _addRepo;

        public AddressController(IAddressRepository addRepo)
        {
            _addRepo = addRepo;

        }
        public IActionResult Index()
        {
            IEnumerable<Address> obList = _addRepo.GetAll();

            return View(obList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Address obj)
        {
            if(ModelState.IsValid)
            {
                _addRepo.Add(obj);
                _addRepo.Save();

                TempData[WC.Success] = "Address Added Successfully";

                return RedirectToAction("Index");
            }


            TempData[WC.Error] = "Error Creating Address";
            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _addRepo.Find(id.GetValueOrDefault());

            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Address obj)
        {
            if(ModelState.IsValid)
            {
                _addRepo.Update(obj);
                _addRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _addRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {

            var obj = _addRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _addRepo.Remove(obj);
            _addRepo.Save();
            return RedirectToAction("Index");



        }

    }
}
