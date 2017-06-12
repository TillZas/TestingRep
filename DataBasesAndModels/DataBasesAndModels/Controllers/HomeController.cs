using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataBasesAndModels.Models;
using System.Data.Entity;

namespace DataBasesAndModels.Controllers
{
    public class HomeController : Controller
    {

        TownContext database = new TownContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Characters(int page=1)
        {
            int ipp = 5;

            if (HttpContext.Request.Cookies["ItemPerPage"] != null)
            {
                if (!Int32.TryParse(HttpContext.Request.Cookies["ItemPerPage"].Value, out ipp))
                {
                    ipp = 5;
                    HttpContext.Response.Cookies["ItemPerPage"].Value = ipp.ToString();
                }
            }
            else
            {
                HttpContext.Response.Cookies["ItemPerPage"].Value = ipp.ToString();
            }
            List<Character> chars = database.Characters.Include(p => p.Street).ToList();
            IEnumerable<Character> charsPP = chars.Skip((page - 1) * ipp).Take(ipp);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = ipp, TotalItems = chars.Count };
            CharacterViewModel cvm = new CharacterViewModel { PageInfo = pageInfo, Characters = charsPP };

            return View(cvm);
        }

        public ActionResult Streets(int page = 1)
        {
            int ipp = 5;

            if (HttpContext.Request.Cookies["ItemPerPage"] != null)
            {
                if (!Int32.TryParse(HttpContext.Request.Cookies["ItemPerPage"].Value, out ipp))
                {
                    ipp = 5;
                    HttpContext.Response.Cookies["ItemPerPage"].Value = ipp.ToString();
                }
            }
            else
            {
                HttpContext.Response.Cookies["ItemPerPage"].Value = ipp.ToString();
            }

            List<Street> streets = database.Streets.ToList();
            IEnumerable<Street> streetsPP = streets.Skip((page - 1) * ipp).Take(ipp);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = ipp, TotalItems = streets.Count };
            StreetViewModel svm = new StreetViewModel { PageInfo = pageInfo, Streets = streetsPP };


            return View(svm);
        }

        public ActionResult About(int id)
        {
            Character ch = database.Characters.Find(id);
            if (ch == null) return HttpNotFound();
            Character father = ch.FatherId > 0 ? database.Characters.Find(ch.FatherId) : null;
            Character mother = ch.MotherId > 0 ? database.Characters.Find(ch.MotherId) : null;
            Character couple = ch.CoupleId > 0 ? database.Characters.Find(ch.CoupleId) : null;
            Street str = (ch.HouseId > 0) ? database.Streets.Find(ch.StreetId) : null;
            ViewBag.Id = id;
            ViewBag.Name = ch.Name;
            ViewBag.Surname = ch.Surname;
            ViewBag.Age = ch.Age;
            ViewBag.Gender = ch.Gender == 1 ? "М" : "Ж";
            ViewBag.FatherName = father!=null ? father.Name + " " + father.Surname : "Без отца";
            ViewBag.MotherName = mother!=null ? mother.Name + " " + mother.Surname : "Без матери";
            ViewBag.CoupleName = couple!=null ? couple.Name + " " + couple.Surname : "Без пары";
            ViewBag.HouseName = ch.HouseId>0 && str != null ? str!=null? "ул." + str.Name + " д." + ch.HouseId : "Дом номер "+ ch.HouseId : "Бездомный";
            return View();
        }

        [HttpGet]
        public ActionResult EditCharacter(int? id)
        {
            if (id == null) return HttpNotFound();

            Character ch = database.Characters.Find(id);
            if (ch != null)
            {
                SelectList streets = new SelectList(database.Streets, "Id", "Name");
                ViewBag.Streets = streets;
                return View(ch);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditCharacter(Character ch)
        {
            database.Entry(ch).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("About", "Home", new { id = ch.Id });
        }

        [HttpGet]
        public ActionResult CreateCharacter()
        {
            SelectList streets = new SelectList(database.Streets, "Id", "Name");
            ViewBag.Streets = streets;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCharacter(Character ch)
        {
            database.Entry(ch).State = EntityState.Added;
            database.SaveChanges();

            return RedirectToAction("Characters");
        }

        public ActionResult DeleteCharacter(int id)
        {
            Character ch = database.Characters.Find(id);
            if(ch == null)
            {
                return HttpNotFound();
            }
            return View(ch);
        }

        [HttpPost,ActionName("DeleteCharacter")]
        public ActionResult DeleteCharacterConfirm(int id)
        {
            Character ch = database.Characters.Find(id);
            if (ch == null)
            {
                return HttpNotFound();
            }
            database.Entry(ch).State = EntityState.Deleted;
            database.SaveChanges();
            return RedirectToAction("Characters");
        }

        public ActionResult AboutStreet(int? id)
        {
            if (id == null) return HttpNotFound();

            Street str = database.Streets.Include(c => c.Characters).FirstOrDefault(c => c.Id == id);

            if(str == null) return HttpNotFound();

            return View(str);

        }

        protected override void Dispose(bool disposing)
        {
            database.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult CreateStreet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateStreet(Street str)
        {
            database.Entry(str).State = EntityState.Added;
            database.SaveChanges();

            return RedirectToAction("Streets");
        }
    }
}