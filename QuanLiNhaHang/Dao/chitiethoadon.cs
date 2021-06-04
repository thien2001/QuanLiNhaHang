using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanliNhaHang.Models;
using MySql.Data.MySqlClient;
using QuanliNhaHang.Connect;
namespace QuanliNhaHang.Dao
{
    public class ChiTietHoaDonDAO
    {
        public static bool insertChiTietHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
            MySqlConnection connection = DBConnect.getConnection();
            connection.Open();
            String sql = "Insert into ChiTietHoaDon(MaHoaDon,MaSach,SoLuongBan,GiaBanCu,KhuyenMaiCu) values(?MaHoaDon,?MaSach,?SoLuongBan,?GiaBanCu,?KhuyenMaiCu)";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("?MaHoaDon", chiTietHoaDon.getMaHoaDon());
            cmd.Parameters.AddWithValue("?MaSach", chiTietHoaDon.getMaSach());
            cmd.Parameters.AddWithValue("?SoLuongBan", chiTietHoaDon.getSoLuongBan());
            cmd.Parameters.AddWithValue("?GiaBanCu", chiTietHoaDon.getGiaBanCu());
            cmd.Parameters.AddWithValue("?KhuyenMaiCu", chiTietHoaDon.getKhuyenMaiCu());
            cmd.ExecuteNonQuery();
            connection.Close();
            return true;
        }
    }
}