using System.Linq;
using LifeGoals.Models;

namespace LifeGoals.Dbmanagement
{
    public static class WebStats
    {
        public static int GetAccountCount()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Users.ToList().Count();
            }
        }
        
        public static int GetGoalsCount()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Goals.ToList().Count();
            }
        }

        public static uint Requests { get; set; } = 0;

    }
}