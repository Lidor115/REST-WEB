
using System.Xml;

namespace REST_WEB.Models
{
    /**
     * Point class
     */
    public class Point
    {
        public string Lon { get; set; }
        public string Lat { get; set; }
        public XmlWriter ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Point");
            writer.WriteElementString("Lon", this.Lon);
            writer.WriteElementString("Lat", this.Lat);
            writer.WriteEndElement();
            return writer;
        }
    }
}