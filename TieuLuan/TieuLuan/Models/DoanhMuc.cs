using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace TieuLuan.Models
{
    public class DoanhMuc
    {
        public string MaDM { get; set; }
        public string TenDM { get; set; }
        public List<DoanhMuc> doanhmuc;
        //private static string connectionString;

        public void loadDM()
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand("select ID_DM, TENDM  from DOANHMUC ", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            doanhmuc = new List<DoanhMuc>();
            while (dr.Read())
            {
                doanhmuc.Add(new DoanhMuc { MaDM = dr.GetValue(0).ToString(), TenDM = dr.GetValue(1).ToString() });
            }
            dr.Close();
        }
        //public void loadSPBC()
        //{
        //    string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
        //    SqlConnection conn = new SqlConnection(connStr);
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("select ID_SP, TITLE, GIA,GIAMGIA,HINHANH,MOTA  from SANPHAM where SPNOIBAC=1", conn);
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    spbc = new List<SanPhamBanChay>();
        //    while (dr.Read())
        //    {
        //        spbc.Add(new SanPhamBanChay { MaSP = dr.GetValue(0).ToString(), TenSP = dr.GetValue(1).ToString(), GiaBan = Convert.ToInt32(dr.GetValue(2)), GiamGia = Convert.ToInt32(dr.GetValue(3)), HinhAnh = dr.GetValue(4).ToString(), MoTa = dr.GetValue(5).ToString() });
        //    }
        //    dr.Close();
        //}
        // Add a new category
        public static List<DoanhMuc> LoadDM()
        {
            var danhMucs = new List<DoanhMuc>();
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ID_DM, TENDM FROM DOANHMUC", conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            danhMucs.Add(new DoanhMuc
                            {
                                MaDM = dr["ID_DM"].ToString(),
                                TenDM = dr["TENDM"].ToString()
                            });
                        }
                    }
                }
            }
            return danhMucs;
        }
        public  void AddDM(DoanhMuc DM)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                
                string sql = "INSERT INTO DOANHMUC (TENDM) VALUES (@TenDM)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDM", DM.TenDM ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Update an existing category
        public void UpdateDM(string tendm,string madm)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE DOANHMUC SET TENDM = @TenDM WHERE ID_DM = @maDM";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDM", tendm?? (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Cập nhật dữ liệu thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Không có dữ liệu nào được cập nhật.");
                    }
                }
            }
        }

        // Delete a category
        public void DeleteDM(string maDM)
        {
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM DOANHMUC WHERE ID_DM = @MaDM", conn))
                {
                    cmd.Parameters.AddWithValue("@MaDM", maDM);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}