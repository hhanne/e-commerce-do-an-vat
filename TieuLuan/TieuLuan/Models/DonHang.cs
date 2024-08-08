using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TieuLuan.Models
{
    public class DonHang
    {
        public string MaKH { get; set; }
        public string MaDH { get; set; }
        public string TenKH { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string NgayDat { get; set; }
        public int TrangThai { get; set; }
        public int SoLuong { get; set; }
        public int TongTien { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int GiamGia { get; set; }
        public string HinhAnh { get; set; }

        public List<DonHang> sp;
        public void loadDH(string id)
        {

            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select D.ID_DH,FULLNAME,EMAIL,PHONE_NUMBER,DIACHI,ODER_DATE,TRANGTHAI,SOLUONGMUA,SOLUONGMUA*S.GIAMGIA,S.ID_SP,S.TITLE,S.GIAMGIA,S.HINHANH,ID_USERS  from CHITIECDONHANG C, DONHANG D, SANPHAM S WHERE C.ID_DH=D.ID_DH  AND C.ID_SP=S.ID_SP AND D.ID_USERS= " + id, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            sp = new List<DonHang>();
            while (dr.Read())
            {
                sp.Add(new DonHang { MaDH = dr.GetValue(0).ToString(), TenKH = dr.GetValue(1).ToString(), Email = dr.GetValue(2).ToString(), SDT = dr.GetValue(3).ToString(), DiaChi = dr.GetValue(4).ToString(), NgayDat = (dr.GetValue(5).ToString()), TrangThai = Convert.ToInt32(dr.GetValue(6)), SoLuong = Convert.ToInt32(dr.GetValue(7)), TongTien = Convert.ToInt32(dr.GetValue(8)), MaSP = (dr.GetValue(9).ToString()), TenSP = (dr.GetValue(10).ToString()), GiamGia = Convert.ToInt32(dr.GetValue(11)), HinhAnh = dr.GetValue(12).ToString(), MaKH = dr.GetValue(13).ToString() });
            }
            dr.Close();
        }
        public static List<DonHang> loadDH1()
        {
            var donhang = new List<DonHang>();
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select D.ID_DH,FULLNAME,EMAIL,PHONE_NUMBER,DIACHI,ODER_DATE,TRANGTHAI,SOLUONGMUA,SOLUONGMUA*S.GIAMGIA AS TONGTIEN,S.ID_SP,S.TITLE,S.GIAMGIA,S.HINHANH, ID_USERS  from CHITIECDONHANG C, DONHANG D, SANPHAM S WHERE C.ID_DH=D.ID_DH  AND C.ID_SP=S.ID_SP ", conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            donhang.Add(new DonHang
                            {
                                MaDH = dr["ID_DH"].ToString(),
                                TenKH = dr["FULLNAME"].ToString(),
                                Email = dr["EMAIL"].ToString(),
                                SDT = dr["PHONE_NUMBER"].ToString(),
                                DiaChi = dr["DIACHI"].ToString(),
                                NgayDat = dr["ODER_DATE"].ToString(),
                                TrangThai = Convert.ToInt32(dr["TRANGTHAI"]),
                                SoLuong = Convert.ToInt32(dr["SOLUONGMUA"]),
                                TongTien = Convert.ToInt32(dr["TONGTIEN"]),
                                MaSP = dr["ID_SP"].ToString(),
                                TenSP = dr["TITLE"].ToString(),
                                GiamGia = Convert.ToInt32(dr["GIAMGIA"]),
                                HinhAnh = dr["HINHANH"].ToString(),
                                MaKH = dr["ID_USERS"].ToString(),
                            });
                        }
                    }
                }
            }
            return donhang;
        }
        
        public void SuaDonHang(DonHang donhang)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE DONHANG SET TRANGTHAI = @TrangThai WHERE ID_DH = @MaDH";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDH", donhang.MaDH);
                    cmd.Parameters.AddWithValue("@TrangThai", donhang.TrangThai );
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void XoaDonHang(string id)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Xóa dữ liệu trong bảng CHITIECDONHANG
                        string sql1 = "DELETE FROM CHITIECDONHANG WHERE ID_DH = @MaDH";
                        using (SqlCommand cmd1 = new SqlCommand(sql1, conn, transaction))
                        {
                            cmd1.Parameters.AddWithValue("@MaDH", id);
                            cmd1.ExecuteNonQuery();
                        }

                        // Xóa dữ liệu trong bảng DONHANG
                        string sql2 = "DELETE FROM DONHANG WHERE ID_DH = @MaDH";
                        using (SqlCommand cmd2 = new SqlCommand(sql2, conn, transaction))
                        {
                            cmd2.Parameters.AddWithValue("@MaDH", id);
                            cmd2.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();
                        Console.WriteLine("Xóa đơn hàng thành công!");
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        Console.WriteLine("Xóa đơn hàng thất bại: " + ex.Message);
                    }
                }
            }
        }
    }
}