using DatabaseManager.ServiceReference1;
using SnusProjekat;//DO reference
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class Program
    {
        static ServiceReference1.IDatabaseManager client = null;
        //static ServiceReference1.DatabaseManagerClient client = new ServiceReference1.DatabaseManagerClient();
        static public User session = null;

        static void meni(int option)
        {
            if (session != null)
                if (session.UserName.Count() > 3)
                    Console.WriteLine($"\t\t\t\t\tUser:{session.UserName}");
            Console.WriteLine("select an option:");
            Console.WriteLine("\tAdd tag:[1]");
            Console.WriteLine("\tDelete tag:[2]");
            Console.WriteLine("\tTurning on scaning:[3]");
            Console.WriteLine("\tTurn off scaning:[4]");
            Console.WriteLine("\tEntering output tag values:[5]");
            Console.WriteLine("\tDisplay current output tag values:[6]");
            Console.WriteLine("\tUser registration:[7]");
            Console.WriteLine("\tUser login:[8]");
            Console.WriteLine("\tUser logout:[9]");
            Console.WriteLine("\tAdd alarm:[10]");
            Console.WriteLine("\tDelete alarm:[11]");
        }

       
        static bool ValidateName(string Name)
        {
            if(Name.Length<3 || !(char.IsUpper(Name[0])))
            {
                return false;
            }
            return true;
        }

        static bool ValidatePass(string Name)
        {
            if (Name.Length < 3 )
            {
                return false;
            }
            return true;
        }

        static bool ValidateAdmin(string ad)
        {
            if (ad=="T" || ad=="F")
            {
                return true;
            }
            return false;
        }

        static string inputUser()
        {
            Console.WriteLine("Input User name:");
            string name = Console.ReadLine();

            Console.WriteLine("Input User pass:");
            string pass = Console.ReadLine();

            Console.WriteLine("Is this User admin (T=true,F=false):");
            string ad = Console.ReadLine();

            if (ValidateName(name) && ValidatePass(pass)&&ValidateAdmin(ad))
            {
                return name + "," + pass+","+ad;
            }

            Console.WriteLine("Bad input");
            return " , , ";
        }
     
        static void changeDO(string id)
        {
            Console.WriteLine($"new value for Digital output tag[{id}] (double):");
            string value = Console.ReadLine();

            double tmp = double.Parse(value);
            if (tmp < 5)//za digital manje od 0.5 ali stavio sam 5 radi testiranja
                tmp = 0;
            else
                tmp = 1;
            value = tmp.ToString();

            bool correct = client.InputValuesDO(id, value);
            Console.WriteLine((correct) ? "change completed successfully" : "change completed unsuccessfully");
        }

        static void changeAO(string id)
        {  
            Console.WriteLine($"new value for Analog output tag[{id}]:");
            string value = Console.ReadLine();
            
            bool correct = client.InputValuesAO(id, value);
            Console.WriteLine((correct) ? "change completed successfully" : "change completed unsuccessfully");
        }

        bool ValidateAO()
        {
            return true;
        }        

        static void option1()
        {
            Console.WriteLine("\t\tAdd tag:");
            Console.WriteLine("\t\t\tAnalog output:[1]");
            Console.WriteLine("\t\t\tAnalog input:[2]");
            Console.WriteLine("\t\t\tDigital output:[3]");
            Console.WriteLine("\t\t\tDigital input:[4]");
            int outputOption = Convert.ToInt32(Console.ReadLine());
            if (outputOption == 1)
            {
                bool valid = true;

                Console.WriteLine("Input AO tag name(id):");
                string tg = Console.ReadLine();
                Console.WriteLine("Input AO description:");
                string des = Console.ReadLine();
                Console.WriteLine("Input AO IOAddress:");
                string io = Console.ReadLine();
                Console.WriteLine("Input AO initial value:");
                string iv = Console.ReadLine();
                Console.WriteLine("Input AO low limit:");
                string ll = Console.ReadLine();
                Console.WriteLine("Input AO high limit:");
                string hl = Console.ReadLine();

                //valid = ValidateAO();
                if (valid)
                {
                    AO digitalOutput = new AO()
                    {
                        tagName = tg,
                        description = des,
                        IOaddress = io,
                        initialValue = iv,
                        lowLimit = double.Parse(ll),
                        highLimit = double.Parse(hl)
                    };
                    client.printToConfigAO(digitalOutput);
                    client.inputAO(digitalOutput);
                }   
            }
            else if (outputOption == 2)
            {
                bool valid = true;

                Console.WriteLine("Input AI tag name(id):");
                string tg = Console.ReadLine();
                Console.WriteLine("Input AI description:");
                string des = Console.ReadLine();
                Console.WriteLine("Input AI driver:");
                string dr = Console.ReadLine();
                Console.WriteLine("Input AI IOAddress:");
                string io = Console.ReadLine();
                Console.WriteLine("Input AI scanTime:");
                int sc = Convert.ToInt32(Console.ReadLine());
                //Console.WriteLine("Input AI alarms:");
                //string al = Console.ReadLine();
                Console.WriteLine("Input AI on/offScan[T/F]:");
                string on = Console.ReadLine();
                Console.WriteLine("Input AI low limit:");
                string ll = Console.ReadLine();
                Console.WriteLine("Input AI high limit:");
                string hl = Console.ReadLine();
                Console.WriteLine("Input AI units:");
                string un = Console.ReadLine();

                //valid = ValidateAI();
                if (valid)
                {
                    bool b = true;
                    if (on == "F")
                        b = false;

                    AI digitalOutput = new AI()
                    {
                        tagName = tg,
                        description = des,
                        IOaddress = io,
                        driver = dr,
                        scanTime = sc,
                        onOffscan = b,
                        lowLimit = double.Parse(ll),
                        highLimit = double.Parse(hl),
                        units = un
                    };
                    client.printToConfigAI(digitalOutput);
                    client.inputAI(digitalOutput);                    
                }
                
            }
            if (outputOption == 3)
            {
                bool valid = true;

                Console.WriteLine("Input DO tag name(id):");
                string tg = Console.ReadLine();
                Console.WriteLine("Input DO description:");
                string des = Console.ReadLine();
                Console.WriteLine("Input DO IOAddress:");
                string io = Console.ReadLine();
                Console.WriteLine("Input DO initialValue:");
                string iv = Console.ReadLine();

                //valid = ValidateDO();
                if (valid)
                {
                    double tmp = double.Parse(iv);
                    if (tmp < 5)//za digital manje od 5 ali stavio sam 5 radi testiranja
                        tmp = 0;
                    else
                        tmp = 1;
                    iv = tmp.ToString();

                    DO digitalOutput = new DO()
                    {
                        tagName = tg,
                        description = des,
                        IOaddress = io,
                        initialValue = iv
                    };
                    client.printToConfigDO(digitalOutput);
                    client.inputDO(digitalOutput);
                }
            }
            else if (outputOption == 4)
            {
                bool valid = true;

                Console.WriteLine("Input DI tag name(id):");
                string tg = Console.ReadLine();
                Console.WriteLine("Input DI description:");
                string des = Console.ReadLine();
                Console.WriteLine("Input DI IOAddress:");
                string io = Console.ReadLine();
                Console.WriteLine("Input DI driver:");
                string dr = Console.ReadLine();
                Console.WriteLine("Input DI scanTime:");
                int sc = Convert.ToInt32(Console.ReadLine());
                //Console.WriteLine("Input DI alarms:");
                //string al = Console.ReadLine();
                Console.WriteLine("Input DI on/offScan[T/F]:");
                string on = Console.ReadLine();

                //valid = ValidateDI();
                if (valid)
                {
                    bool b = true;
                    if (on == "F")
                        b = false;
                    DI digitalOutput = new DI()
                    {
                        tagName = tg,
                        description = des,
                        IOaddress = io,
                        driver = dr,
                        scanTime = sc,
                        onOffscan = b
                    };
                    client.printToConfigDI(digitalOutput);
                    client.inputDI(digitalOutput);
                }
            }
            Console.Clear();
        }

        static void option2()
        {
            Console.WriteLine("\t\tDelete tag:");
            Console.WriteLine("\t\t\tAnalog output:[1]");
            Console.WriteLine("\t\t\tAnalog input:[2]");
            Console.WriteLine("\t\t\tDigital output:[3]");
            Console.WriteLine("\t\t\tDigital input:[4]");
            int outputOption = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\t\tId of tag to delete:");
            string id = Console.ReadLine();
            if (outputOption == 1)
            {
                client.deleteAOTag(id);
            }
            else if (outputOption == 2)
            {
                client.deleteAITag(id);
            }
            if (outputOption == 3)
            {
                client.deleteDOTag(id);
            }
            else if (outputOption == 4)
            {
                client.deleteDITag(id);
            }
            Console.Clear();
        }

        static void option3() {
            Console.WriteLine("\t\tTurning on scaning for:");
            Console.WriteLine("\t\t\tAnalog input:[1]");
            Console.WriteLine("\t\t\tDigital input:[2]");
            int outputOption = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\t\t\tEnter the tag id");
            string tagId = Console.ReadLine();//Convert.ToInt32(Console.ReadLine());

            if (outputOption == 1)
            {
                client.turningOnScaning("AI",tagId);
            }
            else if (outputOption == 2)
            {
                client.turningOnScaning("DI", tagId);
            }
            Console.Clear();
        }

        static void option4() {
            Console.WriteLine("\t\tTurning off scaning for:");
            Console.WriteLine("\t\t\tAnalog input:[1]");
            Console.WriteLine("\t\t\tDigital input:[2]");
            int outputOption = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\t\t\tEnter the tag id");
            string tagId = Console.ReadLine();//Convert.ToInt32(Console.ReadLine());

            if (outputOption == 1)
            {
                client.turningOffScaning("AI", tagId);
            }
            else if (outputOption == 2)
            {
                client.turningOffScaning("DI", tagId);
            }
            Console.Clear();
        }

        static void option5() {
            Console.WriteLine("\t\tOutput tag values for:");
            Console.WriteLine("\t\t\tAnalog output:[1]");
            Console.WriteLine("\t\t\tDigital output:[2]");
            int outputOption5 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\t\t\tEnter the tag id");
            string tagId = Console.ReadLine();//Convert.ToInt32(Console.ReadLine());
            if (outputOption5 == 1)
            {
                changeAO(tagId);
            }
            else if (outputOption5 == 2)
            {
                changeDO(tagId);
            }
            Console.Clear();
        }

        static void option6() {
            Console.WriteLine("\t\tDisplay tag value:");
            Console.WriteLine("\t\t\tAnalog output:[1]");
            Console.WriteLine("\t\t\tAnalog input:[2]");
            Console.WriteLine("\t\t\tDigital output:[3]");
            Console.WriteLine("\t\t\tDigital input:[4]");
            int outputOption = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Input Tag Id:");
            string id = Console.ReadLine();
            string value = null;

            if (outputOption == 1)
            {
                value=client.displayTagValueAO(id);
            }
            else if (outputOption == 2)
            {
                value = client.displayTagValueAI(id);
            }
            if (outputOption == 3)
            {
                value = client.displayTagValueDO(id);
            }
            else if (outputOption == 4)
            {
                value = client.displayTagValueDI(id);
            }
            Console.Clear();
            Console.WriteLine($"value od tag[{id}] is {value}");
        }

        static void option7() {
            string namePassAd = inputUser();
            string name = namePassAd.Split(',')[0];
            string pass = namePassAd.Split(',')[1];
            string ad = namePassAd.Split(',')[2];
            bool IsAdmin = false;
            if (session != null)
                IsAdmin = session.admin;
            if (name != " " && pass != " " && ad!=" ")
            {
                if(ad=="T")
                    client.addUser(name, pass,true,IsAdmin);
                else
                    client.addUser(name, pass,false,IsAdmin);
            }
             Console.Clear();
        }

        static void option8() {
            Console.WriteLine("User Name:");
            string name = Console.ReadLine();
            Console.WriteLine("User Password:");
            string pass = Console.ReadLine();
            session = client.login(name, pass);
            Console.Clear();
        }

        static void option9()
        {
            client.logout();
            session = null;
            Console.Clear();
        }

        static void option10()
        {
            Console.WriteLine("Input alarm type[low/high]:");
            string type = Console.ReadLine();

            Console.WriteLine("Input alarm priority[1/2/3]:");
            int priority = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Input alarm value:");
            string tmp= Console.ReadLine();
            double value=double.Parse(tmp);

            Console.WriteLine("Input Tag Id:");
            string tagName = Console.ReadLine();

            client.addAlarm(type, priority, value, tagName);
            Console.Clear();
        }

        static void option11()
        {
            Console.WriteLine("Input alarm type[low/high]:");
            string type = Console.ReadLine();

            Console.WriteLine("Input alarm value:");
            string value = Console.ReadLine();

            Console.WriteLine("Input Tag Id:");
            string tagName = Console.ReadLine();
            client.deleteAlarmFromFile(tagName, type,value);
            Console.Clear();
        }

        static void Main(string[] args)
        {
            client = new DatabaseManagerClient();
            client.readConfig();


            int number = 0;
            do
            {
                meni(0);
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
                    case 7:
                        option7();
                        break;
                    case 8:
                        option8();
                        break;
                    case 9:
                        option9();
                        break;
                    case 10:
                        option10();
                        break;
                    case 11:
                        option11();
                        break;
                    default:
                        Console.Clear();
                        break;

                }
            } while (number  != 0);
        }
    }
}
