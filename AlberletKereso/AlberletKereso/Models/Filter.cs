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
     
        [Display(Name = "Szobák minimális száma")]
        public int? Szobak_szama_min { get; set; }

        [Display(Name = "Szobák maximális száma")]
        public int? Szobak_szama_max { get; set; }

        

        [Display(Name = "Minimális alapterület")]
        public int? Alapterulet_min { get; set; }
        [Display(Name = "Maximális alapterület")]
        public int? Alapterulet_max { get; set; }

        [Display(Name = "Minimális ár")]
        public int? MinAr { get; set; }

        [Display(Name = "Maximális ár")]
        public int? MaxAr { get; set; }
        
        public  ApplicationUser feliratkozo { get; set; }
        public Filter(string cim, int szobak_min, int szobak_max, int alap_min, int alap_max, int minar,int maxar,  ApplicationUser user)
        {
            Cim = cim;
            Szobak_szama_min = szobak_min;
            Szobak_szama_max = szobak_max;
            Alapterulet_min = alap_min;
            Alapterulet_max = alap_max;
             MinAr = minar;
            MaxAr = maxar;
           
            feliratkozo = user;        

        }

        public Filter() { }

        public Filter(int maxAr)
        {
            MaxAr = maxAr;
        }

        public Filter(string cim, int? szobak_min, int? szobak_max, int? alap_min, int? alap_max, int? minAr, int? maxAr, ApplicationUser user)
        {
            Cim = cim;
            Szobak_szama_min = szobak_min;
            Szobak_szama_max = szobak_max;
            Alapterulet_min = alap_min;
            Alapterulet_max = alap_max;
            MinAr = minAr;
            MaxAr = maxAr;
            this.feliratkozo = user;
        }
    }

}