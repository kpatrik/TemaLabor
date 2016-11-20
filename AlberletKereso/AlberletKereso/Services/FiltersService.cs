using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AlberletKereso.Services
{
    public class FiltersService
    {
        private UnitOfWork unitOfWork;

        public FiltersService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool CheckID(int? id)
        {
            if (id == null) return true;
            else return false;
        }

        public Models.Filter GetFilter(int id)
        {
            return unitOfWork.FilterRepository.GetByID(id);
        }

        public void CreateFilter(string id, Models.Filter filter)
        {
            var user = unitOfWork.UserManager.FindById(id);
            var ujszuro = new Models.Filter(filter.Cim, filter.Szobak_szama, filter.Emelet, 
                filter.Mosdok_szama, filter.Alapterulet, filter.MinAr, 
                filter.MaxAr, filter.Berendezett, user);
            user.Filters.Add(ujszuro);
            unitOfWork.UserManager.Update(user);
            unitOfWork.Save();
        }

        public void UpdateFilter(Models.Filter filter)
        {
            unitOfWork.FilterRepository.Update(filter);
            unitOfWork.Save();
        }

        public void DeleteFilter(Models.Filter filter)
        {
            unitOfWork.FilterRepository.Delete(filter);
            unitOfWork.Save();
        }
    }
}