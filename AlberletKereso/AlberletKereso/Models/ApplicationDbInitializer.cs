using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlberletKereso.Models
{
    public class ApplicationDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName="95berta.balazs@gmail.com", Email="95berta.balazs@gmail.com"},
                new ApplicationUser { UserName="patrik5000@gmail.com", Email="patrik5000@gmail.com"},
                new ApplicationUser { UserName="moczardavid@gmail.com", Email="moczardavid@gmail.com"},
                new ApplicationUser { UserName="marci-95@hotmail.com", Email="marci-95@hotmail.com"}

            };
            Task<IdentityResult> createTask = UserManager.CreateAsync(users[0], "Temp_123");
            createTask.Wait(5000);
            createTask = UserManager.CreateAsync(users[1], "Temp_123");
            createTask.Wait(5000);
            createTask = UserManager.CreateAsync(users[2], "Temp_123");
            createTask.Wait(5000);
            createTask = UserManager.CreateAsync(users[3], "Temp_123");
            createTask.Wait(5000);
            users.ForEach(u => u.LockoutEnabled = true);
            users.ForEach(u => context.Users.Add(u));
            context.SaveChangesAsync();
           
        }
    }
}