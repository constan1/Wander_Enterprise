using Azure.Storage.Blobs;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wander_DataAccess.Repository.IRepository;
using Wander_Models;
using Wander_Models.ViewModels;
using Wander_Utilities;

/* All Code By Andrei Constantinescu
 */
namespace Wander.Controllers
{

    public class PropertyController : Controller
    {

        private readonly IPropertyRepository _propRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IOptions<StorageAccountOptions> _optionAccessor;

        public string value_ { get; set; }

        public PropertyController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> SignInManager,IPropertyRepository propRepo, IWebHostEnvironment webHostEnvironment, IOptions<StorageAccountOptions> optionAccessor)
        {
            _userManager = userManager;
            _signInManager = SignInManager;
            _propRepo = propRepo;
            _webHostEnviroment = webHostEnvironment;
            _optionAccessor = optionAccessor;

            

        }
        public IActionResult Index()
        {
            IEnumerable<Property> obList = _propRepo.GetAll(u => u.Agent_Id == _userManager.GetUserId(User), includeProperties: "Address");

            return View(obList);
        }


        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var segmentList = new List<string>() { "Condo", "Apartment", "Vacation Home", "House" };

            PropertyVM propertyVM = new PropertyVM()
            {
                Property = new Property(),
                AddressList = _propRepo.GetAllDropDown(WC.Address,u=>u.Agent_Id == _userManager.GetUserId(User)),
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
        public async Task< IActionResult> Upsert(PropertyVM propertyVM, string value, string blobContainer, string directoryName)
        {
            if (ModelState.IsValid)
            {

               

                if (propertyVM.Property.Id == 0)
                {
                    //create the Property object.

                    propertyVM.Property.Type = value;

                  

                    propertyVM.Property.Agent_Id = _userManager.GetUserId(User);

                  

                        

                    var httpRequest = HttpContext.Request;



                    await AddToDbAndAzure(propertyVM, httpRequest);



                    return RedirectToAction("Index");
                }
                else
                {

                    await DeleteFromAzureAsync(propertyVM.Property.Id);

                    var httpRequest = HttpContext.Request;

                    await UpdateToDbAndAzure(propertyVM, httpRequest);

                   

                }


                _propRepo.Save();
                return RedirectToAction("Index");

            }
            
            return View(propertyVM);
        }

        private async Task UploadToAzureAsync(IFormFile file, int Id)
        {
            BlobUtility blobUtility = new BlobUtility(_optionAccessor.Value.StorageAccountNameOption, _optionAccessor.Value.StorageAccountKeyOption);


            string directoryName = "User " + Id;

            CloudBlobContainer container = blobUtility.blobClient.GetContainerReference("propertyimages");

            if(await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            var cloudBlockBlob = container.GetBlockBlobReference(directoryName + @"\" + file.FileName);
            cloudBlockBlob.Properties.ContentType = file.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

        }

        private async Task DeleteFromAzureAsync(int Id)
        {
            BlobUtility blobUtility = new BlobUtility(_optionAccessor.Value.StorageAccountNameOption, _optionAccessor.Value.StorageAccountKeyOption);
            string directoryName = "User " + Id;

            CloudBlobContainer container = blobUtility.blobClient.GetContainerReference("propertyimages");

            var Main_Image_Blob = container.GetBlockBlobReference(directoryName + @"\" + _propRepo.FirstOrDefault(u => u.Id == Id, null, false).Main_Image);
            var Second_Image_Blob = container.GetBlockBlobReference(directoryName + @"\" + _propRepo.FirstOrDefault(u => u.Id == Id, null, false).Secondary_Image);
            var Third_Image_Blob = container.GetBlockBlobReference(directoryName + @"\" + _propRepo.FirstOrDefault(u => u.Id == Id, null, false).Third_Image);
            var Fourth_Image_Blob = container.GetBlockBlobReference(directoryName + @"\" + _propRepo.FirstOrDefault(u => u.Id == Id, null, false).Fourth_Image);
            var Fifth_Image_Blob = container.GetBlockBlobReference(directoryName + @"\" + _propRepo.FirstOrDefault(u => u.Id == Id, null, false).Fifth_Image);

            await Main_Image_Blob.DeleteIfExistsAsync();
            await Second_Image_Blob.DeleteIfExistsAsync();
            await Third_Image_Blob.DeleteIfExistsAsync();
            await Fourth_Image_Blob.DeleteIfExistsAsync();
            await Fifth_Image_Blob.DeleteIfExistsAsync();




        }

        private async Task AddToDbAndAzure(PropertyVM propertyVM, HttpRequest httpreq)
        {
            var httpRequest = httpreq;




            List<string> filelist = new List<string>();

            if (httpRequest.Form.Files.Count > 0)
            {

                foreach (var file in httpRequest.Form.Files)
                {
                    string fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
                    filelist.Add(fileName);
                }
                propertyVM.Property.Main_Image = filelist[0];
                propertyVM.Property.Secondary_Image = filelist[1];
                propertyVM.Property.Third_Image = filelist[2];
                propertyVM.Property.Fourth_Image = filelist[3];
                propertyVM.Property.Fifth_Image = filelist[4];

                _propRepo.Add(propertyVM.Property);

                _propRepo.Save();


                foreach (var file in httpRequest.Form.Files)
                {
                    string fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);



                    using (var memoryStream = new MemoryStream())
                    {

                        await UploadToAzureAsync(file, propertyVM.Property.Id);

                    }


                }

            }
        }
        private async Task UpdateToDbAndAzure(PropertyVM propertyVM, HttpRequest httpreq)
        {
            var httpRequest = httpreq;




            List<string> filelist = new List<string>();

            if (httpRequest.Form.Files.Count > 0)
            {

                foreach (var file in httpRequest.Form.Files)
                {
                    string fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
                    filelist.Add(fileName);
                }
                propertyVM.Property.Main_Image = filelist[0];
                propertyVM.Property.Secondary_Image = filelist[1];
                propertyVM.Property.Third_Image = filelist[2];
                propertyVM.Property.Fourth_Image = filelist[3];
                propertyVM.Property.Fifth_Image = filelist[4];

                _propRepo.Update(propertyVM.Property);



                foreach (var file in httpRequest.Form.Files)
                {
                    string fileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);



                    using (var memoryStream = new MemoryStream())
                    {

                        await UploadToAzureAsync(file, propertyVM.Property.Id);

                    }


                }

            }
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
      

            string blobContainer = "propertyimages";
            string directoryName = "User " + id;

            BlobUtility blobUtility = new BlobUtility(_optionAccessor.Value.StorageAccountNameOption, _optionAccessor.Value.StorageAccountKeyOption);

            CloudBlobContainer container = blobUtility.blobClient.GetContainerReference(blobContainer);



            CloudBlockBlob blockBlob = container.GetBlockBlobReference(directoryName +
                            @"\" + _propRepo.FirstOrDefault(u => u.Id == id).Main_Image);
            CloudBlockBlob blockBlob1 = container.GetBlockBlobReference(directoryName +
                            @"\" + _propRepo.FirstOrDefault(u => u.Id == id).Secondary_Image);
            CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(directoryName +
                            @"\" + _propRepo.FirstOrDefault(u => u.Id == id).Third_Image);
            CloudBlockBlob blockBlob3 = container.GetBlockBlobReference(directoryName +
                            @"\" + _propRepo.FirstOrDefault(u => u.Id == id).Fourth_Image);
            CloudBlockBlob blockBlob4 = container.GetBlockBlobReference(directoryName +
                            @"\" + _propRepo.FirstOrDefault(u => u.Id == id).Fifth_Image);

            blockBlob.DeleteIfExistsAsync();
            blockBlob1.DeleteIfExistsAsync();
            blockBlob2.DeleteIfExistsAsync();
            blockBlob3.DeleteIfExistsAsync();
            blockBlob4.DeleteIfExistsAsync();

            var obj = _propRepo.Find(id.GetValueOrDefault());

            _propRepo.Remove(obj);
            _propRepo.Save();
               return RedirectToAction("Index");

        }


    }
}
    

