using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TieuLuan.Models
{
    public class SanPhamNoiBac
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiaBan { get; set; }
        public int GiamGia { get; set; }
        public string HinhAnh { get; set; }
        public List<SanPhamNoiBac> danhsach;
        public void loadSPNB()
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH from SANPHAM where SPNOIBAC=1", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            danhsach = new List<SanPhamNoiBac>();
            while (dr.Read())
            {
                danhsach.Add(new SanPhamNoiBac { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString() });
            }
            dr.Close();
        }
        public void loadSPNB2(string madm)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH from SANPHAM where ID_DM=@id", conn);
            cmd.Parameters.AddWithValue("@id", madm);
            SqlDataReader dr = cmd.ExecuteReader();
            danhsach = new List<SanPhamNoiBac>();
            while (dr.Read())
            {
                danhsach.Add(new SanPhamNoiBac { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString() });
            }
            dr.Close();
        }
        public void loadSPNB3(string maspbc)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH from SANPHAM where SPNOIBAC=@id", conn);
            cmd.Parameters.AddWithValue("@id", maspbc);
            SqlDataReader dr = cmd.ExecuteReader();
            danhsach = new List<SanPhamNoiBac>();
            while (dr.Read())
            {
                danhsach.Add(new SanPhamNoiBac { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString() });
            }
            dr.Close();
        }
        
    }

    }
