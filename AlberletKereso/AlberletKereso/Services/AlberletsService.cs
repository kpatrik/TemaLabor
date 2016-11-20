using AlberletKereso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AlberletKereso.Services
{
    public class AlberletsService
    {
        private UnitOfWork unitOfWork;

        public AlberletsService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int DefineMinSzoba(int? minSzoba)
        {
            if (minSzoba == null) return 0;
            else return (int)minSzoba;
        }

        public int DefineMaxSzoba(int? maxSzoba)
        {
            if (maxSzoba == null) return int.MaxValue;
            else return (int)maxSzoba;
        }

        public int DefineMinTerulet(int? minTerulet)
        {
            if (minTerulet == null) return 0;
            else return (int)minTerulet;
        }

        public int DefineMaxTerulet(int? maxTerulet)
        {
            if (maxTerulet == null) return int.MaxValue;
            else return (int)maxTerulet;
        }

        public int DefineMinAr(int? minAr)
        {
            if (minAr == null) return 0;
            else return (int)minAr;
        }

        public int DefineMaxAr(int? maxAr)
        {
            if (maxAr == null) return int.MaxValue;
            else return (int)maxAr;
        }

        public bool CheckAddress(String address)
        {
            return String.IsNullOrEmpty(address);
        }

        public bool CheckID(int? id)
        {
            if (id == null) return true;
            else return false;
        }

        public bool CheckAlberlet(Alberlet alberlet)
        {
            if (alberlet == null) return true;
            else return false;
        }

        public IEnumerable<Alberlet> GetAlberlets(String search, int? minSzoba, int? maxSzoba, 
            int? minTerulet, int? maxTerulet, int? minAr, int? maxAr)
        {
            var alberletek = unitOfWork.AlberletRepository.Get();

            alberletek = unitOfWork.AlberletRepository.Get(filter: f => f.Alapterulet >= minTerulet & f.Alapterulet <= maxTerulet &
                                                                           f.Szobak_szama >= minSzoba & f.Szobak_szama <= maxSzoba &
                                                                           f.Ar >= minAr & f.Ar <= maxAr);

            if (!CheckAddress(search))
            {
                alberletek = alberletek.Where(s => s.Cim.Contains(search));
            }
            return alberletek;
        }

        public IEnumerable<Kep> GetKepek(int id)
        {
            return unitOfWork.KepRepository.Get(filter: f => f.Alberlet.AlberletId == id);
        }

        public Alberlet GetAlberlet(int? ID)
        {
            return unitOfWork.AlberletRepository.GetByID(ID);
        }
    }
}