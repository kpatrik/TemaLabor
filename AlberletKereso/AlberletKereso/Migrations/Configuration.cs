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
            initUser(context,"95berta.balazs@gmail.com");
            initUser(context, "patrik5000@gmail.com");
            initUser(context, "moczardavid@gmail.com");
            initUser(context, "marci"+"-"+"95"+"@hotmail.com");
            initUser(context, "kaki@kaki.com");


            if(!(context.Alberletek.Any(a=> a.AlberletId==1)))
              {
                  var user = context.Users.Where(u=> u.UserName == "kaki@kaki.com").FirstOrDefault();
                    var albi = new Alberlet("Arany utca 9.", 2, 0, 1, 23, 50000, false,user);
                  context.Alberletek.Add(albi);

              }
              if (!(context.Alberletek.Any(a => a.AlberletId == 1)))
              {
                  var user = context.Users.Where(u => u.UserName == "moczardavid@gmail.com").FirstOrDefault();
                  var albi = new Alberlet("Madách utca 2.", 3, 2, 1, 50, 100000, false, user);
                  context.Alberletek.Add(albi);

              }

        }
        private void initUser(AlberletKereso.Models.ApplicationDbContext context,string mail)
        {
            if (!(context.Users.Any(u => u.UserName .Equals(mail) )))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = mail , Email = mail };
                userManager.Create(userToInsert, "Password@123");
                userManager.Dispose();
            }
        }
    }
    
}
