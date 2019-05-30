using System.Web.Mvc;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {

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
            public ActionResult save(string ip1, string ip2, string ip3, string ip4, int port, int time, int duration, string nameFile)
            {
            // Todo - change all this function
                string ip = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
                return View();
            }
        public ActionResult Def()
        {
            return View();
        }

    }

}
