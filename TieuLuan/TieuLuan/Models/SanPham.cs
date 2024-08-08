using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TieuLuan.Models
{
    public class SanPham
    {
        public string MaDM { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiaBan { get; set; }
        public int GiamGia { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public int SPNB { get; set; }
        public List<SanPham> sp;
        public void loadSP()
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_DM,ID_SP, TITLE, GIA,GIAMGIA,MOTA,HINHANH,SPNOIBAC from SANPHAM ", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            sp = new List<SanPham>();
            while (dr.Read())
            {
                sp.Add(new SanPham { MaDM = dr.GetValue(0).ToString(), MaSP = dr.GetValue(1).ToString(), TenSP = dr.GetValue(2).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(3)), GiamGia = Convert.ToInt32(dr.GetValue(4)), MoTa = dr.GetValue(5).ToString(), HinhAnh = dr.GetValue(6).ToString(), SPNB = Convert.ToInt32(dr.GetValue(7)) });
            }
            dr.Close();
        }
        public void loadSP2(string madm)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH from SANPHAM where ID_DM=@id", conn);
            cmd.Parameters.AddWithValue("@id", madm);
            SqlDataReader dr = cmd.ExecuteReader();
            sp = new List<SanPham>();
            while (dr.Read())
            {
                sp.Add(new SanPham { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString() });
            }
            dr.Close();
        }
        public void loadSP3(string maspnb)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH from SANPHAM where SPNOIBAC=@id", conn);
            cmd.Parameters.AddWithValue("@id", maspnb);
            SqlDataReader dr = cmd.ExecuteReader();
            sp = new List<SanPham>();
            while (dr.Read())
            {
                sp.Add(new SanPham { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString() });
            }
            dr.Close();
        }

        public static List<SanPham> LoadSP()
        {
            var sanPhams = new List<SanPham>();
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ID_SP, TITLE, GIA, GIAMGIA, HINHANH FROM SANPHAM", conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            sanPhams.Add(new SanPham
                            {
                                MaSP = dr["ID_SP"].ToString(),
                                TenSP = dr["TITLE"].ToString(),
                                GiaBan = Convert.ToInt32(dr["GIA"]),
                                GiamGia = Convert.ToInt32(dr["GIAMGIA"]),
                                HinhAnh = dr["HINHANH"].ToString(),
                            });
                        }
                    }
                }
            }
            return sanPhams;
        }

        //public void ThemSanPham(SanPham sanPham)
        //{
        //    string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        string sql = "INSERT INTO SANPHAM (ID_SP, TITLE, GIA, GIAMGIA, HINHANH) VALUES (@MaSP, @TenSP, @GiaBan, @GiamGia, @HinhAnh)";
        //        using (SqlCommand cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@MaSP", sanPham.MaSP);
        //            cmd.Parameters.AddWithValue("@TenSP", sanPham.TenSP);
        //            cmd.Parameters.AddWithValue("@GiaBan", sanPham.GiaBan);
        //            cmd.Parameters.AddWithValue("@GiamGia", sanPham.GiamGia);
        //            cmd.Parameters.AddWithValue("@HinhAnh", sanPham.HinhAnh);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
        public void ThemSanPham(SanPham sanPham)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "INSERT INTO SANPHAM (TITLE, GIA, GIAMGIA,ID_DM,MOTA,SPNOIBAC, HINHANH) VALUES (@TenSP, @GiaBan, @GiamGia,@MaDM,@MoTa,@SPNB, @HinhAnh)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenSP", sanPham.TenSP ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GiaBan", sanPham.GiaBan);
                    cmd.Parameters.AddWithValue("@GiamGia", sanPham.GiamGia);
                    cmd.Parameters.AddWithValue("@MaDM", sanPham.MaDM);
                    cmd.Parameters.AddWithValue("@MoTa", sanPham.MoTa);
                    cmd.Parameters.AddWithValue("@SPNB", sanPham.SPNB);
                    cmd.Parameters.AddWithValue("@HinhAnh", sanPham.HinhAnh ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void SuaSanPham(SanPham sanPham)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE SANPHAM SET TITLE = @TenSP, GIA = @GiaBan, GIAMGIA = @GiamGia,ID_DM=@MaDM,MOTA=@MoTa,SPNOIBAC=@SPNB, HINHANH = @HinhAnh WHERE ID_SP = @MaSP";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSP", sanPham.MaSP);
                    cmd.Parameters.AddWithValue("@TenSP", sanPham.TenSP ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GiaBan", sanPham.GiaBan);
                    cmd.Parameters.AddWithValue("@GiamGia", sanPham.GiamGia);
                    cmd.Parameters.AddWithValue("@MaDM", sanPham.MaDM);
                    cmd.Parameters.AddWithValue("@MoTa", sanPham.MoTa);
                    cmd.Parameters.AddWithValue("@SPNB", sanPham.SPNB);
                    cmd.Parameters.AddWithValue("@HinhAnh", sanPham.HinhAnh ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void XoaSanPham(string id)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM SANPHAM WHERE ID_SP = @MaSP";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSP", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
    
