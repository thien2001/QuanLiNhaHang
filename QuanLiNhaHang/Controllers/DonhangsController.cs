using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using QuanLiNhaHang.Models;

namespace QuanLiNhaHang.Controllers
{
    public class DonhangsController : Controller
    {
        private CT25Team111Entities db = new CT25Team111Entities();

        private List<Chitietdonhang> ShoppingCart = null;

        private void GetShoppingCart()
        {

            if (Session["ShoppingCart"] != null)
                ShoppingCart = Session["ShoppingCart"] as List<Chitietdonhang>;
            else
            {
                ShoppingCart = new List<Chitietdonhang>();
                Session["ShoppingCart"] = ShoppingCart;
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Donhangs
        public ActionResult Index()
        {
            var donhangs = db.Donhangs.Include(d => d.Trangthaidonhang);
            return View(donhangs.ToList());
        }

        // GET: Donhangs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }

        // GET: Donhangs/Create
        public ActionResult Create()
        {
            GetShoppingCart();
            ViewBag.Cart = ShoppingCart;
            ViewBag.Mã_TT = new SelectList(db.Trangthaidonhangs, "Mã_TT", "Trạng_thái");
            return View();
        }

        // POST: Donhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Donhang donhang)
        {
            ValidateDonhang(donhang);
            if (ModelState.IsValid)
            {
                donhang.Ngày_giao = DateTime.Now;
                donhang.Mã_ĐH = Guid.NewGuid().ToString();
                db.Donhangs.Add(donhang);
                db.SaveChanges();
                return RedirectToAction("Index2","Sanphams");
            }

            GetShoppingCart();
            ViewBag.Cart = ShoppingCart;
            ViewBag.Mã_TT = new SelectList(db.Trangthaidonhangs, "Mã_TT", "Trạng_thái", donhang.Mã_TT);
            return View(donhang);
        }
        private void ValidateDonhang (Donhang donhang)
        {
            var regex = new Regex("[0-9]{3}");
            GetShoppingCart();
            if (ShoppingCart.Count == 0)
                ModelState.AddModelError("", "Không có sản phẩm nào trong giỏ hàng!");
            if (!regex.IsMatch(donhang.SĐT))
                ModelState.AddModelError("SĐT", "Sai số điện thoại");
        }

        // GET: Donhangs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Mã_TT = new SelectList(db.Trangthaidonhangs, "Mã_TT", "Trạng_thái", donhang.Mã_TT);
            return View(donhang);
        }

        // POST: Donhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Donhang donhang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donhang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            ViewBag.Mã_TT = new SelectList(db.Trangthaidonhangs, "Mã_TT", "Trạng_thái", donhang.Mã_TT);
            return View(donhang);
        }

        // GET: Donhangs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(donhang);
        }

        // POST: Donhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Donhang donhang = db.Donhangs.Find(id);
            db.Donhangs.Remove(donhang);
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
