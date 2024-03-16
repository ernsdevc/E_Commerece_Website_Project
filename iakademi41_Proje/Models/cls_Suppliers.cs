using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class cls_Suppliers
    {
        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
        public bool MarkaEkle(tbl_Suppliers sup)
        {
            try
            {
                sup.active = true;
                db.tbl_Suppliers.Add(sup);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<vw_markalar> markalargetir()
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                List<vw_markalar> mrk = db.vw_markalar.OrderBy(p => p.SupplierID).ToList();
                return mrk;
            }
        }

        public bool MarkaGuncelle(tbl_Suppliers mrk)
        {
            try
            {
                db.Entry(mrk).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MarkaSil(int id)
        {
            try
            {
                tbl_Suppliers mrk = db.tbl_Suppliers.FirstOrDefault(p => p.SupplierID == id);
                db.tbl_Suppliers.Remove(mrk);
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