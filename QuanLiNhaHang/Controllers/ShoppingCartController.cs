using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLiNhaHang.Models;

namespace QuanLiNhaHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private CT25Team111Entities db = new CT25Team111Entities();
        private List<Chitietgiohang> ShoppingCart = null;
        private Sanpham sanpham;

        public ShoppingCartController()
        {
            if (Session["ShoppingCart"] != null)
                ShoppingCart = Session["ShoppingCart"] as List<Chitietgiohang>;
            else
            {
                ShoppingCart = new List<Chitietgiohang>();
                Session["ShoppingCart"] = ShoppingCart;
            }

        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View(ShoppingCart);
        }

        // GET: ShoppingCart/Create
        [HttpPost]
        public ActionResult Create(int masp, int soluong)
        {
            var donhang = db.Chitietdonhangs.Find(masp);
            ShoppingCart.Add(new Chitietgiohang
            {
                Sanpham = sanpham,
                Số_lượng = soluong

            });
            return RedirectToAction("Index");
        }


        // GET: ShoppingCart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chitietgiohang chitietgiohang = db.Chitietgiohangs.Find(id);
            if (chitietgiohang == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mã_GH = new SelectList(db.Giohangs, "Mã_GH", "Email", chitietgiohang.Mã_GH);
            ViewBag.Mã_SP = new SelectList(db.Sanphams, "Mã_SP", "Mã_loại_SP", chitietgiohang.Mã_SP);
            return View(chitietgiohang);
        }

        

        // GET: ShoppingCart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chitietgiohang chitietgiohang = db.Chitietgiohangs.Find(id);
            if (chitietgiohang == null)
            {
                return HttpNotFound();
            }
            return View(chitietgiohang);
        }

        // POST: ShoppingCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chitietgiohang chitietgiohang = db.Chitietgiohangs.Find(id);
            db.Chitietgiohangs.Remove(chitietgiohang);
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
