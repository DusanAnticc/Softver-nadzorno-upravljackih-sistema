using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class Program
    {
        public class Callback : ServiceReference3.IAlarmCallback
        {
            public void WriteToAlarmFile(string message)
            {
                int number = Int32.Parse(message.Substring(0, 1));
                for (int i = 0; i < number; i++)
                    Console.WriteLine($"[ALARM] Written \"{message}\" to file as alarming data");
            }

            public void WriteToConsole(string message)
            {
                int number = Int32.Parse(message.Substring(0, 1));
                for (int i = 0; i < number; i++)
                    Console.WriteLine($"[CONSOLE] Alarm arrived: {message}");
            }
        }

        static ServiceReference3.AlarmClient ac;
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            ac = new ServiceReference3.AlarmClient(ic);

            ac.ConsoleSubInit();

            Console.ReadKey();
        }
    }
}
