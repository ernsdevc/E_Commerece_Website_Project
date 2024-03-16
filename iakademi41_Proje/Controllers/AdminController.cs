using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi41_Proje.Models;
using iakademi41_Proje.MVVM;

namespace iakademi41_Proje.Controllers
{
    public class AdminController : Controller
    {
        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
        cls_Categories ck = new cls_Categories();
        cls_Suppliers cs = new cls_Suppliers();
        cls_Products cp = new cls_Products();
        cls_Settings cst = new cls_Settings();
        AnaSayfaModel asm = new AnaSayfaModel();

        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_Users usr)
        {
            tbl_Users us = db.tbl_Users.FirstOrDefault(u => u.Email == usr.Email && u.Password == usr.Password && u.active == true && u.isAdmin == true);

            if (us != null)
            {
                Session["UserID"] = us.UserID;
                return RedirectToAction("AnaSayfa");
            }
            return View();
        }

        public ActionResult AnaSayfa()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult KategoriGiris()
        {
            List<tbl_Categories> catlist = db.tbl_Categories.ToList();
            ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.CategoryName, Value = a.CategoryID.ToString() });
            return View();
        }

        [HttpPost]
        public ActionResult KategoriGiris(tbl_Categories cat)
        {
            bool sonuc = ck.KategoriEkle(cat);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Kategori Başarıyla Eklendi.";
            }
            else
            {
                Session["Mesaj"] = "Kategori Eklenemedi!";
            }
            return RedirectToAction("KategoriGiris");
        }

        public ActionResult UrunGiris()
        {
            List<tbl_Categories> catlist = db.tbl_Categories.ToList();
            ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.CategoryName, Value = a.CategoryID.ToString() });

            List<tbl_Suppliers> suplist = db.tbl_Suppliers.ToList();
            ViewData["suplist"] = suplist.Select(a => new SelectListItem { Text = a.BrandName, Value = a.SupplierID.ToString() });
            
            return View();
        }

        [HttpPost]
        public ActionResult UrunGiris(tbl_Products prd,HttpPostedFileBase fileuploader)
        {
            prd.PhotoPath = fileuploader.FileName;
            bool sonuc = cp.UrunEkle(prd);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Ürün Başarıyla Eklendi.";
            }
            else
            {
                Session["Mesaj"] = "Ürün Eklenemedi!";
            }
            return RedirectToAction("UrunGiris");
        }

        public ActionResult UrunListele()
        {
            asm.urunler = cls_Products.urunlergetir();
            return View(asm);
        }

        public ActionResult UrunGuncelle(int id)
        {
            List<tbl_Categories> catlist = db.tbl_Categories.ToList();
            ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.CategoryName, Value = a.CategoryID.ToString() });

            List<tbl_Suppliers> suplist = db.tbl_Suppliers.ToList();
            ViewData["suplist"] = suplist.Select(a => new SelectListItem { Text = a.BrandName, Value = a.SupplierID.ToString() });

            vw_products prd = db.vw_products.FirstOrDefault(p => p.ProductID == id);
            return View(prd);
        }

        [HttpPost]
        public ActionResult UrunGuncelle(tbl_Products prd, HttpPostedFileBase fileuploader)
        {
            tbl_Products prdt = db.tbl_Products.FirstOrDefault(p => p.ProductID == prd.ProductID);

            if (fileuploader != null)
            {
                prd.PhotoPath = fileuploader.FileName;
            }
            else
            {
                prd.PhotoPath = prdt.PhotoPath;
            }

            if (prdt.Stock == null)
            {
                prd.Stock = prdt.Stock;
            }

            prd.active = prdt.active;
            prd.AddDate = prdt.AddDate;
            prd.CokSatanlar = prdt.CokSatanlar;
            prd.OneCikanlar = prdt.OneCikanlar;

            bool sonuc = cp.UrunGuncelle(prd);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Ürün Başarıyla Güncellendi.";
                return RedirectToAction("UrunListele");
            }
            else
            {
                Session["Mesaj"] = "Ürün Güncellenemedi!";
                return View();
            }
        }

        public ActionResult UrunSil(int id)
        {
            bool sonuc = cp.UrunSil(id);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Ürün Başarıyla Silindi.";
            }
            else
            {
                Session["Mesaj"] = "Ürün Silinemedi!";
            }
            return RedirectToAction("UrunListele");
        }

        public ActionResult KategoriListele()
        {
            asm.kategoriler = cls_Categories.kategorigetir();
            return View(asm);
        }

        public ActionResult KategoriGuncelle(int id)
        {
            List<tbl_Categories> catlist = db.tbl_Categories.ToList();
            ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.CategoryName, Value = a.CategoryID.ToString() });

            vw_kategoriler ctgr = db.vw_kategoriler.FirstOrDefault(p => p.CategoryID == id);
            return View(ctgr);
        }

        [HttpPost]
        public ActionResult KategoriGuncelle(tbl_Categories ctgr)
        {
            tbl_Categories ctgry = db.tbl_Categories.FirstOrDefault(p => p.CategoryID == ctgr.CategoryID);

            bool sonuc = ck.KategoriGuncelle(ctgr);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Kategori Başarıyla Güncellendi.";
                return RedirectToAction("KategoriListele");
            }
            else
            {
                Session["Mesaj"] = "Kategori Güncellenemedi!";
                return View();
            }
        }

        public ActionResult KategoriSil(int id)
        {
            bool sonuc = ck.KategoriSil(id);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Kategori Başarıyla Silindi.";
            }
            else
            {
                Session["Mesaj"] = "Kategori Silinemedi!";
            }
            return RedirectToAction("KategoriListele");
        }

        public ActionResult MarkaGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MarkaGiris(tbl_Suppliers sup, HttpPostedFileBase fileuploader)
        {
            sup.PhotoPath = fileuploader.FileName;
            bool sonuc = cs.MarkaEkle(sup);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Marka Başarıyla Eklendi.";
            }
            else
            {
                Session["Mesaj"] = "Marka Eklenemedi!";
            }
            return RedirectToAction("MarkaGiris");
        }

        public ActionResult MarkaListele()
        {
            asm.markalar = cls_Suppliers.markalargetir();
            return View(asm);
        }

        public ActionResult MarkaGuncelle(int id)
        {
            List<tbl_Suppliers> suplist = db.tbl_Suppliers.ToList();
            ViewData["suplist"] = suplist.Select(a => new SelectListItem { Text = a.BrandName, Value = a.SupplierID.ToString() });

            vw_markalar mrk = db.vw_markalar.FirstOrDefault(p => p.SupplierID == id);
            return View(mrk);
        }

        [HttpPost]
        public ActionResult MarkaGuncelle(tbl_Suppliers mrk, HttpPostedFileBase fileuploader)
        {
            tbl_Suppliers mr = db.tbl_Suppliers.FirstOrDefault(p => p.SupplierID == mrk.SupplierID);

            if (fileuploader != null)
            {
                mrk.PhotoPath = fileuploader.FileName;
            }
            else
            {
                mrk.PhotoPath = mr.PhotoPath;
            }

            bool sonuc = cs.MarkaGuncelle(mrk);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Marka Başarıyla Güncellendi.";
                return RedirectToAction("MarkaListele");
            }
            else
            {
                Session["Mesaj"] = "Marka Güncellenemedi!";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("UserID");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AyarlarGuncelle()
        {
            tbl_Settings set = db.tbl_Settings.FirstOrDefault(s => s.ID == 1);
            return View(set);
        }

        [HttpPost]
        public ActionResult AyarlarGuncelle(tbl_Settings set)
        {
            bool sonuc = cst.AyarlarGuncele(set);
            if (sonuc == true)
            {
                Session["Mesaj"] = "Ayarlar Başarıyla Güncellendi.";
            }
            else
            {
                Session["Mesaj"] = "Ayarlar Güncellenemedi!";
            }
            return RedirectToAction("AnaSayfa");
        }
    }
}