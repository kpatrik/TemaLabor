using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlberletKereso.Models
{

    public class Filter
    {
       

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterId { get; set; }

        [Display(Name = "Cím")]
        
        public string Cim { get; set; }
     
        [Display(Name = "Szobák száma")]
        public int? Szobak_szama { get; set; }

        public int? Emelet { get; set; }

        [Display(Name = "Mosdók száma")]
        public int? Mosdok_szama { get; set; }

        [Display(Name = "Alapterület")]
        public int? Alapterulet { get; set; }

        [Display(Name = "Minimális ár")]
        public int? MinAr { get; set; }

        [Display(Name = "Maximális ár")]
        public int? MaxAr { get; set; }
        public bool? Berendezett { get; set; }
        
        public  ApplicationUser feliratkozo { get; set; }
        public Filter(string cim, int szobak, int emelet, int mosdok, int alap, int minar,int maxar, bool berendezett, ApplicationUser user)
        {
            Cim = cim;
            Szobak_szama = szobak;
            Emelet = emelet;
            Mosdok_szama = mosdok;
            Alapterulet = alap;
            MinAr = minar;
            MaxAr = maxar;
            Berendezett = berendezett;
            feliratkozo = user;        

        }

        public Filter() { }

        public Filter(int maxAr)
        {
            MaxAr = maxAr;
        }

        public Filter(string cim, int? szobak_szama, int? emelet, int? mosdok_szama, int? alapterulet, int? minAr, int? maxAr, bool? berendezett, ApplicationUser user)
        {
            Cim = cim;
            Szobak_szama = szobak_szama;
            Emelet = emelet;
            Mosdok_szama = mosdok_szama;
            Alapterulet = alapterulet;
            MinAr = minAr;
            MaxAr = maxAr;
            Berendezett = berendezett;
            this.feliratkozo = user;
        }
    }

}