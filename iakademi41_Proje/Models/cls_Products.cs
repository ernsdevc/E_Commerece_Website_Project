using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class cls_Products
    {
        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
        List<vw_products> urunler;
        int page = 0;
        public bool UrunEkle(tbl_Products prd)
        {
            try
            {
                if (prd.Discount == null)
                {
                    prd.Discount = 0;
                }
                if (prd.Status == null)
                {
                    prd.Status = 0;
                }
                if (prd.Keywords == null)
                {
                    prd.Keywords = "";
                }
                if (prd.BunaBakanlar == null)
                {
                    prd.BunaBakanlar = 0;
                }
                if (prd.Notes == null)
                {
                    prd.Notes = "";
                }

                prd.OneCikanlar = 0;
                prd.CokSatanlar = 0;
                prd.AddDate = DateTime.Now;
                prd.active = true;
                db.tbl_Products.Add(prd);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<vw_products> urunler_getir(string listeturu, string ajaxmi, string pagenumber, int pcount)
        {

            if (listeturu == "slider")
            {
                urunler = db.vw_products.Where(p => p.Status == 4).ToList();
            }
            else if (listeturu == "enyeni")
            {
                if (ajaxmi == "")
                {
                    // Home/Index
                    urunler = db.vw_products.OrderByDescending(p => p.AddDate).Take(10).ToList();
                }
                else if (ajaxmi == "hayir")
                {
                    urunler = db.vw_products.OrderByDescending(p => p.AddDate).Take(pcount).ToList();
                }
                else
                {
                    page = Convert.ToInt32(pagenumber);
                    urunler = db.vw_products.OrderByDescending(p => p.AddDate).Skip(page * pcount).Take(pcount).ToList();
                }
            }
            else if (listeturu == "özel")
            {
                if (ajaxmi == "")
                {
                    // Home/Index
                    urunler = db.vw_products.Where(p => p.Status == 2).OrderBy(p => p.ProductName).Take(10).ToList();
                }
                else if (ajaxmi == "hayir")
                {
                    urunler = db.vw_products.Where(p => p.Status == 2).OrderBy(p => p.ProductName).Take(pcount).ToList();
                }
                else
                {
                    page = Convert.ToInt32(pagenumber);
                    urunler = db.vw_products.Where(p => p.Status == 2).OrderBy(p => p.ProductName).Skip(page * pcount).Take(pcount).ToList();
                }
            }
            else if (listeturu == "indirim")
            {
                if (ajaxmi == "")
                {
                    // Home/Index
                    urunler = db.vw_products.OrderByDescending(p => p.Discount).Take(10).ToList();
                }
                else if (ajaxmi == "hayir")
                {
                    urunler = db.vw_products.OrderByDescending(p => p.Discount).Take(pcount).ToList();
                }
                else
                {
                    page = Convert.ToInt32(pagenumber);
                    urunler = db.vw_products.Where(p => p.Discount != 0).OrderByDescending(p => p.Discount).Skip(page * pcount).Take(pcount).ToList();
                }
            }
            else if (listeturu == "önecikan")
            {
                if (ajaxmi == "")
                {
                    // Home/Index
                    urunler = db.vw_products.OrderByDescending(p => p.OneCikanlar).Take(10).ToList();
                }
                else if (ajaxmi == "hayir")
                {
                    urunler = db.vw_products.OrderByDescending(p => p.OneCikanlar).Take(pcount).ToList();
                }
                else
                {
                    page = Convert.ToInt32(pagenumber);
                    urunler = db.vw_products.OrderByDescending(p => p.OneCikanlar).Skip(page * pcount).Take(pcount).ToList();
                }
            }
            else if (listeturu == "çoksatan")
            {
                if (ajaxmi == "")
                {
                    // Home/Index
                    urunler = db.vw_products.OrderByDescending(p => p.CokSatanlar).Take(10).ToList();
                }
                else if (ajaxmi == "hayir")
                {
                    urunler = db.vw_products.OrderByDescending(p => p.CokSatanlar).Take(pcount).ToList();
                }
                else
                {
                    page = Convert.ToInt32(pagenumber);
                    urunler = db.vw_products.OrderByDescending(p => p.CokSatanlar).Skip(page * pcount).Take(pcount).ToList();
                }
            }
            else if (listeturu == "fırsat")
            {
                urunler = db.vw_products.Where(p => p.Status == 3).ToList();
            }
            return urunler;
        }
        
        public bool UrunGuncelle(tbl_Products prd)
        {
            try
            {
                db.Entry(prd).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UrunSil(int id)
        {
            try
            {
                tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.ProductID == id);
                db.tbl_Products.Remove(prd);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        tbl_Products urun;

        public tbl_Products urun_getir()
        {
            urun = db.tbl_Products.FirstOrDefault(p => p.Status == 1);
            return urun;
        }

        public void one_cikanlar_degeri_arttir(int id)
        {
            tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.ProductID == id);
            prd.OneCikanlar = prd.OneCikanlar + 1;
            db.SaveChanges();
        }

        public static List<vw_arama> aramagetir(string urunadi)
        {
            List<vw_arama> Arama = new List<vw_arama>();
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                Arama = db.vw_arama.Where(p => p.ARAMAISMI.Contains(urunadi)).Take(15).ToList();
            }
            return Arama;
        }

        public static vw_products urundetaygetir(int id)
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                vw_products prd = db.vw_products.FirstOrDefault(p => p.ProductID == id);
                return prd;
            }
        }

        public static List<vw_products> bunabakangetir(int id)
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
               List<vw_products> prd = db.vw_products.Where(p => p.BunaBakanlar == id).ToList();
                return prd;
            }
        }

        public static List<vw_products> benzerurungetir(int id)
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                List<vw_products> prd = db.vw_products.Where(p => p.CategoryID == id).ToList();
                return prd;
            }
        }

        public static List<vw_products> urunlergetir()
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                List<vw_products> prd = db.vw_products.OrderBy(p => p.ProductID).ToList();
                return prd;
            }
        }
    }
}