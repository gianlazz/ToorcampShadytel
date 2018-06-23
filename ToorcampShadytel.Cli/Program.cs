using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToorcampShadytel.Cli
{
    class Program
    {
        public static System.IO.Ports.SerialPort sport;
        public static List<string> PortNames { get; set; }
        public static string Port { get; set; }
        public static int Baudrate { get; private set; }
        public static int Databits { get; private set; }
        public static Parity ParitySetting { get; private set; }
        public static StopBits StopBitSetting { get; set; }

        static void Main(string[] args)
        {
            PortNames = new List<string>();
            foreach (String p in System.IO.Ports.SerialPort.GetPortNames())
            {
                PortNames.Add(p);
                Console.WriteLine(p);
            }
            Console.WriteLine();
            Console.WriteLine("Input your port from the list above:");
            Port = (Console.ReadLine());
            Console.WriteLine("Input your Baudrate:");
            Baudrate = int.Parse(Console.ReadLine());
            Console.WriteLine("Parity set to None");
            ParitySetting = (Parity)Enum.Parse(typeof(Parity), "None");
            Console.WriteLine("Databit set to 8");
            Databits = 8;
            Console.WriteLine("Stopbits set to 1");
            //Databits = int.Parse(Console.ReadLine());
            StopBitSetting = (StopBits)Enum.Parse(typeof(StopBits), "One");

            Console.WriteLine("Press enter to connect: ");
            Console.ReadKey();
            SerialportConnect(Port, Baudrate, ParitySetting, Databits, StopBitSetting);
        }

        private static void SerialportConnect(string port, int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            sport = new System.IO.Ports.SerialPort(
            port, baudrate, parity, databits, stopbits);
            try
            {
                sport.Open();
                Console.WriteLine("[" + dtn + "] " + "Connected\n");
                sport.DataReceived += new SerialDataReceivedEventHandler(sport_DataReceived);
            }
            catch (Exception ex) { throw; }
        }

        private static void sport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] Received: {sport.ReadExisting()}");
        }

        private void SendData(string data)
        {
            sport.Write(data);
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] Sent: {data}");
        }

        private void CloseConnection()
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();

            if (sport.IsOpen)
            {
                sport.Close();
                Console.WriteLine("[" + dtn + "] " + "Disconnected\n");
            }
        }
    }
}