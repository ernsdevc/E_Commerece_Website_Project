using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iakademi41_Proje.Models;
using iakademi41_Proje.MVVM;
using PagedList;
using PagedList.Mvc;

namespace iakademi41_Proje.Controllers
{
    public class HomeController : BaseController
    {
        // GET:
        // EnYeniler = products tarih kolonu
        // günün ürünü = products status => 1
        // özeller = satatus => 2
        // İndirimli = products indirim kolonu
        // öne çıkanlar = products onecikanlar kolonu
        // cok satanlar = 

        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
        AnaSayfaModel asm = new AnaSayfaModel();
        cls_Products cp = new cls_Products();
        cls_Users cu = new cls_Users();
        int pcount = 1;

        public HomeController()
        {
            pcount = ViewBag.sayfadakiurunsayisi;
        }

        public ActionResult Index()
        {
            asm.slider_urunler = cp.urunler_getir("slider", "", "", pcount);
            asm.enyeni_urunler = cp.urunler_getir("enyeni", "", "", pcount);
            asm.ozel_urunler = cp.urunler_getir("özel", "", "", pcount);
            asm.indirimli_urunler = cp.urunler_getir("indirim", "", "", pcount);
            asm.onecikan_urunler = cp.urunler_getir("önecikan", "", "", pcount);
            asm.coksatan_urunler = cp.urunler_getir("çoksatan", "", "", pcount);
            asm.firsat_urunler = cp.urunler_getir("fırsat", "", "", pcount);
            asm.gunun_urunu = cp.urun_getir();
            return View(asm);
        }

        // sayfalarda sepete ekle butonuna basınca cookiye ProductID ekleyecek
        public ActionResult Sepet(int id)
        {   
            cp.one_cikanlar_degeri_arttir(id);         
            HttpCookie cSetCookie;
            string sepet = "";
            cls_Orders o = new cls_Orders();
            o.ProductID = id;
            o.Count = 1;

            cSetCookie = Request.Cookies.Get("sepetim");

            if (cSetCookie == null)
            {
                cSetCookie = new HttpCookie("sepetim");
                o.sepet = sepet;
            }
            else
            {
                sepet = cSetCookie.Value;
                o.sepet = sepet;
            }

            if (o.add_tocart(id) == true)
            {
                // ürün sepette yok - ekle
                cSetCookie.Values.Add(id.ToString(), "1");
                cSetCookie.Expires = DateTime.Now.AddMinutes(60 * 24 * 7); // 1 haftalık cookie
                Response.Cookies.Add(cSetCookie); // cookie tarayıcıya gönderme 
                string url = Request.UrlReferrer.ToString();
                if (url.Length <= 24)
                {
                    Session["Mesaj"] = "Ürün Sepetinize Eklendi";
                    return RedirectToAction("Index");
                }                
                if (url.Contains("dproducts"))
                {
                    return RedirectToAction("DetayliArama");
                }
                else
                {
                    Session["Mesaj"] = "Ürün Sepetinize Eklendi";
                    string sil = url.Substring(0, 29);
                    url = url.Replace(sil, "");
                    return RedirectToAction(url);
                }                
            }
            else
            {
                // ürün sepette var
                string url = Request.UrlReferrer.ToString();
                if (url.Length <= 24)
                {
                    Session["Mesaj"] = "Bu ürün zaten sepetinizde bulunuyor.";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Mesaj"] = "Bu ürün zaten sepetinizde bulunuyor.";
                    string sil = url.Substring(0, 29);
                    url = url.Replace(sil, "");
                    return RedirectToAction(url);
                }
            }
        }

        // sepete basınca gidilecek sayfa
        public ActionResult Cart()
        {
            // sepet sayfası
            cls_Orders o = new cls_Orders();

            // sepetten ürünü silerek gelirse
            if (Request.QueryString["scid"] != null)
            {
                int scid = Convert.ToInt32(Request.QueryString["scid"]);
                HttpCookie cSetCookie = Request.Cookies.Get("sepetim");
                string sepetcookie = cSetCookie.Value;
                o.sepet = sepetcookie;
                o.sepetten_sil(scid);
                HttpCookie cCookie = new HttpCookie("sepetim", o.sepet);
                Response.Cookies.Add(cCookie);
                cCookie.Expires = DateTime.Now.AddMinutes(144);
                Session["Mesaj"] = "Ürün Sepetinizden Silindi.";
                List<cls_Orders> sepet = o.sepetiyazdir();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                // Sepetime Gitten gelirse
                HttpCookie cSetCookie = Request.Cookies.Get("sepetim");
                if (cSetCookie == null)
                {
                    cSetCookie = new HttpCookie("sepetim");
                    o.sepet = "";
                    ViewBag.Sepetim = o.sepetiyazdir();
                    ViewBag.sepet_tablo_detay = o.sepetiyazdir();
                }
                else
                {
                    o.sepet = cSetCookie.Value.ToString();
                    ViewBag.Sepetim = o.sepetiyazdir();
                    ViewBag.sepet_tablo_detay = o.sepetiyazdir();
                }
            }

            return View();
        }

        public ActionResult Detaylar(int id)
        {
            asm.urundetay = cls_Products.urundetaygetir(id);
            asm.bunabakanlar_urunler = cls_Products.bunabakangetir(Convert.ToInt32(asm.urundetay.BunaBakanlar));
            asm.benzerurunler = cls_Products.benzerurungetir(id);
            cp.one_cikanlar_degeri_arttir(id);
            return View(asm);
        }

        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(tbl_Users usr)
        {
            tbl_Users us = db.tbl_Users.FirstOrDefault(u => u.Email == usr.Email && u.Password == usr.Password && u.active == true);

            if (us != null)
            {
                if (us.isAdmin == true)
                {
                Session["UserID"] = us.UserID;
                return RedirectToAction("AnaSayfa","Admin");
                }
                else
                {
                    Session["UserID"] = us.UserID;
                    Session["NameSurname"] = us.NameSurname;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                Session["Mesaj"] = "Email ya da Şifreniz yanlış!";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("UserID");
            return RedirectToAction("Index");
        }

        public ActionResult Kategoriler(int id, int? page)
        {
            var plist = db.vw_products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();
            var pagenumber = page ?? 1; // page == null ise page == 1
            var pageddata = plist.ToPagedList(pagenumber, pcount);
            ViewBag.Kategoriler = pageddata;
            ViewBag.CategoryName = db.tbl_Categories.FirstOrDefault(c => c.CategoryID == id).CategoryName;
            return View();
        }

        public ActionResult Markalar(int id, int? page)
        {
            var plist = db.vw_products.Where(p => p.SupplierID == id).OrderBy(p => p.ProductName).ToList();
            var pagenumber = page ?? 1; // page == null ise page == 1
            var pageddata = plist.ToPagedList(pagenumber, pcount);
            ViewBag.Markalar = pageddata;
            ViewBag.SupplierName = db.tbl_Suppliers.FirstOrDefault(s => s.SupplierID == id).BrandName;
            return View();
        }

        public ActionResult UyeOl()
        {
            return View();
        }

        public ActionResult captcha()
        {
            Bitmap bmp = new Bitmap(60,20);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.Blue);
            
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font font = new Font("Ariel", 8, FontStyle.Bold);
           
            string ranstr = "";
            int[] myIntArray = new int[5];
            Random r = new Random();

            for (int i = 0; i < myIntArray.Length; i++)
            {
                myIntArray[i] = r.Next(0,10);
                ranstr += myIntArray[i].ToString();
            }

            Session["ranstr"] = ranstr;
            cls_Users u = new cls_Users();
            u.captcha = ranstr;

            graphics.DrawString(ranstr, font, Brushes.White, 4, 4);

            Response.ContentType = "image/GIF";
            bmp.Save(Response.OutputStream,ImageFormat.Gif);
            font.Dispose();
            graphics.Dispose();
            bmp.Dispose();

            return View();
        }

        [HttpPost]
        public ActionResult UyeOl(tbl_Users usr)
        {
            string captcha = Request.Form["txt_captcha"];
            if (Session["ranstr"].ToString() == captcha)
            {
                bool sonuc = cls_Users.UyeEkle(usr);
                if (sonuc == true)
                {
                    Session["Mesaj"] = "Üyelik Başarıyla Gerçekleşti.";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Mesaj"] = "Üyelik Geçekleşemedi!";
                    return View();
                }
            }
            else
            {
                Session["Mesaj"] = "Güvenlik Rakamlarını Yanlış Girdiniz!";
                return View();
            }
        }

        public ActionResult Hakkimizda()
        {
            return View();
        }

        public ActionResult iletisim()
        {
            return View();
        }

        public ActionResult EnYeniler()
        {
            asm.enyeni_urunler = cp.urunler_getir("enyeni", "hayir", "", pcount);
            return View(asm);
        }

        public PartialViewResult partial_EnYeniler(string pagenumber)
        {
            asm.enyeni_urunler = cp.urunler_getir("enyeni", "evet", pagenumber, pcount);
            return PartialView(asm);
        }

        public ActionResult OzelUrunler()
        {
            asm.ozel_urunler = cp.urunler_getir("özel", "hayir", "", pcount);
            return View(asm);
        }

        public PartialViewResult partial_OzelUrunler(string pagenumber)
        {
            asm.ozel_urunler = cp.urunler_getir("özel", "evet", pagenumber, pcount);
            return PartialView(asm);
        }

        public ActionResult IndirimliUrunler()
        {
            asm.indirimli_urunler = cp.urunler_getir("indirim", "hayir", "", pcount);
            ViewBag.pcount = db.tbl_Products.Where(p => p.Discount != 0).OrderByDescending(p => p.Discount).Count() / pcount;
            return View(asm);
        }

        public PartialViewResult partial_IndirimliUrunler(string pagenumber)
        {
            asm.indirimli_urunler = cp.urunler_getir("indirim", "evet", pagenumber, pcount);
            ViewBag.pcount = asm.indirimli_urunler;
            return PartialView(asm);
        }

        public ActionResult OneCikanlar()
        {
            asm.onecikan_urunler = cp.urunler_getir("önecikan", "hayir", "", pcount);
            ViewBag.pcount = db.tbl_Products.OrderByDescending(p => p.OneCikanlar).Count() / pcount;
            return View(asm);
        }

        public PartialViewResult partial_OneCikanlar(string pagenumber)
        {
            asm.onecikan_urunler = cp.urunler_getir("önecikan", "evet", pagenumber, pcount);
            ViewBag.pcount = asm.onecikan_urunler;
            return PartialView(asm);
        }

        public ActionResult CokSatanlar(int? page)
        {
            var plist = db.vw_products.OrderByDescending(p => p.CokSatanlar).ToList();
            var pagenumber = page ?? 1; // page == null ise page == 1
            var pageddata = plist.ToPagedList(pagenumber, pcount);
            ViewBag.CokSatanlar = pageddata;
            return View();
        }

        [HttpPost]
        public ActionResult Mesaj(tbl_Messages msg)
        {
            bool sonuc = cls_Users.Mesaj(msg);
            if (sonuc)
            {
                Session["Mesaj"] = "Mesajınız bize ulaştı. En kısa sürede geri dönüş sağlayacağız.";
                return RedirectToAction("iletisim");
            }
            return RedirectToAction("iletisim");
        }

        public ActionResult Order()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Giris", "Home");
            }

            tbl_Users usr = cu.uyebilgilerigetir(Convert.ToInt32(Session["UserID"]));
            return View(usr);
        }

        [HttpPost]
        public ActionResult Order(FormCollection frm)
        {
            if (!string.IsNullOrWhiteSpace(frm["txt_tckimlikno"]))
            {
                string txt_tckimlikno = frm["txt_tckimlikno"];
            }
            if (!string.IsNullOrWhiteSpace(frm["txt_vergino"]))
            {
                string txt_vergino = frm["txt_vergino"];
            }

            string kkartno = Request.Form["kkartno"];
            string ay = Request.Form["ay"];
            string yil = Request.Form["yil"];
            string cvc = Request.Form["cvc"];

            // payu, iyzico
            return RedirectToAction("backref", "Home");

            // bağlantı olursa aşağıdaki kodlar
            //NameValueCollection data = new NameValueCollection();
            //string backref = "https://www.erensudeveci.com/backref";

            //data.Add("BACK_REF", backref);
            //data.Add("CC_NUMBER", kkartno);
            //data.Add("EXP_MONTH", ay);
            //data.Add("EXP_YEAR","20" + yil);
            //data.Add("CC_CVV", cvc);

            //var deger = "";
            //foreach (var item in data)
            //{
            //    var value = item as string;
            //    var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
            //    deger += byteCount + data.Get(value);
            //}

            //var signatureKey = "SECRET_KEY";
            //var hash = HashWithSignature(deger, signatureKey);
            //data.Add("ORDER_HASH", hash);
            //var x = POSTFormPayu("https://secure.payu.com.tr/order/alu/v3", data);
            //if (x.Contains("<STATUS>SUCCESS</STATUS>"))
            //{
            //    // başarılı
            //}
            //else
            //{
            //    // kredi kartı hataları
            //}

            //return View();
        }

        public static string POSTFormPayu(string url, NameValueCollection data)
        {
            var webClient = new WebClient();
            try
            {
                string request = System.Text.Encoding.UTF8.GetString(webClient.UploadValues(url, data)).Trim();
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string HashWithSignature(string hashString,string signature)
        {
            return "";
        }

        public ActionResult backref()
        {            
            HttpCookie cSetCookie;
            
            cSetCookie = Request.Cookies.Get("sepetim");

            cls_Orders co = new cls_Orders();
            co.sepet = cSetCookie.Value;
            orderGroupGUID = co.cookie_sepeti_siparis_tablosuna_yaz(Convert.ToInt32(Session["UserID"]));
            co.sepet = "";
            HttpCookie cCookie = new HttpCookie("sepetim", co.sepet);
            Response.Cookies.Add(cCookie);
            Response.Cookies["sepetim"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Onay");
        }
        
        public static string orderGroupGUID = "";

        public ActionResult Onay()
        {
            ViewBag.orderGroupGUID = orderGroupGUID;
            return View();
        }

        public ActionResult Siparislerim()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Giris", "Home");
            }

            cls_Orders o = new cls_Orders();
            List<vw_orders> olist = o.siparislerim(Convert.ToInt32(Session["UserID"]));
            return View(olist);
        }

        public ActionResult DetayliArama()
        {
            return View();
        }

        [HttpPost]
        public ActionResult dproducts(string[] SupplierID, string stoktavarmi)
        {
            int CategoryID = Convert.ToInt32(Request.Form["CategoryID"]);

            ArrayList mr = new ArrayList();

            if (SupplierID != null && SupplierID.Length > 0)
            {
                foreach (var item in SupplierID)
                {
                    mr.Add(item);
                }
            }

            string markaID = "";
            for (int i = 0; i < mr.Count; i++)
            {
                if (i != mr.Count - 1)
                {
                    markaID += "SupplierID = " + mr[i].ToString() + " OR ";
                }
                else
                {
                    markaID += "SupplierID = " + mr[i].ToString();
                }
            }

            string price = Request.Form["price"].Replace("$", "").Replace(" ", "");
            string[] fiyatdizi = price.Split('-');
            string min = fiyatdizi[0];
            string max = fiyatdizi[1];

            string all = "";
            if (SupplierID != null)
            {
                all = "SELECT * FROM tbl_Products WHERE (CategoryID = " + CategoryID + ") AND (Price >= " + min + " AND Price <= " + max + ") AND (" + markaID + ") ORDER BY ProductName";
            }
            else
            {
                all = "SELECT * FROM tbl_Products WHERE (CategoryID = " + CategoryID + ") AND (Price >= " + min + " AND Price <= " + max + ") ORDER BY ProductName";
            }

            // SELECT * FROM tbl_Products WHERE (CategoryID = 4) AND (Price >= 33 AND <= 440) AND (SupplierID = 6 OR SupplierID = 5) ORDER BY ProductName

            cls_Orders co = new cls_Orders();
            ViewBag.products = co.detaylisorguurunlerigetir(all);

            if ((ViewBag.products).Count == 0)
            {
                Session["Mesaj"] = "Aradığınız Kriterlerde Ürün Bulunamadı!";
                return RedirectToAction("DetayliArama", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult urunlerstring(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR", false));
            List<vw_arama> ulist = cls_Products.aramagetir(id);
            return Json(ulist, JsonRequestBehavior.AllowGet);
        }

    }
}