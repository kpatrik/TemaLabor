using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlberletKereso.Models
{
    public class Kep { 
        public Kep() { }

        public Kep(string url, string filename, Alberlet alberlet)
        {
            Url = url;
            Alberlet = alberlet;
            FileName = filename;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KepId { get; set; }

        [Required]
        public string Url { get; set; }

        public string FileName { get; set; }
   
        public Alberlet Alberlet { get; set; }

    }
}