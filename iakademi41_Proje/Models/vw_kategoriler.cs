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
    
    public partial class vw_kategoriler
    {
        public int CategoryID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string CategoryName { get; set; }
        public Nullable<bool> active { get; set; }
    }
}
