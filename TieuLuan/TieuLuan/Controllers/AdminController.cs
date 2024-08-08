using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TieuLuan.Models;

namespace TieuLuan.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult DSDanhMuc()
        {
            var danhMucs = DoanhMuc.LoadDM();
            return View(danhMucs);
        }

        // GET: DoanhMuc/Create
        public ActionResult CreateDM()
        {
            return View();
        }

        // POST: DoanhMuc/Create
        [HttpPost]
        public ActionResult CreateDM(DoanhMuc dm)
        {
            if (ModelState.IsValid)
            {
                

                // Tạo đối tượng SanPham để gọi phương thức ThemSanPham
                DoanhMuc DMModel = new DoanhMuc();
                DMModel.AddDM(dm);

                return RedirectToAction("DSDanhMuc");
            }

            return View(dm);
        }

        // GET: DoanhMuc/Edit/5
        public ActionResult EditDM(string maDM)
        { 

            var danhMuc = DoanhMuc.LoadDM();
            var danhMuc1 = danhMuc.FirstOrDefault(sp => sp.MaDM == maDM);

            return View(danhMuc1);
        }

        // POST: DoanhMuc/Edit/5
        [HttpPost]
        public ActionResult EditDM(DoanhMuc dm)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file hình ảnh nếu có
                

                // Cập nhật sản phẩm trong cơ sở dữ liệu
                DoanhMuc sanPhamModel = new DoanhMuc();
                sanPhamModel.UpdateDM(dm.TenDM, dm.MaDM);

                return RedirectToAction("DSDanhMuc");
            }

            return View(dm);
        }

         //GET: DoanhMuc/Delete/5
        //public ActionResult DeleteDM(string maDM)
        //{
        //    var danhMuc = DoanhMuc.LoadDM().FirstOrDefault(dm => dm.MaDM == maDM);
        //    return View(danhMuc);
        //}

        // POST: DoanhMuc/Delete/5
        [HttpPost]
        public ActionResult DeleteDM(string id)
        {
            try
            {
                var danhmuc = new DoanhMuc();
                danhmuc.DeleteDM(id); // Gọi phương thức để xóa sản phẩm
                return RedirectToAction("DSDanhMuc"); // Quay lại danh sách sản phẩm sau khi xóa
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và có thể trả thông báo lỗi cho người dùng
                ViewBag.ErrorMessage = "Xóa không thành công: " + ex.Message;
                return RedirectToAction("DSDanhMuc"); // Quay lại danh sách sản phẩm
            }
        }

        //San pham
        public ActionResult DSSanPham()
        {
            var sanPhams = SanPham.LoadSP();
            return View(sanPhams);
        }

        public ActionResult CreateSP()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult CreateSP(SanPham model, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/img/anhsanpham"), fileName);
                    HinhAnh.SaveAs(path);

                    model.HinhAnh = fileName;
                }

                // Tạo đối tượng SanPham để gọi phương thức ThemSanPham
                SanPham sanPhamModel = new SanPham();
                sanPhamModel.ThemSanPham(model);

                return RedirectToAction("DSSanPham");
            }

            return View(model);
        }



        // Sửa sản phẩm (Hiển thị form)
        public ActionResult EditSP(string id)
        {
            var sanPhams = SanPham.LoadSP();
            var sanPham = sanPhams.FirstOrDefault(sp => sp.MaSP == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // Sửa sản phẩm (Xử lý dữ liệu từ form)
        [HttpPost]
        public ActionResult EditSP(SanPham model, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file hình ảnh nếu có
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/img/anhsanpham"), fileName);
                    HinhAnh.SaveAs(path);

                    model.HinhAnh = fileName;
                }

                // Cập nhật sản phẩm trong cơ sở dữ liệu
                SanPham sanPhamModel = new SanPham();
                sanPhamModel.SuaSanPham(model);

                return RedirectToAction("DSSanPham");
            }

            return View(model);
        }


        // Xóa sản phẩm (Hiển thị xác nhận xóa)
        //public ActionResult DeleteSP(string id)
        //{
        //    var sanPhams = SanPham.LoadSP();
        //    var sanPham = sanPhams.FirstOrDefault(sp => sp.MaSP == id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sanPham);
        //}

        //// Xóa sản phẩm (Xử lý xác nhận xóa)
        //[HttpPost, ActionName("DeleteSP")]
        //public ActionResult DeleteConfirmedSP(string id)
        //{
        //    var sanPham = new SanPham();
        //    sanPham.XoaSanPham(id);
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public ActionResult DeleteSP(string id)
        {
            try
            {
                var sanPham = new SanPham();
                sanPham.XoaSanPham(id); // Gọi phương thức để xóa sản phẩm
                return RedirectToAction("DSSanPham"); // Quay lại danh sách sản phẩm sau khi xóa
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và có thể trả thông báo lỗi cho người dùng
                ViewBag.ErrorMessage = "Xóa không thành công: " + ex.Message;
                return RedirectToAction("DSSanPham"); // Quay lại danh sách sản phẩm
            }
        }

        //Don Hang
        public ActionResult DSDonHang()
        {
            var donhang = DonHang.loadDH1();
            return View(donhang);
        }

        //public ActionResult CreateDH()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult CreateDH(DonHang model, HttpPostedFileBase HinhAnh)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (HinhAnh != null && HinhAnh.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(HinhAnh.FileName);
        //            var path = Path.Combine(Server.MapPath("~/assets/img/anhsanpham"), fileName);
        //            HinhAnh.SaveAs(path);

        //            model.HinhAnh = fileName;
        //        }

        //        // Tạo đối tượng SanPham để gọi phương thức ThemSanPham
        //        DonHang donhangModel = new DonHang();
        //        donhangModel.ThemDonHang(model);

        //        return RedirectToAction("DSDonHang");
        //    }

        //    return View(model);
        //}



        // Sửa sản phẩm (Hiển thị form)
        public ActionResult EditDH(string id)
        {
            var donhangs = DonHang.loadDH1();
            var donhang = donhangs.FirstOrDefault(sp => sp.MaDH == id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }

        // Sửa sản phẩm (Xử lý dữ liệu từ form)
        [HttpPost]
        public ActionResult EditDH(DonHang model, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file hình ảnh nếu có
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/img/anhsanpham"), fileName);
                    HinhAnh.SaveAs(path);

                    model.HinhAnh = fileName;
                }

                // Cập nhật sản phẩm trong cơ sở dữ liệu
                DonHang donhangModel = new DonHang();
                donhangModel.SuaDonHang(model);

                return RedirectToAction("DSDonHang");
            }

            return View(model);
        }


        // Xóa sản phẩm (Hiển thị xác nhận xóa)
        //public ActionResult DeleteSP(string id)
        //{
        //    var sanPhams = SanPham.LoadSP();
        //    var sanPham = sanPhams.FirstOrDefault(sp => sp.MaSP == id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sanPham);
        //}

        //// Xóa sản phẩm (Xử lý xác nhận xóa)
        //[HttpPost, ActionName("DeleteSP")]
        //public ActionResult DeleteConfirmedSP(string id)
        //{
        //    var sanPham = new SanPham();
        //    sanPham.XoaSanPham(id);
        //    return RedirectToAction("Index");
        //}

      
        [HttpPost]
        public ActionResult DeleteDH(string id)
        {
            try
            {
                var donhang = new DonHang();
                donhang.XoaDonHang(id); // Gọi phương thức để xóa sản phẩm
                return RedirectToAction("DSDonHang"); // Quay lại danh sách sản phẩm sau khi xóa
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và có thể trả thông báo lỗi cho người dùng
                ViewBag.ErrorMessage = "Xóa không thành công: " + ex.Message;
                return RedirectToAction("DSDonHang"); // Quay lại danh sách sản phẩm
            }
        }

        public ActionResult DSKhachHang()
        {
            var kh = KhachHang.LoadKh();
            return View(kh);
        }
       


    }
}