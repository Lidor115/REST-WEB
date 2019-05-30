using System.Web.Mvc;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {

        [HttpGet]
        public ActionResult displayWithTime(string ip, int port, int time)
        {
            ClientModel.Instance.Open(ip, port);
            if (ClientModel.Instance.IsConnected())
            {
                ViewBag.lon = ClientModel.Instance.Lon;
                ViewBag.lat = ClientModel.Instance.Lat;
                ViewBag.time = ClientModel.Instance.time = time;


                Session["time"] = time;
                ClientModel.Instance.Close();
            }
            return View();
        }

        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            ClientModel.Instance.Open(ip, port);
            if (ClientModel.Instance.IsConnected())
            {
                ViewBag.lon = ClientModel.Instance.Lon;
                ViewBag.lat = ClientModel.Instance.Lat;

                ClientModel.Instance.Close();
            }
            return View();
        }

        // GET: save
        [HttpGet]
        public ActionResult save(string ip, int port, int second, int time, string name)
        {
            ClientModel.Instance.Open(ip, port);
            ClientModel.Instance.SaveToFile(name);

            Session["time"] = time;
            Session["second"] = second;
            return View();
        }

        public ActionResult DisplayFile(string name, int time)
        {
            Session["time"] = time;
            // read file
            ClientModel.Instance.ReadFile(name);
            return View();

        }
        public ActionResult Def()
        {
            return View();
        }

    }

}
