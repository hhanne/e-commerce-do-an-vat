using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TieuLuan.Models
{
    
    public class KhachHang
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public static List<KhachHang> LoadKh()
        {
            var khachhang = new List<KhachHang>();
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ID_USERS,FULNAME, EMAIL, PHONE_NUMBER FROM USERS WHERE ID_ROLE=2", conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            khachhang.Add(new KhachHang
                            {
                                MaKH = dr["ID_USERS"].ToString(),
                                TenKH = dr["FULNAME"].ToString(),
                                Email = dr["EMAIL"].ToString(),
                                SDT = dr["PHONE_NUMBER"].ToString(),
                               
                            });
                        }
                    }
                }
            }
            return khachhang;
        }
        
    }
}