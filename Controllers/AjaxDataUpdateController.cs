using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using lifeGoals.Cryptocurrencies.Ethereum;
using LifeGoals.Dbmanagement;
using LifeGoals.PageObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeGoals.Controllers
{
    public class AjaxDataUpdateController:Controller
    {
        public IActionResult GetSubscriptionStatus(string address,string pageVisitor)
        {
            WebStats.Requests++;
            var pageStatus = new BasicView { PageVisitor = pageVisitor, UserAddress = address }.GetPageStatus();

           
                
            return PartialView("Profile/UserAllData", new UniversalAddressPage { UserAddress = address, PageVisitor = pageVisitor, PageStatus = pageStatus });
        }

        public IActionResult GetSubscribers(string address)
        {
            WebStats.Requests++;
            var allUserSubscribers = UserManagement.GetSubscribers(address);
            
            return PartialView("Profile/UserListDialog",new UserListDialog(){Users = allUserSubscribers,IsOpen = true});
        }
        
        public IActionResult GoalLineUpdate(string address)
        {
            WebStats.Requests++;
            return PartialView("Profile/GoalLine",address);
        }
        
        public IActionResult GetSubscriptions(string address)
        {
            WebStats.Requests++;
            var allUserSubscriptions = UserManagement.GetSubscriptions(address);
            
            return PartialView("Profile/UserListDialog",new UserListDialog(){Users = allUserSubscriptions,IsOpen = true});
        }
        
        public async Task<ActionResult> UpdateGoals(string address,int scrollNumber,EPageStatus status)
        {
            WebStats.Requests++;
            return PartialView("Goal/GetAllGoals",new AllGoalsScroll(){Address =  address, ScrollNumber = scrollNumber,PageStatus = status});
        }
        public async Task<ActionResult> UpdateFeedGoals(string address,int scrollNumber,EPageStatus status)
        {
            WebStats.Requests++;
            return PartialView("Goal/GetUserFeed",new AllGoalsScroll(){Address =  address, ScrollNumber = scrollNumber,PageStatus = status});
        }
        
        public async Task<ActionResult> Goal(int goalId,EPageStatus status)
        {
            WebStats.Requests++;
            var goal = Goals.GetGoal(goalId);
           
            return PartialView("Goal/Goal",new GoalAndStatusObjects(){ PageStatus = status, GoalObjects = goal});
        }

        [Authorize] 
        public async Task<ActionResult> GoalAdd(string body,string title,bool isDonate,string donateValue,string address,bool isGoal)
        {
            WebStats.Requests++;
            GoalObjects goalObjects = new GoalObjects(); 
            await Task.Run(() =>
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(isDonate)
                {
                    double dDonateValue = 0;
                    try
                    {
                        dDonateValue = Convert.ToDouble(donateValue, new CultureInfo("en-us"));
                    }
                    catch (Exception e)
                    {
                        dDonateValue = 0;
                    }

                    goalObjects = new GoalObjects()
                    {
                        Body = body, Titles = title,
                        User = userId, IsDonate = true,
                        DonateValue = dDonateValue.ToString("0.########", new CultureInfo("en-us")),
                        PublicAddress = address,
                        IsPrivateDonateGoal = true,
                        PrivateKey = $"https://etherscan.io/address/{address}",
                        MaxDonateValue = "0"
                    };

                    Goals.GoalAddDb(goalObjects);
                }
                else if(isGoal)
                {
                    goalObjects = new GoalObjects
                    {
                        Body = body, Titles = title, User = userId,
                        StageImplementation = EGoalStageImplementation.created
                    };
                    
                    Goals.GoalAddDb(goalObjects);
                }
                else
                {
                    
                    goalObjects = new GoalObjects
                    {
                        Body = body, Titles = title, User = userId,
                        StageImplementation = (EGoalStageImplementation)(3)
                    };
                    
                    Goals.GoalAddDb(goalObjects);
                }

                
            });
            
           
            
            return PartialView("Goal/Goal",new GoalAndStatusObjects(){PageStatus = EPageStatus.Owner,GoalObjects = goalObjects});
        }
        
        [Authorize]
        public  ActionResult ChangeGoalStatus(EGoalStageImplementation status,int goalId)
        {
            WebStats.Requests++;
            var goal = Goals.GetGoal(goalId);
          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                

            if (goal.User == userId)
            {
                new Thread(() =>
                    { Goals.ChangeGoalStatus(status, goalId); }).Start();
                    
                    
                goal.StageImplementation = status;
            }
               

           
            return PartialView("Goal/Goal",new GoalAndStatusObjects(){PageStatus = EPageStatus.Owner,GoalObjects = goal});
        }
        
        [Authorize] 
        public IActionResult DoImportant(bool status,int goalId)
        {
            WebStats.Requests++;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var goal = Goals.GetGoal(goalId);

            if (goal.User == userId)
            {
               
                Goals.DoImportant(goalId,status);
             
               
                goal.Important = status;
            }

            


            return PartialView("Goal/Goal",new GoalAndStatusObjects(){PageStatus = EPageStatus.Owner,GoalObjects = goal});
        }
        
        
        [Authorize] 
        public IActionResult SetSubscriptionsStatus(bool status,string subscription)
        {
            WebStats.Requests++;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(status);
            
            UserManagement.SetSubscriptionsStatus(userId, subscription, status);

            return PartialView("Profile/UserAllData",new UniversalAddressPage(){PageStatus = EPageStatus.Authorized, PageVisitor = userId, UserAddress = subscription});
        }
    }
}