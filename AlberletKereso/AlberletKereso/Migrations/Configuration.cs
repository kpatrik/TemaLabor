namespace AlberletKereso.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AlberletKereso.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            
        }

        protected override void Seed(AlberletKereso.Models.ApplicationDbContext context)
        {
            
            if (!(context.Users.Any(u => u.UserName == "95berta.balazs@gmail.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser {
                    UserName = "95berta.balazs@gmail.com", Email = "95berta.balazs@gmail.com" };
                userManager.Create(userToInsert, "Valami_123");
            }
            if (!(context.Users.Any(u => u.UserName == "patrik5000@gmail.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "patrik5000@gmail.com", Email = "patrik5000@gmail.com" };
                userManager.Create(userToInsert, "Valami_123");
            }
            if (!(context.Users.Any(u => u.UserName == "moczardavid@gmail.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "moczardavid@gmail.com", Email = "moczardavid@gmail.com" };
                userManager.Create(userToInsert, "Valami_123");
            }
            if (!(context.Users.Any(u => u.UserName == "marci-95@hotmail.com")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "marci-95@hotmail.com", Email = "marci-95@hotmail.com" };
                userManager.Create(userToInsert, "Valami_123");
            }
            if(!(context.Alberletek.Any(a=> a.AlberletId==1)))
            {
                var user = context.Users.Where(u=> u.UserName == "marci-95@hotmail.com").FirstOrDefault();
                  var albi = new Alberlet("Arany utca 9.", 2, 0, 1, 23, 50000, false,user);
                context.Alberletek.Add(albi);
                context.SaveChanges();
            }
            if (!(context.Alberletek.Any(a => a.AlberletId == 1)))
            {
                var user = context.Users.Where(u => u.UserName == "moczardavid@gmail.com").FirstOrDefault();
                var albi = new Alberlet("Madách utca 2.", 3, 2, 1, 50, 100000, false, user);
                context.Alberletek.Add(albi);
                context.SaveChanges();
            }

        }
    }
}
