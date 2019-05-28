using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {
            [HttpGet]
            public ActionResult display(string ip, int port)
            {
                ViewBag.lon = 50;
                ViewBag.lat = 50;
                return View();
            }
            // GET: save
            [HttpGet]
            public ActionResult save(string ip1, string ip2, string ip3, string ip4, int port, int time, int duration, string nameFile)
            {
                string ip = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
                return View();
            }
        public ActionResult Def()
        {
            return View();
        }

    }

}
