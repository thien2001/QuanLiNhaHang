using System;
using System.Collections;
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
        // GET: ShoppingCart

      

        [Authorize ]
        public ActionResult Index()
        {
            GetShoppingCart();
            var hashtable = new Hashtable();
            foreach (var Chitietdonhang in ShoppingCart)
            {
                if (hashtable[Chitietdonhang.Sanpham.Mã_SP] != null)
                {
                    (hashtable[Chitietdonhang.Sanpham.Mã_SP] as Chitietdonhang).Số_lượng += Chitietdonhang.Số_lượng;
                }
                else hashtable[Chitietdonhang.Sanpham.Mã_SP] = Chitietdonhang;
            }

            ShoppingCart.Clear();
            foreach (Chitietdonhang Chitietdonhang in hashtable.Values)
                ShoppingCart.Add(Chitietdonhang);
            return View(ShoppingCart);
        }


        // GET: ShoppingCart/Create
        [HttpPost]
        public ActionResult Create(string Mã_SP, int số_lượng)
        {
            GetShoppingCart();
            var sanpham = db.Sanphams.Find(Mã_SP);
            ShoppingCart.Add(new Chitietdonhang
            {
                Sanpham = sanpham,
                Số_lượng = số_lượng


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
            Chitietdonhang chitietdonhang = db.Chitietdonhangs.Find(id);
            if (chitietdonhang == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mã_ĐH = new SelectList(db.Donhangs, "Mã_ĐH", "Email", chitietdonhang.Mã_ĐH);
            ViewBag.Mã_SP = new SelectList(db.Sanphams, "Mã_SP", "Mã_loại_SP", chitietdonhang.Mã_SP);
            return View(chitietdonhang);
        }

        // GET: ShoppingCart/Delete/5
        public ActionResult Delete()
        {

            GetShoppingCart();
            ShoppingCart.Clear();
            Session["ShoppingCart"] = ShoppingCart; 
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

