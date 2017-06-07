using MVC5training_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5training_2.Controllers
{
    public class HomeController : Controller
    {

        BookContext db = new BookContext();

        public ActionResult Index()
        {
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View(books);
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            if (id == null) return null;
            ViewBag.BookId = id;
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            db.Purchases.Add(purchase);
            db.SaveChanges();
            return RedirectToAction("AfterBuy", "Home", new { name = purchase.Name,address = purchase.Address});
            // + purchase.Name + ", благодарим за покупку книги "+ purchase.BookID.ToString();
        }

        public ActionResult AfterBuy(string name,string address)
        {
            ViewBag.Name = name;
            //ViewBag.Address = address;
            ViewBag.Message = "Товар прийдет к вам на " + address + " примерно никогда.";
            return View();
        }

        public ActionResult PartialTest()
        {
            ViewBag.Message = "Частичное представление";
            return PartialView();
        }
    }
}