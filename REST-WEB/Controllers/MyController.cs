using System;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using REST_WEB.Models;
namespace REST_WEB.Controllers
{
    public class MyController : Controller
    {
        private ClientModel ClientModel = ClientModel.Instance;
        private string askedName = null;
        /**
         * Display - get the ip with 4 params to be different from Display after the save.
         */
        [HttpGet]
        public ActionResult display(string ip1, string ip2, string ip3, string ip4, int port, int time)
        {
            string ip = ip1 + "." + ip2 + "." + ip3 + "." + ip4;
            ClientModel.Open(ip, port);
            if (ClientModel.IsConnected())
                Session["time"] = time; 
            return View();
        }

        /**
         * save file - from controller to view
         */
        [HttpGet]
        public ActionResult save(string ip, int port, int second, int time, string name)
        {
            ClientModel.Open(ip, port);
            ClientModel.Name = name;
            Session["time"] = second;
            Session["timoutSave"] = time;
            return View();
        }

        /**
         * Display file - sends for the vieww
         */
        [HttpGet]
        public ActionResult DisplayFile(string name, int interval)
        {
            Session["interval"] = interval;
            Session["name"] = name;
            askedName = name;
            return View();

        }
        /**
         * Display from Loading file - XML
         */
        [HttpPost]
        public string DisplayFileToView()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + Session["name"] + ".xml";
            return DBHandler.Instance.ReadData(filename);
        }
        public ActionResult Def()
        {
            return View();
        }



        /**
         * get the lon and the lat from the model and return (to View)
         */
        [HttpPost]
        public string GetLonLat()
        {
            string lon = ClientModel.Lon.ToString();
            string lat = ClientModel.Lat.ToString();
            ClientModel.Lon = double.Parse(lon);
            ClientModel.Lat = double.Parse(lat);
            return ToXml(ClientModel.Location);
        }

        /**
        * Get Lon and Lat by point - parse to XML
        */

        public string ToXml(Location loc)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Azimuth");
            loc.ToXml(writer);
            writer.WriteElementString("Lon", loc.Lon);
            writer.WriteElementString("Lat", loc.Lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        /**
        * Save to Xml File
         */

        [HttpPost]
        public string SaveToXML()
        {
            // get the lon and the lat
            string lon = ClientModel.Lon.ToString();
            string lat = ClientModel.Lat.ToString();
            string rudder = ClientModel.Rudder.ToString();
            string throttle = ClientModel.Throttle.ToString();
            // make  a point
            ClientModel.Lon = double.Parse(lon);
            ClientModel.Lat = double.Parse(lat);
            ClientModel.Rudder = double.Parse(rudder);
            ClientModel.Throttle = double.Parse(throttle);
            // send the point function "ToXml"
            ToXml(ClientModel.Location);
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + ClientModel.Name + ".xml";
            DBHandler.Instance.SaveData(filename);
            return ToXml(ClientModel.Location);
        }

    }
}