using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanliNhaHang.Models;
using MySql.Data.MySqlClient;
using QuanLiNhaHang.Connect;

namespace Tutorial.Dao
{
    public class KhuVucDAO
    {
        public static List<KhuVuc> getListKhuVuc()
        {
            MySqlConnection connection = DBConnect.getConnection();
            connection.Open();
            String sql = "Select * from KhuVuc order by TenKhuVuc asc";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader rs = cmd.ExecuteReader();
            List<KhuVuc> list = new List<KhuVuc>();
            while (rs.Read())
            {
                KhuVuc khuVuc = new KhuVuc();
                khuVuc.setMaKhuVuc(rs.GetInt32("MaKhuVuc"));
                khuVuc.setTenKhuVuc(rs.GetString("TenKhuVuc"));
                khuVuc.setPhiGiaoHang(rs.GetInt32("PhiGiaoHang"));
                list.Add(khuVuc);
            }
            rs.Close();
            connection.Close();
            return list;
        }
        public static int getPhiGiaoHang(String tenKhuVuc)
        {
            MySqlConnection connection = DBConnect.getConnection();
            connection.Open();
            String sql = "Select PhiGiaoHang from KhuVuc where TenKhuVuc='" + tenKhuVuc + "'";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader rs = cmd.ExecuteReader();
            int phiGiaoHang = 0;
            while (rs.Read())
            {
                phiGiaoHang = rs.GetInt32("PhiGiaoHang");
            }
            rs.Close();
            connection.Close();
            return phiGiaoHang;
        }
        public static int getMaKhuVuc(String tenKhuVuc)
        {
            MySqlConnection connection = DBConnect.getConnection();
            connection.Open();
            String sql = "Select MaKhuVuc from KhuVuc where TenKhuVuc='" + tenKhuVuc + "'";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader rs = cmd.ExecuteReader();
            int maKhuVuc = 0;
            while (rs.Read())
            {
                maKhuVuc = rs.GetInt32("MakhuVuc");
            }
            rs.Close();
            connection.Close();
            return maKhuVuc;
        }
    }
}