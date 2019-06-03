
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
               /* XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(filename, settings);
                XElement xmlTree = new XElement("Location",
                    new XElement("Lon", ClientModel.Instance.Lon),
                    new XElement("Lat", ClientModel.Instance.Lat)
                );
                xmlTree.WriteTo(writer);
                writer.Flush();
                writer.Close();*/
            }
            else
            {
                XDocument dataTree = XDocument.Load(filename);
                XElement root = dataTree.Element("Location");

                IEnumerable<XElement> rows = root.Descendants();
                XElement lastRow = rows.Last();
                root.AddAfterSelf(
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
            return XDocument.Load(filename).ToString();
        }
    }
}