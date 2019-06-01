using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace REST_WEB.Models
{
    public class Point
    {
        public string Lon { get; set; }
        public string Lat { get; set; }
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", this.Lon);
            writer.WriteElementString("Lat", this.Lat);
            writer.WriteEndElement();
        }
    }
}