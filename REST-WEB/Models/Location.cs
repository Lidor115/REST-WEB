
using System.Xml;

namespace REST_WEB.Models
{
    /**
     * Point class
     */
    public class Location
    {
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Rudder { get; set; }
        public string Throttle { get; set; }
        public XmlWriter ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Location");
            writer.WriteElementString("Lon", this.Lon);
            writer.WriteElementString("Lat", this.Lat);
            writer.WriteElementString("Rudder", this.Rudder);
            writer.WriteElementString("Throttle", this.Throttle);
            writer.WriteEndElement();
            return writer;
        }
    }
}