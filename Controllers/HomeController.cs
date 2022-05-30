using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;
using LifeGoals.Daemons;
using LifeGoals.Dbmanagement;
using System.Drawing;
using System.Threading;
using LifeGoals.Cryptocurrencies;
using LifeGoals.Images;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LifeGoals.Models;
using LifeGoals.PageObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace LifeGoals.Controllers
{
    public class HomeController : Controller
    {


        IWebHostEnvironment _appEnvironment ;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IWebHostEnvironment appEnvironment)
        { 
            _appEnvironment = appEnvironment;
      
            _logger = logger;
        }
        String GetMethodName()
        {
            //GetFrame(1) hierarchy number
            return new StackTrace(false).GetFrame(1).GetMethod().Name;
        }
        
        
        
        public async Task<IActionResult> AddFUserImage(IFormFile uploadedFile)
        {
            if (uploadedFile != null )
                if (uploadedFile.Length<10000000)
                {
                    Console.WriteLine(uploadedFile.Length);
                    var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
               
                
                    string path = "/UserImages/" +userID + ImageManagement.GetRandomImageName()+".jpg";
                    string fullPath = _appEnvironment.WebRootPath + path;
               
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        await uploadedFile.CopyToAsync(fileStream);
                
                    
                
                
                
                
                    var imageFormat = ImageManagement.GetImageFormat(fullPath);
                
                    switch (imageFormat)
                    {
                        case ImageManagement.ImageFormat.jpeg:
                        {
                        
                            ImageManagement.ResizeImage(fullPath,new Size(512,512));
                            UserManagement.ReplacementImageUser(userID,path, _appEnvironment.WebRootPath);
                            break;
                        }
                        case ImageManagement.ImageFormat.png:
                        {
                            ImageManagement.ResizeImage(fullPath,new Size(512,512));
                            UserManagement.ReplacementImageUser(userID,path, _appEnvironment.WebRootPath);
                            break;
                        } 
                    }
                
               

               
                }
            
            return Redirect("/Identity/Account/Manage");
        }

        public IActionResult EditDescription(string description)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserManagement.SetUserDescription(userId,description);
            
            return Redirect("/Identity/Account/Manage");
        }

        public async Task<IActionResult> AddFUserBackground(IFormFile uploadedFile)
        {
            if (uploadedFile != null )
                if (uploadedFile.Length<10000000)
                {
                    Console.WriteLine(uploadedFile.Length);
                    var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
               
                
                    string path = "/UserBackground/" +userID + ImageManagement.GetRandomImageName()+"B"+".jpg";
                    string fullPath = _appEnvironment.WebRootPath + path;
               
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        await uploadedFile.CopyToAsync(fileStream);
                
                    
                
                
                
                
                    var imageFormat = ImageManagement.GetImageFormat(fullPath);
                
                    switch (imageFormat)
                    {
                        case ImageManagement.ImageFormat.jpeg:
                        {
                        
                            ImageManagement.ResizeImage(fullPath,new Size(1920,1080));
                            UserManagement.ReplacementBackgroundUser(userID,path, _appEnvironment.WebRootPath);
                            break;
                        }
                        case ImageManagement.ImageFormat.png:
                        {
                            ImageManagement.ResizeImage(fullPath,new Size(1920,1080));
                            UserManagement.ReplacementBackgroundUser(userID,path, _appEnvironment.WebRootPath);
                            break;
                        } 
                    }
                
               

               
                }
            
            return Redirect("/Identity/Account/Manage");
        }
        
        
        [Authorize] 
        public IActionResult GoalLineUpdate(string userId)
        {
            return PartialView("Profile/GoalLine",userId);
        }
       
        public IActionResult Privacy()
        {
            return View();
        }
        
        
        public IActionResult Profile(string address)
        {

            if (address==default)
            {
                if(User.Identity.IsAuthenticated)
                {
                    return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = GetMethodName()});
                }
                else 
                {
                    return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = "Welcome"});
                }
                
            }
            else
            {
                return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = GetMethodName()});
            }
            
            
            
        }
        
        public IActionResult Feed(string address)
        {
            
            if (address==default)
            {
                if(User.Identity.IsAuthenticated)
                {
                    return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = GetMethodName()});
                }
                else 
                {
                    return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = "Welcome"});
                }
                
            }
            else
            {
                return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = GetMethodName()});
            }
            
            
        }

        public IActionResult Welcome()
        {
            return View("HomePages/UniversalNoneAddressPage",new UniversalAddressPage{Page = GetMethodName()});
        }

        public IActionResult Goal(string address)
        {
            
            return View("HomePages/UniversalAddressPage",new UniversalAddressPage{UserAddress = address,Page = GetMethodName()});
        }




        public IActionResult Error404()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}