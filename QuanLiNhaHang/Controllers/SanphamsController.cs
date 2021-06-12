using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using QuanLiNhaHang.Models;

namespace QuanLiNhaHang.Controllers
{
    [Authorize]
    public class SanphamsController : Controller
    {
        private CT25Team111Entities db = new CT25Team111Entities();
        [Authorize(Roles = "Admin")]
        // GET: Sanphams
        public ActionResult Index()
        {
            var sanphams = db.Sanphams.Include(s => s.Loaisanpham);
            return View(sanphams.ToList());
        }

        // for khachhang
        [AllowAnonymous]
        public ActionResult Index2()
        {
            var sanphams = db.Sanphams.Include(s => s.Loaisanpham);
            return View(sanphams.ToList());
        }
        // hàm search

        [AllowAnonymous]
        public ActionResult Search(String keyword)
        {
            var model = db.Sanphams.ToList();
            model = model.Where(p => p.Tên_món_ăn.ToLower().Contains(keyword.ToLower())).ToList();
            ViewBag.Keyword = keyword;
            return View("Index2", model);

        }
        // GET: Sanphams/Details/5
        [AllowAnonymous]
        public ActionResult Details(string id)

        {
          var model = db.Sanphams.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Picture(string Mã_SP)
        {
            var path = Server.MapPath(PICTURE_PATH);
            return File(path + Mã_SP, "images");
        }
        // GET: Sanphams/Create
        public ActionResult Create()
        {

            ViewBag.Mã_loại_SP = new SelectList(db.Loaisanphams, "Mã_loại_SP", "Tên_loại_SP");
            return View();
        }

        // POST: Sanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sanpham model, HttpPostedFileBase picture)
        {
            ValidateProduct(model);
            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        db.Sanphams.Add(model);
                        db.SaveChanges();

                        // store picture
                        var path = Server.MapPath(PICTURE_PATH);
                        picture.SaveAs(path + model.Mã_SP);

                        scope.Complete();
                        return RedirectToAction("Index");
                    }

                }
                else ModelState.AddModelError("", "hình ảnh không được tìm thấy!");
            }

            ViewBag.Mã_loại_SP = new SelectList(db.Loaisanphams, "Mã_loại_SP", "Tên_loại_SP", model.Mã_loại_SP);
            return View(model);
        }
        private const string PICTURE_PATH = "~/Upload/Sanphams/";
        public void ValidateProduct(Sanpham sanpham)
        {
            if (sanpham.Giá_tiền < 0)
                ModelState.AddModelError("Giá_tiền", "Giá tiền phải lớn hơn 0");
            if (sanpham.Số_lượng < 0)
                ModelState.AddModelError("Số_lượng", "Số lượng phải lớn hơn 0");
        }

        // GET: Sanphams/Edit/5
        public ActionResult Edit(string id)
        {
            var model = db.Sanphams.Find(id);
            if (model == null)
            {
                return HttpNotFound();  
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mã_loại_SP = new SelectList(db.Loaisanphams, "Mã_loại_SP", "Tên_loại_SP", model.Mã_loại_SP);
            return View(model);
        }

        // POST: Sanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sanpham model, HttpPostedFileBase picture)
        {
            ValidateProduct(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    if (picture != null)
                    {
                        var path = Server.MapPath(PICTURE_PATH);
                        picture.SaveAs(path + model.Mã_SP);
                    }

                    scope.Complete();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Mã_loại_SP = new SelectList(db.Loaisanphams, "Mã_loại_SP", "Tên_loại_SP", model.Mã_loại_SP);
            return View(model);
        }

        // GET: Sanphams/Delete/5
        public ActionResult Delete(string id)
        {

            var model = db.Sanphams.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            using (var scope = new TransactionScope())
            {
                var model = db.Sanphams.Find(id);
                db.Sanphams.Remove(model);
                db.SaveChanges();

                var path = Server.MapPath(PICTURE_PATH);
                System.IO.File.Delete(path + model.Mã_SP);

                scope.Complete();
                return RedirectToAction("Index");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
