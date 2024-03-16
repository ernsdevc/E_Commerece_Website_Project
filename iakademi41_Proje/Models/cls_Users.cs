using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class cls_Users
    {
        public string captcha { get; set; }

        public static bool UyeEkle(tbl_Users usr)
        {
            using (iakademi41proje_DBEntities db = new iakademi41proje_DBEntities())
            {
                try
                {
                    usr.isAdmin = false;
                    usr.active = true;
                    db.tbl_Users.Add(usr);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public static bool Mesaj(tbl_Messages msg)
        {        
            iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
            try
            {
                db.tbl_Messages.Add(msg);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public tbl_Users uyebilgilerigetir(int id)
        {
            iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();
            tbl_Users usr = db.tbl_Users.FirstOrDefault(u => u.UserID == id);
            return usr;
        }
    }
}