using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLiNhaHang.Models;

namespace QuanLiNhaHang.Controllers
{
    public class ChiTietGioHangController : Controller
    {
        CT25Team111Entities2 db = new CT25Team111Entities2();
       
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
              
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
       
        public ActionResult ThemGioHang(int iMasp, string strURL)
        {
            Sanpham sp = db.Sanphams.SingleOrDefault(n => n.Masp == iMasp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
           
            List<GioHang> lstGioHang = LayGioHang();
            
            GioHang sanpham = lstGioHang.Find(n => n.iMasp == iMasp);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMasp);
                
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        
        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            
            Sanpham sp = db.Sanphams.SingleOrDefault(n => n.Masp == iMaSP);
            
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            List<GioHang> lstGioHang = LayGioHang();
           
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMasp == iMaSP);
            
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
       
        public ActionResult XoaGioHang(int iMaSP)
        {
           
            Sanpham sp = db.Sanphams.SingleOrDefault(n => n.Masp == iMaSP);
            
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMasp == iMaSP);
           
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMasp == iMaSP);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
       
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
       
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
       
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
       
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);

        }

        #region Đặt hàng
        
        [HttpPost]
        public ActionResult DatHang()
        {
           
            if (Session["use"] == null || Session["use"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "User");
            }
           
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }

            Donhang ddh = new Donhang();
            Nguoidung kh = (Nguoidung)Session["use"];
            List<GioHang> gh = LayGioHang();
            ddh.MaNguoidung = kh.MaNguoiDung;
            ddh.Ngaydat = DateTime.Now;
            db.Donhangs.Add(ddh);
            db.SaveChanges();
            
            foreach (var item in gh)
            {
                Chitietdonhang ctDH = new Chitietdonhang();
                ctDH.Madon = ddh.Madon;
                ctDH.Masp = item.iMasp;
                ctDH.Soluong = item.iSoLuong;
                ctDH.Dongia = (decimal)item.dDonGia;
                db.Chitietdonhangs.Add(ctDH);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}