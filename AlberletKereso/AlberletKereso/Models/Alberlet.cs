using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace AlberletKereso.Models
{
    
    public class Alberlet
    {
        [Display(Name = "Képek")]
        public ICollection<Kep> Kepek { get; set; }

        public Alberlet() {
            Kepek = new List<Kep>();
        }

        public Alberlet(string cim, int szobak, int emelet, int mosdok,int alap, int ar, bool berendezett, ApplicationUser user) {
            Cim = cim;
            Szobak_szama = szobak;
            Emelet = emelet;
            Mosdok_szama = mosdok;
            Alapterulet = alap;
            Ar = ar;
            Berendezett = berendezett;
            Hirdeto = user;
            Kepek = new List<Kep>();
            
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = Image.FromFile(stPhotoPath,true);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

       
    }
   
}