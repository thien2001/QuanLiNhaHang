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
    [Authorize(Roles = "Admin")]
    public class SanphamsController : Controller
    {
        private CT25Team111Entities db = new CT25Team111Entities();

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

        // GET: Sanphams/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mã_loại_SP = new SelectList(db.Loaisanphams, "Mã_loại_SP", "Tên_loại_SP", sanpham.Mã_loại_SP);
            return View(sanpham);
        }

        // POST: Sanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Mã_SP,Mã_loại_SP,Tên_món_ăn,Số_lượng,Giá_tiền,Mô_tả")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Mã_loại_SP = new SelectList(db.Loaisanphams, "Mã_loại_SP", "Tên_loại_SP", sanpham.Mã_loại_SP);
            return View(sanpham);
        }

        // GET: Sanphams/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // POST: Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sanpham sanpham = db.Sanphams.Find(id);
            db.Sanphams.Remove(sanpham);
            db.SaveChanges();
            return RedirectToAction("Index");
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
