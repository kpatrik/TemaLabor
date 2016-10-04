using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlberletKereso.Models
{
    public class Alberlet
    {
        public Alberlet() { }


        public int AlberletId { get; set; }
        public string Cim { get; set; }
        public int Szobak_szama { get; set; }
        public int Emelet { get; set; }
        public int Mosdok_szama { get; set; }
        public int Alapterulet { get; set; }
        public int Ar { get; set; }
        public bool Berendezett { get; set; }
        public ApplicationUser Hirdeto { get; set; }

    }
}