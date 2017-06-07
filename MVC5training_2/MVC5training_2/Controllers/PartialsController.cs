using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5training_2.Controllers
{
    public class PartialsController : Controller
    {
        // GET: Partials
        public ActionResult Index()
        {
            ViewBag.Message = "Частичное представление";
            return PartialView();
        }
    }
}