using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace REST_WEB.Models
{
    public class ClientModel
    {
        private readonly static object locker = new object();
        private NetworkStream stream = null;
        private TcpClient client;
        private static ClientModel self = null;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        private bool connected = false;
        List<List<float>> myList;
        private ClientModel() { }

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
        public void SaveToFile(string name)
        {
            string fileName = name + ".txt";
            StreamWriter writer = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName);
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
                return GetInfo("get /position/longitude-deg\r\n");
            }
        }

        public double Lat
        {
            get
            {
                return GetInfo("get /position/latitude-deg\r\n");
            }
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