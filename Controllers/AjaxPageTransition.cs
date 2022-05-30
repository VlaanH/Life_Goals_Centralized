using System;
using System.Security.Claims;
using lifeGoals.Cryptocurrencies.Ethereum;
using LifeGoals.Dbmanagement;
using LifeGoals.PageObjects;
using Microsoft.AspNetCore.Mvc;

namespace LifeGoals.Controllers
{
    public class AjaxPageTransitionController:Controller
    {
        public IActionResult Privacy()
        {
            WebStats.Requests++;
            return PartialView("Pages/Privacy");
        }


        public IActionResult Profile(string address = default)
        {
            WebStats.Requests++;
            
            string addressVisitor= User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            
            var pageStatus = new BasicView { PageVisitor = addressVisitor, UserAddress = address }.GetPageStatus();
            

            if (pageStatus == EPageStatus.NotFound)
            {
                return PartialView("Pages/Error404");
            }
            else if (pageStatus == EPageStatus.NotAuthorized)
            {
                return PartialView("Pages/Profile", new UniversalAddressPage { UserAddress = address, PageVisitor = addressVisitor, PageStatus = pageStatus });
            }
            else
            {
                return PartialView("Pages/Profile", new UniversalAddressPage { UserAddress = address, PageVisitor = addressVisitor, PageStatus = pageStatus });
            }
        }
        public IActionResult Feed(string address = default)
        {
            WebStats.Requests++;
            
            string addressVisitor= User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var pageStatus = new BasicView { PageVisitor = addressVisitor, UserAddress = address }.GetPageStatus();


            if (pageStatus == EPageStatus.NotFound)
            {
                return PartialView("Pages/Error404");
                
            }
            else if (pageStatus == EPageStatus.NoAccount)
            {
                return Redirect("Identity/Account/Register");
            }
            else
            {
                return PartialView("Pages/Feed", new UniversalAddressPage { UserAddress = address, PageVisitor = addressVisitor, PageStatus = pageStatus });
            }
           
        }

        public IActionResult Goal(string address = default)
        {
            WebStats.Requests++;
            
            string addressVisitor= User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try
            {
                var goalObjects = Goals.GetGoal(Int32.Parse(address));
                
                var pageStatus = new BasicView { PageVisitor = addressVisitor, UserAddress = goalObjects.User }.GetPageStatus();

            
                return PartialView("Pages/Goal", new GoalAndStatusObjects { PageStatus = pageStatus, GoalObjects = goalObjects});
            }
            catch (Exception)
            {
                return PartialView("Pages/Error404"); 
            }
            
        }

        public IActionResult Welcome()
        {
            WebStats.Requests++;
            return PartialView("Welcome");
        }

        public IActionResult Settings()
        {
            WebStats.Requests++;
            return PartialView("Pages/Settings");
        }

        public IActionResult Error404()
        {
            WebStats.Requests++;
            return PartialView("Pages/Error404");
        }
     
    }
}