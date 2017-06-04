using ControllersLearn.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControllersLearn.Controllers
{
    public class HomeController : Controller
    {
        Random rnd = new Random();

        public ActionResult Index()
        {
            /*string browser = HttpContext.Request.Browser.Browser;
            string user_agent = HttpContext.Request.UserAgent;
            string url = HttpContext.Request.RawUrl;
            string ip = HttpContext.Request.UserHostAddress;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;
            return "<p>Browser: " + browser + "</p><p>User-Agent: " + user_agent + "</p><p>Url запроса: " + url +
                "</p><p>Реферер: " + referrer + "</p><p>IP-адрес: " + ip + "</p>";*/
           return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestText()
        {
            return new HtmlResult("<h2>Text</h2>");
        }

        public ActionResult GetImage(int id = -1)
        {
            if (id < 0) id = rnd.Next(5);
            if (id > 4) return HttpNotFound();
            return new ImageResult("/./Controllers/Imgs/"+id+".png");
        }

        public FileResult GetFile(int id = -1)
        {
            if (id < 0) id = rnd.Next(5);
            if (id > 4) id = 4;
            string file_path = Server.MapPath("/./Controllers/Imgs/"+id+".png");
            string file_type = "png";
            return File(file_path, file_type);

        }
    }
}