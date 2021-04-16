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
        public async Task< IActionResult> Upsert(PropertyVM propertyVM, string value, List<IFormFile> files, string blobContainer, string directoryName)
        {
            if (ModelState.IsValid)
            {

                BlobUtility blobUtility = new BlobUtility(_optionAccessor.Value.StorageAccountNameOption, _optionAccessor.Value.StorageAccountKeyOption);

                if (propertyVM.Property.Id == 0)
                {
                    //create the Property object.

                    propertyVM.Property.Type = value;

                    propertyVM.Property.Main_Image = files[0].FileName.Substring(files[0].FileName.LastIndexOf("\\") + 1);

                    propertyVM.Property.Secondary_Image = files[1].FileName.Substring(files[1].FileName.LastIndexOf("\\") + 1);
                    propertyVM.Property.Third_Image = files[2].FileName.Substring(files[2].FileName.LastIndexOf("\\") + 1);
                    propertyVM.Property.Fourth_Image = files[3].FileName.Substring(files[3].FileName.LastIndexOf("\\") + 1);
                    propertyVM.Property.Fifth_Image = files[4].FileName.Substring(files[4].FileName.LastIndexOf("\\") + 1);

                    propertyVM.Property.Agent_Id = _userManager.GetUserId(User);

                    _propRepo.Add(propertyVM.Property);

                    _propRepo.Save();

                    blobContainer = "propertyimages";

                    directoryName = "User " + propertyVM.Property.Id;

                    CloudBlobContainer container = blobUtility.blobClient.GetContainerReference(blobContainer);

                    for (var i = 0; i < files.Count; i++)
                    {
                        int fileNameStartLocation = files[i].FileName.LastIndexOf("\\") + 1;

                        string fileName = files[i].FileName.Substring(fileNameStartLocation);

                        await container.CreateIfNotExistsAsync();

                        await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(directoryName +
                            @"\" + fileName);

                        blockBlob.Properties.ContentType = "image/jpg";

                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();


                        MagickImage image = new MagickImage(files[i].OpenReadStream());



                        await memoryStream.WriteAsync(image.ToByteArray(), 0, image.ToByteArray().Count());

                        memoryStream.Position = 0;

                        await blockBlob.UploadFromStreamAsync(memoryStream);

                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    blobContainer = "propertyimages";

                    directoryName = "User " + propertyVM.Property.Id;

                    CloudBlobContainer container = blobUtility.blobClient.GetContainerReference(blobContainer);


                    CloudBlockBlob blockBlob_0 = container.GetBlockBlobReference(directoryName +
                                    @"\" +_propRepo.FirstOrDefault(u=>u.Id == propertyVM.Property.Id, null,false).Main_Image);
                    CloudBlockBlob blockBlob1 = container.GetBlockBlobReference(directoryName +
                                    @"\" + _propRepo.FirstOrDefault(u => u.Id == propertyVM.Property.Id, null, false).Secondary_Image);
                    CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(directoryName +
                                    @"\" + _propRepo.FirstOrDefault(u => u.Id == propertyVM.Property.Id, null, false).Third_Image);
                    CloudBlockBlob blockBlob3 = container.GetBlockBlobReference(directoryName +
                                    @"\" + _propRepo.FirstOrDefault(u => u.Id == propertyVM.Property.Id, null, false).Fourth_Image);
                    CloudBlockBlob blockBlob4 = container.GetBlockBlobReference(directoryName +
                                    @"\" + _propRepo.FirstOrDefault(u => u.Id == propertyVM.Property.Id, null, false).Fifth_Image);

                    await blockBlob_0.DeleteIfExistsAsync();
                    await blockBlob1.DeleteIfExistsAsync();
                    await blockBlob2.DeleteIfExistsAsync();
                    await blockBlob3.DeleteIfExistsAsync();
                    await  blockBlob4.DeleteIfExistsAsync();

                    propertyVM.Property.Main_Image = files[0].FileName.Substring(files[0].FileName.LastIndexOf("\\") + 1);

                    propertyVM.Property.Secondary_Image = files[1].FileName.Substring(files[1].FileName.LastIndexOf("\\") + 1);
                    propertyVM.Property.Third_Image = files[2].FileName.Substring(files[2].FileName.LastIndexOf("\\") + 1);
                    propertyVM.Property.Fourth_Image = files[3].FileName.Substring(files[3].FileName.LastIndexOf("\\") + 1);
                    propertyVM.Property.Fifth_Image = files[4].FileName.Substring(files[4].FileName.LastIndexOf("\\") + 1);

                    propertyVM.Property.Agent_Id = _userManager.GetUserId(User);
                    propertyVM.Property.Type = value;

                    _propRepo.Update(propertyVM.Property);



                    for (var i = 0; i < files.Count; i++)
                    {
                        int fileNameStartLocation = files[i].FileName.LastIndexOf("\\") + 1;

                        string fileName = files[i].FileName.Substring(fileNameStartLocation);

                        await container.CreateIfNotExistsAsync();

                        await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(directoryName +
                            @"\" + fileName);

                        blockBlob.Properties.ContentType = "image/jpg";

                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();


                        MagickImage image = new MagickImage(files[i].OpenReadStream());



                        await memoryStream.WriteAsync(image.ToByteArray(), 0, image.ToByteArray().Count());

                        memoryStream.Position = 0;

                        await blockBlob.UploadFromStreamAsync(memoryStream);

                    }
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
    

