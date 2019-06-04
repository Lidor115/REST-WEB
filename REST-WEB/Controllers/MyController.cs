using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {
        private ClientModel ClientModel = ClientModel.Instance;
        [HttpGet]
        public ActionResult display(string ip1, string ip2, string ip3, string ip4, int port, int time)
        {
            string ip = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
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
            return sb.ToString();
        }
        /*
            double lonD = double.Parse(ClientModel.point.Lon);
            lonD += 100;
            ClientModel.point.Lon = lonD.ToString();
            ClientModel.Lon = lonD; 
            double latD = double.Parse(ClientModel.point.Lat);
            latD -= 100;
            ClientModel.Lat = latD;
            ClientModel.point.Lat = latD.ToString();
            
        }
        */

        [HttpPost]
        public string SaveToXML()
        {
            string lon = ClientModel.Lon.ToString();
            string lat = ClientModel.Lat.ToString();
            ClientModel.point.Lon = lon;
            ClientModel.point.Lat = lat;
            ToXml(ClientModel.point);
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + ClientModel.Name + ".xml";
            DBHandler.Instance.SaveData(filename);
            return ToXml(ClientModel.point);

        }

    }
}
