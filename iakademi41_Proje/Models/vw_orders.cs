//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iakademi41_Proje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_orders
    {
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string orderGroupGUID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string NameSurname { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}