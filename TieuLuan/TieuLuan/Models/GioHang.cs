using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TieuLuan.Models
{
    public class GioHang
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiaBan { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public int TongTien { get; set; }
        public List<GioHang> gh;
        public void loadSP(string id)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT GIOHANG.ID_SP, SANPHAM.TITLE,SANPHAM.GIAMGIA, SANPHAM.HINHANH, SOLUONG,SUM(GIOHANG.SOLUONG * SANPHAM.GIAMGIA) AS TONGTIEN FROM GIOHANG JOIN SANPHAM ON GIOHANG.ID_SP = SANPHAM.ID_SP JOIN USERS ON GIOHANG.ID_USERS = USERS.ID_USERS WHERE GIOHANG.ID_USERS = '" + id + "' GROUP BY GIOHANG.ID_SP, SANPHAM.TITLE, SANPHAM.HINHANH, SANPHAM.GIAMGIA, SOLUONG", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            gh = new List<GioHang>();
            while (dr.Read())
            {
                gh.Add(new GioHang { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)),  HinhAnh = dr.GetValue(3).ToString(), SoLuong = Convert.ToInt32(dr.GetValue(4)), TongTien = Convert.ToInt32(dr.GetValue(5)) });
            }
            dr.Close();
        }

    }
}