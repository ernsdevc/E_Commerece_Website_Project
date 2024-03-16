using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class cls_Categories
    {
        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();

        public static List<vw_kategoriler> kategorigetir()
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                List<vw_kategoriler> ctgr = db.vw_kategoriler.OrderBy(p => p.CategoryID).ToList();
                return ctgr;
            }
        }

        public bool KategoriEkle(tbl_Categories cat)
        {
            try
            {
                if (cat.ParentID == null)
                {
                    cat.ParentID = 0;
                }
                cat.active = true;
                db.tbl_Categories.Add(cat);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool KategoriGuncelle(tbl_Categories ctgr)
        {
            try
            {
                db.Entry(ctgr).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool KategoriSil(int id)
        {
            try
            {
                tbl_Categories ctgr = db.tbl_Categories.FirstOrDefault(p => p.CategoryID == id);
                db.tbl_Categories.Remove(ctgr);
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