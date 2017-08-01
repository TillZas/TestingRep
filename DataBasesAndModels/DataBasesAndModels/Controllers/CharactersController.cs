using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataBasesAndModels.Models;

namespace DataBasesAndModels.Controllers
{
    public class CharactersController : Controller
    {
        private TownContext db = new TownContext();

        // GET: Characters
        public async Task<ActionResult> Index()
        {
            var characters = db.Characters.Include(c => c.Street);
            return View(await characters.ToListAsync());
        }

        // GET: Characters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = await db.Characters.FindAsync(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // GET: Characters/Create
        public ActionResult Create()
        {
            ViewBag.StreetId = new SelectList(db.Streets, "Id", "Name");

            List<SelectListItem> genders = new List<SelectListItem>();
            genders.Add(new SelectListItem() { Text = "Мужчина", Value = "1" });
            genders.Add(new SelectListItem() { Text = "Женщина", Value = "2" });

            ViewBag.GenderOptions = new SelectList(genders, "Value", "Text");

            List<SelectListItem> mothersList;

            mothersList = db.Characters
            .Where(item => item.Gender == 2)
            .Select(c => new SelectListItem()
            {
                Text = (c.Name + " " + c.Surname),
                Value = c.Id.ToString()
            }).ToList();
            mothersList.Insert(0, new SelectListItem() { Text = "Без матери", Value = "-10" });
            ViewBag.Mothers = new SelectList(mothersList, "Value", "Text"); ;

            List<SelectListItem> fathersList = db.Characters
                .Where(item => item.Gender == 1)
                .Select(c => new SelectListItem()
                {
                    Text = (c.Name + " " + c.Surname),
                    Value = c.Id.ToString()
                }).ToList();
            fathersList.Insert(0, new SelectListItem() { Text = "Без отца", Value = "-10" });
            ViewBag.Fathers = new SelectList(fathersList, "Value", "Text");

            List<SelectListItem> coupleList = db.Characters
                .Select(c => new SelectListItem()
                {
                    Text = (c.Name + " " + c.Surname),
                    Value = c.Id.ToString()
                }).ToList();
            coupleList.Insert(0, new SelectListItem() { Text = "Нет пары", Value = "-10" });
            ViewBag.Couples = new SelectList(coupleList, "Value", "Text");

            return View();
        }

        // POST: Characters/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Surname,Age,Gender,FatherId,MotherId,CoupleId,HouseId,StreetId")] Character character)
        {
            if (ModelState.IsValid)
            {
                db.Characters.Add(character);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StreetId = new SelectList(db.Streets, "Id", "Name", character.StreetId);
            return View(character);
        }

        // GET: Characters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = await db.Characters.FindAsync(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            ViewBag.StreetId = new SelectList(db.Streets, "Id", "Name", character.StreetId);
            return View(character);
        }

        // POST: Characters/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Surname,Age,Gender,FatherId,MotherId,CoupleId,HouseId,StreetId")] Character character)
        {
            if (ModelState.IsValid)
            {
                db.Entry(character).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StreetId = new SelectList(db.Streets, "Id", "Name", character.StreetId);
            return View(character);
        }

        // GET: Characters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = await db.Characters.FindAsync(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Character character = await db.Characters.FindAsync(id);
            db.Characters.Remove(character);
            await db.SaveChangesAsync();
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
