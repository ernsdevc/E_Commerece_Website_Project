using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iakademi41_Proje.Models;

namespace iakademi41_Proje.MVVM
{
    public class AnaSayfaModel
    {
        public List<vw_products> slider_urunler { get; set; }
        public tbl_Products gunun_urunu { get; set; }
        public List<vw_products> enyeni_urunler { get; set; }
        public List<vw_products> ozel_urunler { get; set; }
        public List<vw_products> indirimli_urunler { get; set; }
        public List<vw_products> onecikan_urunler { get; set; }
        public List<vw_products> coksatan_urunler { get; set; }
        public List<vw_products> firsat_urunler { get; set; }
        public List<vw_products> bunabakanlar_urunler { get; set; }
        public vw_products urundetay { get; set; }
        public List<vw_products> benzerurunler { get; set; }
        public List<vw_products> urunler { get; set; }
        public List<vw_kategoriler> kategoriler { get; set; }
        public List<vw_markalar> markalar { get; set; }
    }
}