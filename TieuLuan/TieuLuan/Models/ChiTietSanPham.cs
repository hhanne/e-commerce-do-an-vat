﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TieuLuan.Models
{
    public class ChiTietSanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiaBan { get; set; }
        public int GiamGia { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa{get;set;}

        public List<ChiTietSanPham> sp;
        public void loadSPCT(string masp)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH,MOTA from SANPHAM ID_SP=@id ", conn);
            cmd.Parameters.AddWithValue("@id", masp);
            SqlDataReader dr = cmd.ExecuteReader();
            sp = new List<ChiTietSanPham>();
            while (dr.Read())
            {
                sp.Add(new ChiTietSanPham { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString(), MoTa = dr.GetValue(5).ToString() });
            }
            dr.Close();
        }
    }
}