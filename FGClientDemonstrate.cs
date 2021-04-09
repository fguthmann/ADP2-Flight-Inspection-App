using System.Collections.Generic;
using System.Xml;
using System.Runtime.InteropServices;
using System;
using System.Threading;

//Those lines should be copied to setting-> aditional setting in the FlightGear
//--generic=socket,in,40,127.0.0.1,5400,udp,playback_small
//--fdm = null
namespace ConsoleApp3
{
    class FGClientDemonstrate
    {
        //constructor
        [DllImport("DllForAnomalyDetector.dll")]
        public static extern IntPtr Create(string csvFileName, string[] attributes, int size);

        


        static int Main(string[] args)
        {
            //initializing the timeseries and FGClient
            //read attributes from XML
            List<string> names = new List<string>();
            //as default, could be nice to let the user change the source xml
            string adress = "playback_small.xml";
            XmlParser p = new XmlParser(adress);
            string startNode = "input";
            p.buildingTS(names, startNode);


            //create ts
            string csvLocation = "reg_flight.csv";
            IntPtr ts = Create(csvLocation, names.ToArray(), names.Count);
            Console.WriteLine("TS created");

            //create FGClient
            IData data = new TSDataAdapter(ts);
            FGClient client = new FGClient(data);
            Console.WriteLine("client created");

            //run the client on diffrenet thread
            Thread thread = new Thread(new ThreadStart(client.Start));
            thread.Start();

            
            //test functionalities
            //normal flight
            Thread.Sleep(2000);
            Console.WriteLine("Frames per second is:    {0}\n" +
                "Current frame is:  {1}", client.FramesPerSecond, client.CurrentFrame);
            Thread.Sleep(25000);

            //speed 4x
            client.FramesPerSecond = 4 * client.FramesPerSecond;
            Console.WriteLine("Frames per second is:    {0}\n" +
                "Current frame is:  {1}", client.FramesPerSecond, client.CurrentFrame);

            //speed 8x
            Thread.Sleep(15000); client.FramesPerSecond = 2 * client.FramesPerSecond;
            Console.WriteLine("Frames per second is:    {0}\n" +
                "Current frame is:  {1}", client.FramesPerSecond, client.CurrentFrame);
            Thread.Sleep(15000);

            Console.WriteLine("return to default speed");
            client.FramesPerSecond = 10;
            Thread.Sleep(10000);
            Console.WriteLine("Frames per second is:    {0}\n" +
                "Current frame is:  {1}", client.FramesPerSecond, client.CurrentFrame);
            
            //jummp to speciefic time
            Console.WriteLine("Jump to frame 300");
            client.CurrentFrame = 300;
            Console.WriteLine("Frames per second is:    {0}\n" +
                "Current frame is:  {1}", client.FramesPerSecond, client.CurrentFrame);
            Thread.Sleep(10000);

            Console.WriteLine("lower speed till zero");
            while(client.FramesPerSecond > 0)
            {
                client.FramesPerSecond--;
                Console.WriteLine("current speed {0}", client.FramesPerSecond);
                Thread.Sleep(300);
            }

            //lowest speed possible is 0 fps, treated as pause
            client.FramesPerSecond--;
            Console.WriteLine("current speed {0}", client.FramesPerSecond);
            Thread.Sleep(5000);

            Console.WriteLine("Paused");
            client.Pause = true;
            Thread.Sleep(2000);

            Console.WriteLine("return to default speed but stay Paused");
            client.FramesPerSecond = 10;
            Thread.Sleep(2000);

            Console.WriteLine("continue");
            client.Pause = false;
            client.FramesPerSecond = 10;
            Thread.Sleep(10000);


            client.Pause = true;
            Console.WriteLine("Paused");
            Thread.Sleep(3000);


            Console.WriteLine("set speed to zero while pausing");
            client.FramesPerSecond = 0;
            Thread.Sleep(3000);


            Console.WriteLine("end pause but speed still zero");
            client.Pause = false;
            Thread.Sleep(3000);


            Console.WriteLine("return to default speed");
            client.FramesPerSecond = 10;
            Thread.Sleep(10000);

            Console.WriteLine("read vertical-speed-fps attribute:");
            int n = client.MaxFrame;
            for (int i = 4; i < n; i++)
            {
                Console.Write("{0}      ", client.GetElement("vertical-speed-fps", i));
            }

            Console.WriteLine("end flight");
            client.Close();
            Thread.Sleep(1000);
            return 0;
        }
    }
}
