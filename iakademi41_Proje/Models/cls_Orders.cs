using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iakademi41_Proje.Models
{
    public class cls_Orders
    {
        public int ProductID { get; set; }

        public int Count { get; set; }

        public string sepet { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public int KDV { get; set; }

        public string PhotoPath { get; set; }

        iakademi41proje_DBEntities db = new iakademi41proje_DBEntities();

        public bool add_tocart(int id)
        {
            bool varmi = true;

            string[] sepetdizi = sepet.Split('&');
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                if (sepetdizi2[0] == id.ToString())
                {
                    // bu ürün sepette var
                    varmi = false;
                    return varmi;
                }
            }

            return varmi;
        }

        // sepet üzerinde bilgi yazdırma
        public List<cls_Orders> sepetiyazdir()
        {
            List<cls_Orders> sip = new List<cls_Orders>();
            string[] sepetdizi = sepet.Split('&');
            if (sepetdizi[0] != "")
            {
                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    string[] sepetdizi2 = sepetdizi[i].Split('=');
                    int pid = Convert.ToInt32(sepetdizi2[0]);

                    if (sepetdizi2[0] != "")
                    {
                        tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.ProductID == pid);
                        cls_Orders o = new cls_Orders();
                        o.ProductName = prd.ProductName;
                        o.Price = Convert.ToDecimal(prd.Price);
                        o.KDV = Convert.ToInt32(prd.KDV);
                        o.Count = Convert.ToInt32(sepetdizi2[1]);
                        o.ProductID = Convert.ToInt32(prd.ProductID);
                        o.PhotoPath = prd.PhotoPath;
                        sip.Add(o);
                    }
                }
            }
            return sip;
        }

        // cart sayfasında ürün sil
        public void sepetten_sil(int id)
        {
            string[] sepetdizi = sepet.Split('&');
            string yenisepet = "";
            int count = 1;

            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');

                if (count == 1)
                {
                    // yok
                    if (sepetdizi2[0] != id.ToString())
                    {
                        yenisepet += sepetdizi2[0] + "=" + sepetdizi2[1];
                        count++;
                    }
                }
                else
                {
                    if (sepetdizi2[0] != id.ToString())
                    {
                        yenisepet += "&" + sepetdizi2[0] + "=" + sepetdizi2[1];
                        count++;
                    }
                }
            }
            sepet = yenisepet;
        }

        public string cookie_sepeti_siparis_tablosuna_yaz(int id)
        {
            string orderGroupGUID = DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("-", "").Replace(".", "").Replace("/", "").Replace("PM", "");
            List<cls_Orders> sip = sepetiyazdir();
            foreach (var item in sip)
            {
                tbl_Orders ord = new tbl_Orders();
                ord.ProductID = item.ProductID;
                ord.UserID = id;
                ord.orderGroupGUID = orderGroupGUID;
                ord.OrderDate = DateTime.Now;
                ord.Quantity = item.Count;
                db.tbl_Orders.Add(ord);
                db.SaveChanges();
            }
            return orderGroupGUID;
        }

        public List<vw_orders> siparislerim(int UserID)
        {
            List<vw_orders> olist = db.vw_orders.OrderByDescending(o => o.OrderDate).Where(o => o.UserID == UserID).ToList();
            return olist;
        }

        public List<cls_Orders> detaylisorguurunlerigetir(string hepsi)
        {
            List<cls_Orders> prd = new List<cls_Orders>();
            SqlConnection sqlcon = Connection.baglanti;
            SqlCommand sqlcmd = new SqlCommand(hepsi,sqlcon);
            sqlcon.Open();
            SqlDataReader sdr = sqlcmd.ExecuteReader();
            while (sdr.Read())
            {
                cls_Orders p = new cls_Orders();
                p.ProductID = Convert.ToInt32(sdr["ProductID"]);
                p.ProductName = sdr["ProductName"].ToString();
                p.Price = Convert.ToDecimal(sdr["Price"]);
                p.PhotoPath = sdr["PhotoPath"].ToString();
                prd.Add(p);
            }
            sqlcon.Close();
            return prd;
        }
    }
}