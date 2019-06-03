using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {
        private ClientModel ClientModel = ClientModel.Instance;
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            ClientModel.Open(ip, port);
            if (ClientModel.IsConnected())
            {
                Session["time"] = time; 
            }
            return View();
        }

        // GET: save
        [HttpGet]
        public ActionResult save(string ip, int port, int second, int time, string name)
        {
            ClientModel.Open(ip, port);
            ClientModel.Name = name; 
            Session["time"] = second;
            Session["timoutSave"] = time;
            return View();
        }

        [HttpPost]
        public string DisplayFile(string name, int interval)
        {
            Session["interval"] = interval;
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + name + ".xml";
            return DBHandler.Instance.ReadData(filename);

        }
        public ActionResult Def()
        {
            return View();
        }

        /*
        [HttpPost]
        public string GetLonLat()
        {
            string lon = ClientModel.Lon.ToString();
            string lat = ClientModel.Lat.ToString();
            ClientModel.point.Lon = lon;
            ClientModel.point.Lat = lat;
            return ToXml(ClientModel.point);
        }
        public string ToXml(Point point)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            point.ToXml(writer);
            writer.WriteElementString("Lon", point.Lon);
            writer.WriteElementString("Lat", point.Lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            /*
            double lonD = double.Parse(ClientModel.point.Lon);
            lonD += 100;
            ClientModel.point.Lon = lonD.ToString();
            ClientModel.Lon = lonD; 
            double latD = double.Parse(ClientModel.point.Lat);
            latD -= 100;
            ClientModel.Lat = latD;
            ClientModel.point.Lat = latD.ToString();
            
            return sb.ToString();
        }
        */

        [HttpPost]
        public string SaveToXML()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + ClientModel.Name + ".xml";
            return DBHandler.Instance.SaveData(filename);
        }

    }
}
