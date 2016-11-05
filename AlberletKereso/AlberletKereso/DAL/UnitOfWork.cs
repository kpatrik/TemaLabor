using AlberletKereso;
using AlberletKereso.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Microsoft.Owin.Host.SystemWeb;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private GenericRepository<Alberlet> alberletRepository;
        private GenericRepository<Filter> filterRepository;
        private GenericRepository<Kep> kepRepository;
        private GenericRepository<ApplicationUser> userRepository;
        private ApplicationUserManager userManager;



    public ApplicationUserManager UserManager {

        get {

            if(userManager == null)
                userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            return userManager;
        }

    }


    public GenericRepository<Alberlet> AlberletRepository
    {
            get
            {

                if (this.alberletRepository == null)
                {
                    this.alberletRepository = new GenericRepository<Alberlet>(context);
                }
                return alberletRepository;
            }
        }

        public GenericRepository<Filter> FilterRepository
        {
            get
            {

                if (this.filterRepository == null)
                {
                    this.filterRepository = new GenericRepository<Filter>(context);
                }
                return filterRepository;
            }
        }

        public GenericRepository<Kep> KepRepository
        {
            get
            {

                if (this.kepRepository == null)
                {
                    this.kepRepository = new GenericRepository<Kep>(context);
                }
                return kepRepository;
            }
        }

        public GenericRepository<ApplicationUser> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<ApplicationUser>(context);
                }
                return userRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
