using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;


namespace ex1.Model
{
    public class FGHandlerInitializer
    {


        public void RunFlight(FGHandler handler, string pathFileExceptionFile)
        {
            //initializing the timeseries and FGClient
            List<string> names = new List<string>();
            string adress = "playback_small.xml";
            XMLParser p = new XMLParser(adress);

            string startNode = "input";
            p.buildingTS(names, startNode);

            pathFileExceptionFile = "reg_flight.csv";
            IData data = new CSData(names, pathFileExceptionFile);
            handler.InitialHandler(data);

            Thread thread = new Thread(new ThreadStart(handler.Start));
            thread.Start();
        }
    }
}
