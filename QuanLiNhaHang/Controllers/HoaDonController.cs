using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tutorial.Models;
using Tutorial.Dao;
using System.Globalization;
using System.Text;
using QuanLiNhaHang.tools;
namespace QuanLiNhaHang.Controllers
{
    public class HoaDonController : Controller
    {
        //
        // GET: /HoaDon/
        public ActionResult hoadon()
        {
            return View();
        }
        [HttpPost]
        public ActionResult hoadon(FormCollection f)
        {
            String error = "";
            if (string.IsNullOrEmpty(f["user2"]))
            {
                error += "Tên người nhận không được để trống";
                ViewBag.message1 = "Tên người nhận không được để trống";
            }
            if (string.IsNullOrEmpty(f["soDT"]))
            {
                error += "Số điện thoại không được để trống";
                ViewBag.message2 = "Số điện thoại không được để trống";
            }
            if (string.IsNullOrEmpty(f["diaChi"]))
            {
                error += "Dia chi không được để trống";
                ViewBag.messag31 = "Địa chỉ không được để trống";
            }
            if (string.IsNullOrEmpty(error))
            {
                //Lưu hóa đơn và chi tiết hóa đơn
                //Câp nhật lại số xu tích lũy , tiền nợ khách hàng
                KhachHang kh1 = (KhachHang)Session["user"];
                int soXuKhachHang = kh1.getSoXuTichLuy();
                int soXuSuDung = 0;
                if (f["xu"] != null)
                {
                    soXuSuDung = Convert.ToInt32(f["xu"]);
                }
                Cart cart = (Cart)Session["cart"];
                int phiGiaoHang = 0;
                if (cart.total() < 250000)
                {
                    phiGiaoHang = KhuVucDAO.getPhiGiaoHang(f["khuvuc"].ToString());
                }
                int tongTien = 0;
                if (cart.total() + phiGiaoHang > soXuSuDung)
                {
                    tongTien = (int)cart.total() + phiGiaoHang - soXuSuDung;
                }
                else
                {
                    tongTien = 0;
                    soXuSuDung = (int)cart.total() + phiGiaoHang;
                }
                soXuKhachHang = soXuKhachHang - soXuSuDung;
                KhachHangDAO.capNhatKhachHang(kh1.getMaKhachHang(), 0, soXuKhachHang);
                //Lưu lại hóa đơn
                HoaDon hd = new HoaDon();
                hd.setMaHoaDon(HoaDonDAO.getSoLuong() + 1);
                DateTime dt = DateTime.Now;
                String date1 = dt.ToString("yyyy/MM/dd");
                hd.setNgayLapHoaDon(DateTime.ParseExact(date1, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None));
                hd.setMaKhachHang(kh1.getMaKhachHang());
                hd.setDiaChiGiaoHang(f["diaChi"]);
                hd.setMaKhuVucGiaoHang(KhuVucDAO.getMaKhuVuc(f["khuvuc"]));
                hd.setSoDienThoai(f["soDT"]);
                hd.setTenNguoiNhan(f["user2"]);
                hd.setSoXuSuDung(Convert.ToInt32(f["xu"]));
                hd.setTongTienHoaDon(tongTien);
                hd.setPhiGiaoHang(phiGiaoHang);
                HoaDonDAO.insertHoaDon(hd);
                //Lưu lại chi tiết hóa đơn
                String noiDung = "";
                foreach (Item item in cart.getCartItems().Values)
                {
                    //cập số lượng của từng đầu sách
                    int soLuongBan = item.getSoLuongBan();
                    int soLuongTon = item.getSach().getSoLuongTon() - soLuongBan;
                    SachDAO.capNhatSoLuong(item.getSach().getMaSach(), soLuongTon);
                    //Lưu vào bảng chi tiết hóa đơn
                    ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                    chiTietHoaDon.setMaHoaDon(hd.getMaHoaDon());
                    chiTietHoaDon.setMaSach(item.getSach().getMaSach());
                    chiTietHoaDon.setGiaBanCu(item.getSach().getGiaBan());
                    chiTietHoaDon.setKhuyenMaiCu(item.getSach().getKhuyenMai());
                    chiTietHoaDon.setSoLuongBan(soLuongBan);
                    noiDung += soLuongBan + "   cuốn  " + SachDAO.getSach(item.getSach().getMaSach()).getTenSach() + " , ";
                    ChiTietHoaDonDAO.insertChiTietHoaDon(chiTietHoaDon);
                }
                cart = new Cart();
                Session["cart"] = cart;
                //Gửi email cho khách hàng
                SendMail sm = new SendMail();
                StringBuilder builder = new StringBuilder();
                builder.Append("Kính chào quý khách " + kh1.getHoTenKhachHang());
                builder.Append("\nNhà Hàng vừa nhận được đơn hàng " + hd.getMaHoaDon() + " của quý khách đặt ngày " + hd.getNgayLapHoaDon().ToString("dd-MM-yyyy"));
                builder.Append(". Chúng tôi thông báo đến khách hàng thông tin chi tiết đơn hàng đã đặt.");
                builder.Append("\n\n\n\nSau đây là thông tin chi tiết đơn hàng:");
                builder.Append("\nNgười nhận đơn hàng: " + hd.getTenNguoiNhan());
                builder.Append("\nĐịa chỉ giao hàng: " + hd.getDiaChiGiaoHang());
                builder.Append("\nSố điện thoại người nhận: " + hd.getSoDienThoai());
                builder.Append("\nCác loại sách đã mua: " + noiDung);
                builder.Append("\nTổng tiền cần thanh toán: " + tongTien + " VNĐ");
                sm.sendMail(kh1.geteMail(), "Nhà Hàng", builder.ToString());
                return Redirect("~/Home/index");
            }
            else
            {
                ViewBag.error = "Điền đầy thông tin";
            }
            return View();
        }
        // GET: /XacNhanMuaHang/
        public ActionResult xacnhanmuahang(String dangNhap = "")
        {
            String url = "~/HoaDon/hoadon";
            if (Session["user"] == null)
            {
                url = "~/KhachHang/thongtinkhachhang";
            }
            return Redirect(url);
        }
    }
}