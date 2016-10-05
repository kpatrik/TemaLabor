using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlberletKereso.Models
{
    public class Alberlet
    {
        public Alberlet() { }


        public int AlberletId { get; set; }

        [Required]
        [Display(Name = "Cím")]
        public string Cim { get; set; }

        [Display(Name = "Szobák száma")]
        public int Szobak_szama { get; set; }

        
        public int Emelet { get; set; }

        [Display(Name = "Mosdók száma")]
        public int Mosdok_szama { get; set; }

        [Display(Name = "Alapterület")]
        public int Alapterulet { get; set; }

        [Display(Name = "Ár")]
        public int Ar { get; set; }
        public bool Berendezett { get; set; }
        public ApplicationUser Hirdeto { get; set; }

    }
}