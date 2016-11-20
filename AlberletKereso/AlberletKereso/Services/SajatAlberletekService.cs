using AlberletKereso.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AlberletKereso.Services
{
    public class SajatAlberletekService
    {
        private UnitOfWork unitOfWork;

        public SajatAlberletekService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Alberlet> GetAlberletek(string id)
        {
            var userID = unitOfWork.UserManager.FindById(id).Id;
            return unitOfWork.AlberletRepository.Get(filter: f => f.Hirdeto.Id == userID);
        }

        public bool CheckID(int? id)
        {
            if (id == null) return true;
            else return false;
        }

        public Alberlet GetAlberlet(int id)
        {
            return unitOfWork.AlberletRepository.GetByID(id);
        }

        public void UploadImage(HttpPostedFileBase file, Alberlet alberlet, HttpContextBase context)
        {
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(context.Server.MapPath("~/Uploads/"), fileName);
            file.SaveAs(path);
            Image img = alberlet.resizeImage(640, 480, path);
            img.Save(path);
            Kep kep = new Models.Kep(path, fileName, alberlet);
            alberlet.Kepek.Add(kep);
            unitOfWork.Save();
        }

        public bool CheckExtension(HttpPostedFileBase file)
        {
            string extension = Path.GetExtension(file.FileName);
            if (extension.Equals(".jpeg") || extension.Equals(".png") || extension.Equals(".jpg"))
                return true;
            else return false;
        }

        public void UploadAlberlet(Alberlet model, string userID)
        {
            var user = unitOfWork.UserManager.FindById(userID);
            var ujalberlet = new Alberlet(model.Cim, model.Szobak_szama, 
                                          model.Emelet, model.Mosdok_szama, model.Alapterulet, 
                                          model.Ar, model.Berendezett, user);
            user.Hirdetesek.Add(ujalberlet);
            unitOfWork.UserManager.Update(user);
            IterateUsers(ujalberlet);
            unitOfWork.Save();
        }

        public void IterateUsers(Alberlet alberlet)
        {
            foreach (var ite2 in unitOfWork.UserManager.Users)
            {
                var filters = unitOfWork.FilterRepository.Get(filter: f => f.feliratkozo.Id == ite2.Id);

                ite2.Filters = filters.ToList();
                foreach (Models.Filter item in ite2.Filters)
                {
                    if (
                        (item.Alapterulet == null || item.Alapterulet <= alberlet.Alapterulet)
                            && (item.Berendezett == null || item.Berendezett.Equals(alberlet.Berendezett) || !item.Berendezett.HasValue)
                            && (item.Cim == null || item.Cim.Equals(alberlet.Cim))
                            && (item.Emelet == null || item.Emelet <= alberlet.Emelet)
                            && (item.MaxAr == null || item.MaxAr >= alberlet.Ar)
                            && (item.MinAr == null || alberlet.Ar <= item.MinAr)
                            && (item.Mosdok_szama == null || item.Mosdok_szama <= alberlet.Mosdok_szama)
                            && (item.Szobak_szama == null || item.Szobak_szama <= alberlet.Szobak_szama)
                            && ite2.Id != alberlet.Hirdeto.Id) { }
                    SendMail(ite2, alberlet);
                }
            }
        }

        public void SendMail(ApplicationUser user, Alberlet alberlet)
        {
            MailMessage o = new MailMessage("temalabor@hotmail.com", user.Email, "Találat",
                "Megtaláltuk a neked megfelelő albérletet! \n " +
                "Az albérlet adatai: \n" +
                "Cím: " + alberlet.Cim + "\n" +
                "Szobák száma: " + alberlet.Szobak_szama + "\n" +
                "Emelet: " + alberlet.Emelet + "\n" +
                "Mosdok száma: " + alberlet.Mosdok_szama + "\n" +
                "Alapterület: " + alberlet.Alapterulet + "\n" +
                "Ár: " + alberlet.Ar);
            NetworkCredential netCred = new NetworkCredential("temalabor@hotmail.com", "valami123");
            SmtpClient smtpobj = new SmtpClient("smtp.live.com", 587);
            smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            smtpobj.Send(o);
        }

        public void DeleteAlberlet(int id)
        {
            Alberlet alberlet = unitOfWork.AlberletRepository.GetByID(id);
            unitOfWork.AlberletRepository.Delete(alberlet);
            unitOfWork.Save();
        }
    }
}