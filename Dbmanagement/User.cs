using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LifeGoals.Daemons;
using lifeGoals.DataObjects;
using LifeGoals.Models;

namespace LifeGoals.Dbmanagement
{
    public static class UserManagement
    {
        public static string StandardUserImage = "/UserImages/standardUser.png";
        public static string StandardUserBackground = "/UserBackground/standardBackground.png";

        public static ApplicationUser GetUser(string userId)
        {
            ApplicationUser user = default;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user = db.Users.Single(u => u.Id == userId);
            }

            return user;

        }

        public static List<ApplicationUser> GetSubscribers(string address)
        {
            List<ApplicationUser> subscribers = new List<ApplicationUser>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var subObj = db.Subscriptions.Where(u => u.User == address & u.Status == true).ToList();

                foreach (var subscriber in subObj)
                {
                    subscribers.Add(db.Users.Single(u => u.Id == subscriber.Subscriber));
                }
            }

            return subscribers;
        }

        public static List<ApplicationUser> GetSubscriptions(string address)
        {
            List<ApplicationUser> Subscriptions = new List<ApplicationUser>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var subObj = db.Subscriptions.Where(u => u.Subscriber == address & u.Status == true).ToList();

                foreach (var subscriber in subObj)
                {
                    Subscriptions.Add(db.Users.Single(u => u.Id == subscriber.User));
                }
            }

            return Subscriptions;
        }

        public static void SetSubscriptionsStatus(string subscriber, string user,bool status)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                SubscriptionObjects subscriptionObjects = new SubscriptionObjects()
                    { Status = true, Subscriber = subscriber, User = user };
                
                if (status == true)
                {
                    if (subscriber!= user)
                    {
                        db.Subscriptions.Add(subscriptionObjects);
                        db.SaveChangesAsync();
                    }
                    
                }
                else
                {
                    var dbSubObj= 
                        db.Subscriptions.Single(subs => subs.Subscriber == subscriptionObjects.Subscriber & subs.User== subscriptionObjects.User);
                    
                    db.Subscriptions.Remove(dbSubObj);
                    db.SaveChangesAsync();
                }
                
            }
            
            
        }
        

        public static int GetSubscribersCount(string address)
        {
            int subscribersCount = 0;
            using (ApplicationDbContext db= new ApplicationDbContext())
            {
                subscribersCount = db.Subscriptions.Where(u => u.User == address & u.Status==true).ToList().Count;
            }

            return subscribersCount;
        }
        public static int GetSubscriptionsCount(string address)
        {
            int subscriptionsCount = 0;
            using (ApplicationDbContext db= new ApplicationDbContext())
            {
                subscriptionsCount = db.Subscriptions.Where(u => u.Subscriber == address & u.Status==true).ToList().Count;
                
                
            }

            return subscriptionsCount;
        }

        public static bool IsSubscriber(string address,string subscriberAddress)
        {
            bool isSubscriber = false;
            using (ApplicationDbContext db= new ApplicationDbContext())
            {
                var singleOrDefault = db.Subscriptions.SingleOrDefault(u => u.User == address & u.Subscriber==subscriberAddress & u.Status==true);
                
                if (singleOrDefault!=default)
                {
                    isSubscriber = true;
                }
                
            }

            return isSubscriber;
        }

        public static int GetGoalsCount(string address)
        {
            int goalsCount = 0;
            using (ApplicationDbContext db= new ApplicationDbContext())
            {
                goalsCount = db.Goals.Where(u => u.User == address).ToList().Count;
            }

            return goalsCount;
        }
        


        public static void ReplacementImageUser(string userId,string necessaryImage,string rootPath)
        {
            string oldUserImagePath = GetUser(userId).Imag;

            SetUserImage(userId,necessaryImage);
            
            if (oldUserImagePath!=StandardUserImage & File.Exists(rootPath+oldUserImagePath))
                File.Delete(rootPath+oldUserImagePath);
            

        }
        
        public static void ReplacementBackgroundUser(string userId,string necessaryImage,string rootPath)
        {
            string oldUserImagePath = GetUser(userId).Background;

            SetUserBackground(userId,necessaryImage);
            
            if (oldUserImagePath!=StandardUserBackground & File.Exists(rootPath+oldUserImagePath))
                File.Delete(rootPath+oldUserImagePath);

            
        }
        
        
        public static bool IsUserExists(string userId)
        {
            bool result = true;
            try
            {
                ApplicationUser user = default;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    user = db.Users.Single(u => u.Id == userId);
                }
            }
            catch (Exception e)
            {
                result = false;
            }
           

            return result;
    
        }


        private static void SetUserImage(string userId,string imagePath)
        {
            using (ApplicationDbContext db =new  ApplicationDbContext())
            {
               db.Users.Single(id => id.Id == userId).Imag=imagePath;
               db.SaveChangesAsync();
            }   
            
        }

        private static void SetUserBackground(string userId,string imagePath)
        {
            using (ApplicationDbContext db =new  ApplicationDbContext())
            {
                db.Users.Single(id => id.Id == userId).Background=imagePath;
                db.SaveChangesAsync();
            }   
            
        }
        
        public static void SetUserDescription(string userId,string description)
        {
            using (ApplicationDbContext db =new  ApplicationDbContext())
            {
                db.Users.Single(id => id.Id == userId).Description=description;
                db.SaveChangesAsync();
            }   
            
        }

    }


}