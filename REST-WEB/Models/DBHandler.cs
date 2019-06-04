
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace REST_WEB.Models
{
    public class DBHandler 
    {
        private readonly static object locker = new object();
        private static DBHandler self = null;

        private DBHandler() { }

        public static DBHandler Instance
        {
            get
            {
                lock (locker)
                {
                    if (null == self)
                    {
                        self = new DBHandler();
                    }
                    return self;
                }
            }
        }

        public string SaveData(string filename)
        {
            if (!File.Exists(filename))
            {
                XDocument dataTree = new XDocument(new XElement("Location",
                    new XElement("Lon", ClientModel.Instance.Lon),
                    new XElement("Lat", ClientModel.Instance.Lat)
                ));
                dataTree.Save(filename);
                return dataTree.ToString();
            }
            else
            {
                XDocument dataTree = XDocument.Load(filename);
                XElement root = dataTree.Element("Location");

                IEnumerable<XElement> rows = root.Descendants();
                XElement lastRow = rows.Last();
                lastRow.AddAfterSelf(
                   new XElement("Lon", ClientModel.Instance.Lon),
                   new XElement("Lat", ClientModel.Instance.Lat));
                dataTree.Save(filename);
                return dataTree.ToString();
            }
        }

        public string ReadData(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("This file was not found.");
            XDocument doc = XDocument.Load(filename);
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            doc.WriteTo(writer);
            return sb.ToString();
        }
    }
}