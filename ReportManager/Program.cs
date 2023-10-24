using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManager
{
    class Program
    {
        static ServiceReference4.ReportMenagerClient client = null;

        static void meni()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("alarms in interval [1]:");
            Console.WriteLine("alarms with priority [2]:");
            Console.WriteLine("values of tag in interval [3]:");
            Console.WriteLine("values of AI tags- sorted [4]:");
            Console.WriteLine("values of DI tags- sorted [5]:");
            Console.WriteLine("all values of tag(id)- sorted[6]:");
        }

        static void option1()
        {
            string lista=client.alarmsInInterval(new DateTime(2021, 1, 1), DateTime.Now);
            Console.Clear();
            Console.WriteLine(lista);
        }
        static void option2()
        {
            Console.WriteLine("Priority :");
            int number = Int32.Parse(Console.ReadLine());
            string lista = client.alarmsWithPriority(number);
            Console.Clear();
            Console.WriteLine(lista);
        }
        static void option3()
        {
            string lista = client.tagsInInterval(new DateTime(2021, 1, 1), DateTime.Now);
            Console.Clear();
            Console.WriteLine(lista);
        }
        static void option4()
        {
            string lista = client.aItagsInInterval(new DateTime(2021, 1, 1), DateTime.Now);
            Console.Clear();
            Console.WriteLine(lista);
        }
        static void option5()
        {
            string lista = client.dItagsInInterval(new DateTime(2021, 1, 1), DateTime.Now);
            Console.Clear();
            Console.WriteLine(lista);
        }
        static void option6()
        {
            //nema vise vrijednosti
        }

        
        static void Main(string[] args)
        {
            client = new ServiceReference4.ReportMenagerClient();
            int number = 0;
            do
            {
                meni();
                number = Convert.ToInt32(Console.ReadLine());


                switch (number)
                {
                    case 1:
                        option1();
                        break;
                    case 2:
                        option2();
                        break;
                    case 3:
                        option3();
                        break;
                    case 4:
                        option4();
                        break;
                    case 5:
                        option5();
                        break;
                    case 6:
                        option6();
                        break;                   
                    default:
                        Console.Clear();
                        break;

                }
            } while (number != 0);
        }

        
    }
}
