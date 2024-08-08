using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TieuLuan.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace TieuLuan.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult baocao()
        {
            string filename = "~/assets/Nhom12.pdf";
            return File(filename, "application/pdf");
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            SanPhamNoiBac data = new SanPhamNoiBac();
            data.loadSPNB();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View(data);
        }
        public ActionResult about_us()
        {
            return View();
        }
        //public ActionResult login()
        //{
        //    SqlConnection conn;
        //    var e = Request.Form["email"];
        //    var p = Request.Form["pswd"];

        //    if (e == null)
        //        return View();
        //    string p2 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(p))).Replace("-", "");
        //    string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
        //    conn = new SqlConnection(connStr);
        //    conn.Open();
        //    if (conn == null)
        //        return Content("No connect");
        //    string sql = "select*from USERS where EMAIL ='" + e + "' and MATKHAU ='" + p2 + "'";
        //    //return Content(sql);
        //    SqlCommand comm = new SqlCommand(sql, conn);
        //    SqlDataReader data = comm.ExecuteReader();
        //    if (data.HasRows)
        //    {
        //        data.Read();
        //        Session["user_id"] = data.GetValue(0).ToString();
        //        Session["login"] = data.GetValue(1).ToString();
        //        string sql1 = "select count(*) from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
        //        data.Close();
        //        SqlCommand comm1 = new SqlCommand(sql1, conn);
        //        SqlDataReader data1 = comm1.ExecuteReader();
        //        if (data1.HasRows)
        //        {
        //            data1.Read();
        //            Session["cart"] = data1.GetValue(0).ToString();
        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Thong tin sai";

        //    }
        //    return View();

        //}

        public ActionResult login()
        {
            SqlConnection conn;
            var e = Request.Form["email"];
            var p = Request.Form["pswd"];

            if (e == null)
                return View();

            string p2 = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(p))).Replace("-", "");
            string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
            conn = new SqlConnection(connStr);
            conn.Open();
            if (conn == null)
                return Content("No connect");

            string sql = "SELECT ID_USERS, EMAIL,FULNAME, ID_ROLE FROM USERS WHERE EMAIL ='" + e + "' AND MATKHAU ='" + p2 + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataReader data = comm.ExecuteReader();

            if (data.HasRows)
            {
                data.Read();
                Session["user_id"] = data.GetValue(0).ToString();
                Session["login"] = data.GetValue(2).ToString();
                int roleId = (int)data.GetValue(3);

                data.Close();

                string sql1 = "SELECT COUNT(*) FROM GIOHANG WHERE id_users = '" + Session["user_id"].ToString() + "'";
                SqlCommand comm1 = new SqlCommand(sql1, conn);
                SqlDataReader data1 = comm1.ExecuteReader();

                if (data1.HasRows)
                {
                    data1.Read();
                    Session["cart"] = data1.GetValue(0).ToString();
                }
                data1.Close();
                string sql2 = "select count(*) from CHITIECDONHANG C, DONHANG D, SANPHAM S WHERE C.ID_DH=D.ID_DH AND D.TRANGTHAI != 2 AND C.ID_SP=S.ID_SP AND D.ID_USERS=" + Session["user_id"].ToString();
                comm1 = new SqlCommand(sql2, conn);
                data1 = comm1.ExecuteReader();

                if (data1.HasRows)
                {
                    data1.Read();
                    Session["order"] = data1.GetValue(0).ToString();
                }
                data1.Close();
                if (roleId == 1) // Assuming role ID 1 is for admin
                {
                    return RedirectToAction("DSDanhMuc", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Thông tin sai";
            }
            return View();
        }

        public ActionResult logout()
        {
            Session["login"] = null;
            Session["user_id"] = null;
            Session["cart"] = null;
            Session["order"] = null;
            return RedirectToAction("login");
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessRegister()
        {
            if (Request.HttpMethod == "POST")
            {
                string fullname = Request.Form["fullname"];
                string email = Request.Form["email"];
                string password = Request.Form["pws"];
                string phone = Request.Form["phone"];
                string hashedPassword = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "");

                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "INSERT INTO USERS (FULNAME, EMAIL, MATKHAU,ID_ROLE,PHONE_NUMBER) VALUES (@f, @e, @p2,2,@ph)";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@f", fullname);
                        comm.Parameters.AddWithValue("@e", email);
                        comm.Parameters.AddWithValue("@p2", hashedPassword);
                        comm.Parameters.AddWithValue("@ph", phone);
                        int rowsAffected = comm.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Session["register"] = email;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "Registration failed";
                            return View();
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult reset_password()
        {
            return View();
        }
        public ActionResult products()
        {
            SanPham data = new SanPham();
            data.loadSP();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View(data);
        }

        public ActionResult product_detail(string idsp)
        {
            ChiTietSanPham data = new ChiTietSanPham();
            data.loadSPCT(idsp);
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            SanPhamBanChay data4 = new SanPhamBanChay();
            data4.loadSPLQ(idsp);
            return View(data);
        }
        public ActionResult Doanhmuc(string id)
        {
            SanPhamNoiBac data = new SanPhamNoiBac();
            data.loadSPNB2(id);
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View(data);
            //return Content(id);            
        }
        public ActionResult news()
        {

            SanPham data = new SanPham();
            data.loadSP();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View(data);
        }
        public ActionResult SanPhamBanChay(string id)
        {
            //SanPhamBanChay data1 = new SanPhamBanChay();
            //data1.loadSPCT(id);
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            SanPhamBanChay data4 = new SanPhamBanChay();
            data4.loadSPLQ(id);
            return View(data4);
        }
        //News
        public ActionResult chitiettintuc()
        {
            SanPham data = new SanPham();
            data.loadSP();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View();
        }
        public ActionResult Cac_mon_an_vat_vao_mua_dong_gia_chi_tu_10k()
        {
            SanPham data = new SanPham();
            data.loadSP();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View();
        }
        public ActionResult Nhung_mon_an_vat_tai_Ha_Noi_vao_mua_dong()
        {
            SanPham data = new SanPham();
            data.loadSP();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View();
        }
        public ActionResult Top_21_mon_an_Ha_Noi_no_quen_loi_ve()
        {
            SanPham data = new SanPham();
            data.loadSP();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            return View();
        }
        public ActionResult search(string searchString = "")
        {
            SanPhamNoiBac data = new SanPhamNoiBac();
            data.loadSPNB();
            DoanhMuc data2 = new DoanhMuc();
            data2.loadDM();
            ViewData["dm"] = data2;
            SanPhamBanChay data3 = new SanPhamBanChay();
            data3.loadSPBC();
            ViewData["spbc"] = data3;
            if (!string.IsNullOrEmpty(searchString))
            {

                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com; Database=nhom12_qlydoanvat; User ID=nnhom12_SQLLogin_1; password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT ID_SP, TITLE, GIA, GIAMGIA, HINHANH FROM SANPHAM WHERE TITLE LIKE @searchString", conn))
                    {
                        cmd.Parameters.AddWithValue("@searchString", "%" + searchString + "%");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            var sp = new List<SanPham>();
                            while (dr.Read())
                            {
                                sp.Add(new SanPham
                                {
                                    MaSP = dr["ID_SP"].ToString(),
                                    TenSP = dr["TITLE"].ToString(),
                                    GiaBan = Convert.ToInt32(dr["GIA"]),
                                    GiamGia = Convert.ToInt32(dr["GIAMGIA"]),
                                    HinhAnh = dr["HINHANH"].ToString()
                                });
                            }
                            return View(sp);
                        }
                    }
                }
            }

            return ViewBag("Khong tim thay");

        }
        public ActionResult GioHang(string id)
        {
            if (Session["user_id"] != null)
            {
                GioHang gioHang = new GioHang();
                gioHang.loadSP(Session["user_id"].ToString());
                return View(gioHang);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ThemVaoGioHang(string id)
        {
            if (Session["user_id"] != null)
            {
                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "select * from GIOHANG where id_users='" + Session["user_id"].ToString() + "' and id_sp = '" + id + "'";
                    //return Content(sql);
                    SqlCommand comm = new SqlCommand(sql, conn);
                    SqlDataReader data = comm.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Close();
                        // if the product is already in cart
                        string updatesql = "update GIOHANG set SOLUONG = SOLUONG + 1 where ID_USERS = @user_id and ID_SP='" + id + "'";
                        using (SqlCommand nestedcomm = new SqlCommand(updatesql, conn))
                        {
                            nestedcomm.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                            int rowsAffected = nestedcomm.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                string sql1 = "select count(*) from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
                                data.Close();
                                SqlCommand comm1 = new SqlCommand(sql1, conn);
                                SqlDataReader data1 = comm1.ExecuteReader();
                                if (data1.HasRows)
                                {
                                    data1.Read();
                                    Session["cart"] = data1.GetValue(0).ToString();
                                }
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                            else
                            {
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                        }
                    }
                    else
                    {
                        data.Close();
                        // if the product is not in cart
                        string insertsql = "INSERT INTO GIOHANG (ID_USERS, ID_SP, SOLUONG) VALUES (@user_id, @prod_id, 1)";
                        using (SqlCommand nestedcomm = new SqlCommand(insertsql, conn))
                        {
                            nestedcomm.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                            nestedcomm.Parameters.AddWithValue("@prod_id", id);
                            int rowsAffected = nestedcomm.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                string sql1 = "select count(*) from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
                                data.Close();
                                SqlCommand comm1 = new SqlCommand(sql1, conn);
                                SqlDataReader data1 = comm1.ExecuteReader();
                                if (data1.HasRows)
                                {
                                    data1.Read();
                                    Session["cart"] = data1.GetValue(0).ToString();
                                }
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                            else
                            {
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                        }
                    }
                }
            }
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        [HttpPost]
        public ActionResult ThemVaoGioHangCT()
        {
            if (Session["user_id"] != null)
            {
                string id = Request.Form["id_sp"];
                string soLuong = Request.Form["quantity"];
                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "select * from GIOHANG where id_users='" + Session["user_id"].ToString() + "' and id_sp = '" + id + "'";
                    //return Content(sql);
                    SqlCommand comm = new SqlCommand(sql, conn);
                    SqlDataReader data = comm.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Close();
                        // if the product is already in cart
                        string updatesql = "update GIOHANG set SOLUONG = SOLUONG + " + soLuong + " where ID_USERS = @user_id and ID_SP='" + id + "'";
                        using (SqlCommand nestedcomm = new SqlCommand(updatesql, conn))
                        {
                            nestedcomm.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                            int rowsAffected = nestedcomm.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                string sql1 = "select count(*) from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
                                data.Close();
                                SqlCommand comm1 = new SqlCommand(sql1, conn);
                                SqlDataReader data1 = comm1.ExecuteReader();
                                if (data1.HasRows)
                                {
                                    data1.Read();
                                    Session["cart"] = data1.GetValue(0).ToString();
                                }
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                            else
                            {
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                        }
                    }
                    else
                    {
                        data.Close();
                        // if the product is not in cart
                        string insertsql = "INSERT INTO GIOHANG (ID_USERS, ID_SP, SOLUONG) VALUES (@user_id, @prod_id, " + soLuong + ")";
                        using (SqlCommand nestedcomm = new SqlCommand(insertsql, conn))
                        {
                            nestedcomm.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                            nestedcomm.Parameters.AddWithValue("@prod_id", id);
                            int rowsAffected = nestedcomm.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                string sql1 = "select count(*) from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
                                data.Close();
                                SqlCommand comm1 = new SqlCommand(sql1, conn);
                                SqlDataReader data1 = comm1.ExecuteReader();
                                if (data1.HasRows)
                                {
                                    data1.Read();
                                    Session["cart"] = data1.GetValue(0).ToString();
                                }
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                            else
                            {
                                return Redirect(HttpContext.Request.Headers["Referer"]);
                            }
                        }
                    }
                }
            }
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        public ActionResult XoaGioHang()
        {
            if (Session["user_id"] != null)
            {
                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "delete from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.ExecuteNonQuery();
                    }
                }
            }
            Session["cart"] = null;
            return RedirectToAction("GioHang", "Home");
        }

        public ActionResult ThanhToan()
        {
            if (Session["user_id"] != null)
            {
                GioHang gioHang = new GioHang();
                gioHang.loadSP(Session["user_id"].ToString());
                return View(gioHang);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult XoaSP(string id)
        {
            if (Session["user_id"] != null)
            {
                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "delete from GIOHANG where id_users = '" + Session["user_id"].ToString() + "' and id_sp='" + id + "'";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.ExecuteNonQuery();
                    }
                    string sql1 = "select count(*) from GIOHANG where id_users = '" + Session["user_id"].ToString() + "'";
                    SqlCommand comm1 = new SqlCommand(sql1, conn);
                    SqlDataReader data1 = comm1.ExecuteReader();
                    if (data1.HasRows)
                    {
                        data1.Read();
                        Session["cart"] = data1.GetValue(0).ToString();
                    }
                }
                return RedirectToAction("GioHang", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DonHang()
        {
            DonHang data = new DonHang();

            if (Session["user_id"] != null && !string.IsNullOrEmpty(Session["user_id"].ToString()))
            {
                data.loadDH(Session["user_id"].ToString());
                return View(data);
            }
            else
            {
                ViewBag.Message = "Bạn chưa đăng nhập tài khoản!";
                return View();
            }
        }

        [HttpPost]
        public ActionResult DatHang()
        {
            if (Session["user_id"] != null)
            {
                String fullname = Request.Form["fullname"].ToString();
                String email = Request.Form["email"].ToString();
                String diachi = Request.Form["diachi"].ToString();
                String phone_number = Request.Form["phone_number"].ToString();
                String note = Request.Form["note"].ToString();

                DateTime dt = DateTime.Now;
                string year = dt.Year.ToString();
                string month = dt.Month.ToString();
                string day = dt.Day.ToString();
                string fdate = year + "-" + month + "-" + day;

                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "insert into DonHang (fullname, email, phone_number, diachi, note, oder_date, trangthai, id_users) values (N'" + fullname +"', N'" + email + "', N'" + phone_number + "', N'" + diachi + "', N'" + note + "','" + fdate + "', " + "1" + "," + Session["user_id"].ToString() + ")";
                    SqlCommand comm = new SqlCommand(sql, conn);
                    int rowsAffected = comm.ExecuteNonQuery();
                    string sql1 = "select * from donhang where id_users = " + Session["user_id"].ToString() + " order by id_dh desc";
                    SqlCommand comm1 = new SqlCommand(sql1, conn);
                    SqlDataReader data = comm1.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        string iddh = data.GetValue(0).ToString();
                        GioHang gh = new GioHang();
                        gh.loadSP(Session["user_id"].ToString());
                        data.Close();
                        foreach (var s in gh.gh)
                        {
                            string sql2 = "insert into CHITIECDONHANG (id_sp, id_dh, gia, soluongmua) values (" + s.MaSP + ", " + iddh + ", " + s.GiaBan + ", " + s.SoLuong + ")";
                            SqlCommand comm2 = new SqlCommand(sql2, conn);
                            int n = comm2.ExecuteNonQuery();
                        }
                        Session["cart"] = null;
                        string sql3 = "delete from GIOHANG where id_users = " + Session["user_id"].ToString();
                        SqlCommand comm3 = new SqlCommand(sql3, conn);
                        comm3.ExecuteNonQuery();
                        string sqlx = "select count(*) from CHITIECDONHANG C, DONHANG D, SANPHAM S WHERE C.ID_DH=D.ID_DH AND D.TRANGTHAI != 2 AND C.ID_SP=S.ID_SP AND D.ID_USERS=" + Session["user_id"].ToString();
                        comm3 = new SqlCommand(sqlx, conn);
                        data = comm3.ExecuteReader();

                        if (data.HasRows)
                        {
                            data.Read();
                            Session["order"] = data.GetValue(0).ToString();
                        }
                        data.Close();
                    }
                }
            }
            return RedirectToAction("DonHang", "Home");
        }

        public ActionResult HuyDonHang(string iddh)
        {
            if (Session["user_id"] != null)
            {
                string connStr = @"Server=nhom12_qlydoanvat.mssql.somee.com ; Database=nhom12_qlydoanvat ; User ID= nnhom12_SQLLogin_1;  password=pcr1e1eogu";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "update DonHang set TrangThai = 2 where id_dh = " + iddh;
                    SqlCommand comm = new SqlCommand(sql, conn);
                    int rowsAffected = comm.ExecuteNonQuery();
                    string sql2 = "select count(*) from CHITIECDONHANG C, DONHANG D, SANPHAM S WHERE C.ID_DH=D.ID_DH AND D.TRANGTHAI != 2 AND C.ID_SP=S.ID_SP AND D.ID_USERS=" + Session["user_id"].ToString();
                    comm = new SqlCommand(sql2, conn);
                    SqlDataReader data;
                    data = comm.ExecuteReader();
                    if (data.HasRows)
                    {
                        data.Read();
                        Session["order"] = data.GetValue(0).ToString();
                    }
                }
            }
            return RedirectToAction("DonHang", "Home");
        }
    }
}