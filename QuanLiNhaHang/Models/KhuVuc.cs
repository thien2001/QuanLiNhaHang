using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLiNhaHang.Models
{
    public class KhuVuc
    {
        private int maKhuVuc;
        private String tenKhuVuc;
        private int phiGiaoHang;

        public KhuVuc()
        {
        }
        public int getMaKhuVuc()
        {
            return maKhuVuc;
        }

        public void setMaKhuVuc(int maKhuVuc)
        {
            this.maKhuVuc = maKhuVuc;
        }

        public String getTenKhuVuc()
        {

            return tenKhuVuc;
        }

        public void setTenKhuVuc(String tenKhuVuc)
        {
            this.tenKhuVuc = tenKhuVuc;
        }

        public int getPhiGiaoHang()
        {
            return phiGiaoHang;
        }

        public void setPhiGiaoHang(int phiGiaoHang)
        {
            this.phiGiaoHang = phiGiaoHang;
        }
    }
}