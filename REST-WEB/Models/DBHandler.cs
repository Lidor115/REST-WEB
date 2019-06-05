
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace REST_WEB.Models
{
    public class DBHandler 
    {
        private readonly static object locker = new object();
        private static DBHandler self = null;
        private int idx = 0;

        private DBHandler() { }
        ~DBHandler() { cleanDB(); }
        /**
         * Get a singleton instance of the DBHandler
         */
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
        /**
         * Save the Data in th the XML
         */
        public string SaveData(string filename)
        {
            if (!File.Exists(filename))
            {
                XDocument dataTree = new XDocument(new XElement("Location",
                    new XElement("Lon", ClientModel.Instance.Lon),
                    new XElement("Lat", ClientModel.Instance.Lat),
                    new XElement("Rudder", ClientModel.Instance.Rudder),
                    new XElement("Throttle", ClientModel.Instance.Throttle)
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
                   new XElement("Lat", ClientModel.Instance.Lat),
                    new XElement("Rudder", ClientModel.Instance.Rudder),
                    new XElement("Throttle", ClientModel.Instance.Throttle));
                dataTree.Save(filename);
                return dataTree.ToString();
            }
        }

        /**
         * Read the data from the XML file
         */
        public string ReadData(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("This file was not found.");
            XDocument root = XDocument.Load(filename);
            IEnumerable<XElement> dec = root.Element("Location").Descendants();
            if (idx >= dec.Count())
                return "stop";
            XDocument ret = new XDocument(new XElement("Location",
                    dec.ElementAt(idx++),
                    dec.ElementAt(idx++)
                ));
            idx += 2;
            return ret.ToString();
        }

        private void cleanDB()
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\" + ClientModel.Instance.Name + ".xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
                File.WriteAllText(filename, "");
            }


        }
    }
}