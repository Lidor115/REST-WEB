﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
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

        public ActionResult DisplayFile(string name, int time)
        {
            Session["time"] = time;
            ClientModel.ReadFile(name);
            return View();

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
            /*
            double lonD = double.Parse(ClientModel.point.Lon);
            lonD += 100;
            ClientModel.point.Lon = lonD.ToString();
            ClientModel.Lon = lonD;
            double latD = double.Parse(ClientModel.point.Lat);
            latD -= 100;
            ClientModel.Lat = latD;
            ClientModel.point.Lat = latD.ToString();
*/
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
            */
            return sb.ToString();
        }


        [HttpPost]
        public string SaveToXML()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + ClientModel.Name + ".txt";
            if (!System.IO.File.Exists(filename))
            {
                var lon = ClientModel.Instance.Lon;
                var lat = ClientModel.Instance.Lat;
                Point p = new Point();
                p.Lat = lat.ToString();
                p.Lon = lon.ToString();
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(filename, settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("Location");
                writer.WriteAttributeString("Count", "1");
                writer.WriteStartElement("Lon");
                writer.WriteString(p.Lon.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Lat");
                writer.WriteString(p.Lat.ToString());
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                return writer.ToString(); ;
            }
            else
            {
                XDocument xDocument = XDocument.Load(filename);
                XElement root = xDocument.Element("Location");

                IEnumerable<XElement> rows = root.Descendants();
                XElement lastRow = rows.Last();
                root.AddAfterSelf(
                   new XElement("Lon", ClientModel.Instance.Lon),
                   new XElement("Lat", ClientModel.Instance.Lat));
                xDocument.Save(filename);
                return xDocument.ToString();
            }
            //writer = p.ToXml(writer);
            //string data = p.Lon + "," + p.Lat; 
            //ClientModel.Instance.SaveToFile(data);
            //return writer.ToString();
        }

    }
}
