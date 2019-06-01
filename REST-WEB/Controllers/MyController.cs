using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {
        /*
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
        */
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            ClientModel.Instance.Open(ip, port);
            if (ClientModel.Instance.IsConnected())
            {
                ClientModel.Instance.time = time;
                ViewBag.lon = ClientModel.Instance.Lon;
                ViewBag.lat = ClientModel.Instance.Lat;
                Session["time"] = time;
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
            ClientModel.Instance.ReadFile(name);
            return View();

        }
        public ActionResult Def()
        {
            return View();
        }


        [HttpPost]
        public string GetLonLat()
        {
            var client = ClientModel.Instance;


            return ToXml(client.Lon, client.Lat);
        }
        private string ToXml(double lon , double lat)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");

            writer.WriteElementString("lon", lon.ToString());
            writer.WriteElementString("lat", lat.ToString());

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        [HttpPost]
        public string SaveToXML()
        {
            List<float> point = new List<float>();

            var lon = ClientModel.Instance.Lon;
            var lat = ClientModel.Instance.Lat;
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(builder, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            //TODO: Save all parameters
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return builder.ToString();
        }

    }

}
