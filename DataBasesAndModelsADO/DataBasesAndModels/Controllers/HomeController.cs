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
            List<Character> chars = database.getCharacters();
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

            List<Street> streets = database.getStreets();
            IEnumerable<Street> streetsPP = streets.Skip((page - 1) * ipp).Take(ipp);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = ipp, TotalItems = streets.Count };
            StreetViewModel svm = new StreetViewModel { PageInfo = pageInfo, Streets = streetsPP };


            return View(svm);
        }

        public ActionResult About(int id)
        {
            Character ch = database.getCharacter(id);
            if (ch == null) return HttpNotFound();
            Character father = ch.FatherId > 0 ? database.getCharacter(ch.FatherId) : null;
            Character mother = ch.MotherId > 0 ? database.getCharacter(ch.MotherId) : null;
            Character couple = ch.CoupleId > 0 ? database.getCharacter(ch.CoupleId) : null;
            CharacterView cv = new CharacterView();
            ViewBag.Id = cv.Id = id;
            ViewBag.Name = cv.Name = ch.Name;
            ViewBag.Surname = cv.Surname = ch.Surname;
            ViewBag.Age = cv.Age = ch.Age;
            ViewBag.Gender = cv.Gender = ch.Gender == 1 ? "М" : "Ж";
            ViewBag.FatherName = cv.FatherName = father!=null ? father.Name + " " + father.Surname : "Без отца";
            ViewBag.MotherName = cv.MotherName = mother!=null ? mother.Name + " " + mother.Surname : "Без матери";
            ViewBag.CoupleName = cv.CoupleName = couple!=null ? couple.Name + " " + couple.Surname : "Без пары";
            ViewBag.HouseName = cv.HouseName = ch.HouseId>0 && ch.Street != null ? ch.Street!=null? "ул." + ch.Street.Name + " д." + ch.HouseId : "Дом номер "+ ch.HouseId : "Бездомный";
            return View(cv);
        }

        [HttpGet]
        public ActionResult EditCharacter(int? id)
        {
            if (id == null) return HttpNotFound();

            Character ch = database.getCharacter((int)id);
            if (ch != null)
            {
                SelectList streets = new SelectList(database.getStreets(), "Id", "Name");
                ViewBag.Streets = streets;
                return View(ch);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditCharacter(Character ch)
        {
            database.updateCharacter(ch);
            return RedirectToAction("About", "Home", new { id = ch.Id });
        }

        [HttpGet]
        public ActionResult CreateCharacter()
        {
            SelectList streets = new SelectList(database.getStreets(), "Id", "Name");
            ViewBag.Streets = streets;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCharacter(Character ch)
        {
            database.addCharacter(ch);

            return RedirectToAction("Characters");
        }

        public ActionResult DeleteCharacter(int id)
        {
            Character ch = database.getCharacter(id);
            if(ch == null)
            {
                return HttpNotFound();
            }
            return View(ch);
        }

        [HttpPost,ActionName("DeleteCharacter")]
        public ActionResult DeleteCharacterConfirm(int id)
        {
            Character ch = database.getCharacter(id);
            if (ch == null)
            {
                return HttpNotFound();
            }
            database.deleteCharacter(id);
            return RedirectToAction("Characters");
        }

        public ActionResult AboutStreet(int? id)
        {
            if (id == null) return HttpNotFound();

            Street str = database.getStreet((int)id);

            if(str == null) return HttpNotFound();

            return View(str);

        }

        protected override void Dispose(bool disposing)
        {
            //database.Dispose();
            //base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult CreateStreet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateStreet(Street strIn)
        {
            Street str = new Street();
            if (TryUpdateModel(str))
            {
                database.addStreet(str);
                return RedirectToAction("Streets");
            }
            else
            {
                return Content("Not valid field!!!");
            }

        }

        public string GetValues(string someValues)
        {
            return someValues;
        }

        public string GetValue(string someValue)
        {
            return someValue;
        }
    }
}