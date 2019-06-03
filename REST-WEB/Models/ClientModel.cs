using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace REST_WEB.Models
{
    public class ClientModel
    {
        public Point point { get; private set; }
        private readonly static object locker = new object();
        private NetworkStream stream = null;
        private TcpClient client;
        private static ClientModel self = null;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        private bool connected = false;
        List<List<float>> myList;
        private string name; 
        private ClientModel() {
            point = new Point();
        }

        /**
         * The Instance static property for the Singleton getter.
         * */
        public static ClientModel Instance
        {
            get
            {
                lock (locker)
                {
                    if (null == self)
                    {
                        self = new ClientModel();
                    }
                    return self;
                }
            }
        }
        public int time { get; set; }


        private bool isConnected()
        {
            return this.connected;
        }

        //Save route to file
        public void SaveToFile(string data)
        {
            string fileName = this.Name + ".txt";
            StreamWriter writer = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName);
            writer.WriteLine(data);
           
            writer.Close();
        }

        //Read file
        public void ReadFile(string name)
        {
            string fileName = name + ".txt";
            string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName);
            List<List<float>> temp = new List<List<float>>();
            for (int i = 0; i < lines.Length; i++)
                temp.Add(Split(lines[i]));
            myList = temp;
        }

        private List<float> Split(string str)
        {
            List<float> list = new List<float>();
            String[] data = str.Split(' ');
            list.Add(float.Parse(data[0]));
            list.Add(float.Parse(data[1]));

            return list;
        }

        /*
        * Open a new Tcp Client connection to the server.
        */
        public void Open(string ip, int port)
        {
            if (connected) return;
            this.client = new TcpClient(ip, port);
            stream = client.GetStream();
            stream.Flush();
            Console.WriteLine("Conncted");
            binaryReader = new BinaryReader(this.stream);
            binaryWriter = new BinaryWriter(this.stream);

        }

        public string Name
        {
            set
            {
                this.Name = value;
            }
            get
            {
                return this.Name;
            }
        }

        /**
         * closes the client and the network stream.
         * */
        public void Close()
        {
            stream.Close();
            client.Close();
        }

        /**
         * Sends the string to the server.
         * */
        private double GetInfo(string toSend)
        {
            lock (locker) { 
            // convert the command string to an array of bytes.
            binaryWriter.Write(Encoding.ASCII.GetBytes(toSend));
            char c;
            string input = "";
            while ((c = binaryReader.ReadChar()) != '\n')
            {
                input += c;
            }
            stream.Flush();
            return Parser(input);
        }
            }

        // TODO - check if there is problem here when we read 4hz
        private double Parser(string toParse)
        {
            string[] words = toParse.Split('\'');
            return Convert.ToDouble(words[1]);
        }

        public double Lon
        {
            get
            {
                double l = GetInfo("get /position/longitude-deg\r\n");
                this.point.Lat = l.ToString();
                return l;
            }
            set {; }
        }

        public double Lat
        {
            get
            {
                double l = GetInfo("get /position/latitude-deg\r\n");
                this.point.Lat = l.ToString();
                return l;


            }
            set {; }

        }


        /**
         * Is the connection open
         * */
        public bool IsConnected()
        {
            return this.client != null;
        }


    }
}