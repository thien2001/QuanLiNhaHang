//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLiNhaHang.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Chitietgiohang
    {
        public int Mã_CTGH { get; set; }
        public string Mã_GH { get; set; }
        public string Mã_SP { get; set; }
        public int Số_lượng { get; set; }
    
        public virtual Giohang Giohang { get; set; }
        public virtual Sanpham Sanpham { get; set; }
    }
}
