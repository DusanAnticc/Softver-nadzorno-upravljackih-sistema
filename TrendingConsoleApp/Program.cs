using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TrendingConsoleApp.ServiceReference2;

namespace TrendingConsoleApp
{
    class Program
    {
        

        public class Callback : ServiceReference2.ITrendingCallback
        {
            public void WriteToAlarmFile(string message)
            {
                Console.WriteLine($"[ALARM] Written \"{message}\" to file as alarming data");
            }

            public void WriteToConsole(string message)
            {
                Console.WriteLine($"[CONSOLE] Message arrived: {message}");
            }

            public void WriteToLogFile(string message)
            {
                Console.WriteLine($"[FILE] Written \"{message}\" to file as regular data");
            }
        }

        static ServiceReference2.TrendingClient sc;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            sc = new ServiceReference2.TrendingClient(ic);

            sc.ConsoleTrendingInit();

            Console.ReadKey();
        }
        
    }
}
