using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class cls_Settings
    {
        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
        public bool AyarlarGuncele(tbl_Settings set)
        {
            try
            {
                tbl_Settings se = db.tbl_Settings.FirstOrDefault(s => s.ID == 1);
                se.telefon = set.telefon;
                se.sayfadakiurunsayisi = set.sayfadakiurunsayisi;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}