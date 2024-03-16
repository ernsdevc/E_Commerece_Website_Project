using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi41_Proje.Models;

namespace iakademi41_Proje.Controllers
{
    public class BaseController : Controller
    {
        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
        // GET: Base
        public BaseController()
        {
            ViewBag.suplist = db.tbl_Suppliers.ToList();

            ViewBag.catlist = db.tbl_Categories.ToList();

            ViewBag.telefon = db.tbl_Settings.FirstOrDefault(t => t.ID == 1).telefon;

            ViewBag.sayfadakiurunsayisi = db.tbl_Settings.FirstOrDefault(t => t.ID == 1).sayfadakiurunsayisi;

            ViewBag.urunsayisi = db.tbl_Products.Count();
        }
    }
}