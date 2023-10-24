using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimulationDriver;

namespace SnusProjekat
{

    public class DatabaseManager : IDatabaseManager,ITrending
    {
        public static ConcurrentDictionary<string, Thread> dictionaryAI = new ConcurrentDictionary<string, Thread>();
        public static ConcurrentDictionary<string, Thread> dictionaryDI = new ConcurrentDictionary<string, Thread>();
        public static Dictionary<string, double> dictionaryAO = new Dictionary<string, double>();
        public static Dictionary<string, double> dictionaryDO = new Dictionary<string, double>();
        public static ConcurrentDictionary<string, double> dictionaryAIvalues = new ConcurrentDictionary<string, double>();
        public static ConcurrentDictionary<string, double> dictionaryDIvalues = new ConcurrentDictionary<string, double>();

        public const string PATH = @"C:\Users\Dusan\source\repos\SnusProjekat\DatabaseManager\bin\Debug\scadaConfig.txt";
        public const string PATHalarm = @"C:\Users\Dusan\source\repos\SnusProjekat\DatabaseManager\bin\Debug\alarmsLog.txt";
        public const string PATHalarmtmp = @"C:\Users\Dusan\source\repos\SnusProjekat\DatabaseManager\bin\Debug\tmp.txt";

        public User session = null;
        
        public void addUser(string name, string pass,bool ad,bool isAdmin)
        {
            if (name == " " || pass == " ")
                return;

            string encryptedPassword = EncryptData(pass);
            User u = new User()
            {
                UserName = name,
                Password = encryptedPassword,
                admin=ad
            };

            using (var database = new UsersDatabase())
            {  
                if(database.UserBase.Any())
                {
                    if (!isAdmin)
                        return;
                }
                try
                {
                    database.UserBase.Add(u);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        private static string EncryptData(string valueToEncrypt)
        {
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(string strValue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(valueToEncrypt);
        }

        public User login(string name, string pass)
        {
            using (var db = new UsersDatabase())
            {
                foreach (var user in db.UserBase)
                {
                    if (name == user.UserName && ValidateEncryptedData(pass, user.Password))
                    {
                        session = user;
                        return session;
                    }
                }
            }
            session = null;
            return null;
        }

        private static bool ValidateEncryptedData(string valueToValidate,string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + valueToValidate);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        public void logout()
        {
            session = null;
        }

        public void inputDO(DO tag)
        {
            addToDODatabase(tag);
            dictionaryDO.Add(tag.tagName,double.Parse(tag.initialValue));
            addToValueDatabase(tag.tagName, double.Parse(tag.initialValue),DateTime.Now);
        }

        public void inputDI(DI tag)
        {
            addToDIDatabase(tag);
            Thread thread = new Thread(TagProcessing.startNewDITag);
            thread.Start(tag);
        }

        public void inputAI(AI tag)
        {
            addToAIDatabase(tag);
            Thread thread = new Thread(TagProcessing.startNewAITag);
            thread.Start(tag);
        }

        public void inputAO(AO tag)
        {
            addToAODatabase(tag);
            dictionaryAO.Add(tag.tagName, double.Parse(tag.initialValue));
            addToValueDatabase(tag.tagName, double.Parse(tag.initialValue), DateTime.Now);
        }      

        //public async void startNewDITag(object otag)
        //{
        //    DI tag = otag as DI;
        //    Thread.CurrentThread.Name = ($"DI-{tag.tagName}");
        //    double value = 1;
        //    dictionaryDI.TryAdd(tag.tagName, Thread.CurrentThread);
        //    dictionaryDIvalues.TryAdd(tag.tagName, value);
        //    addToValueDatabase(tag.tagName, value, DateTime.Now);
        //    while (true)
        //    {
        //        value = SimulationDriver.SimulationDriver.ReturnValue(tag.IOaddress);
                
        //        if (value < 5)//za digital manje od 0.5 ali stavio sam 5 radi testiranja
        //            value = 0;
        //        else
        //            value = 1;

        //        double oldValue = 0;
        //        dictionaryDIvalues.TryGetValue(tag.tagName, out oldValue);
        //        dictionaryDIvalues.TryUpdate(tag.tagName, value, oldValue);
        //        updateValuesDatabase(tag.tagName, value);
        //        if (isScanOnOrOff(tag.tagName))
        //            Trending.onRegularMessageReceived?.Invoke($"tag[{tag.tagName}]:value[{value}]");
        //        Thread.Sleep(tag.scanTime);
        //    }
        //}
        
        //string isOk(string id,double value)
        //{
        //    string[] arrLine = File.ReadAllLines(PATHalarmtmp);

        //    foreach (var l in arrLine)
        //    {
        //        if (l.Split(';')[3] == id)
        //        {
        //           if(l.Split(';')[0] == "low")
        //           {
        //               if(value< double.Parse(l.Split(';')[2]))
        //               {
        //                    addAlarmToBase(l.Split(';')[0], Int32.Parse(l.Split(';')[1]),double.Parse(l.Split(';')[2]), l.Split(';')[3], DateTime.Now);
        //                    return $"{l.Split(';')[1]}tag[{id}]:below lower limit[{double.Parse(l.Split(';')[2])}]:time[{DateTime.Now}]";
        //               }
        //           }
        //           else
        //           {
        //                if (value > double.Parse(l.Split(';')[2]))
        //                {
        //                    addAlarmToBase(l.Split(';')[0], Int32.Parse(l.Split(';')[1]), double.Parse(l.Split(';')[2]), l.Split(';')[3], DateTime.Now);
        //                    return $"{l.Split(';')[1]}tag[{id}]:above upper limit[{double.Parse(l.Split(';')[2])}]:time[{DateTime.Now}]";
        //                }
        //           }
        //        }
        //    }
        //    return null;
        //}

        
        //public async void startNewAITag(object otag)
        //{
        //    AI tag = otag as AI;
        //    Thread.CurrentThread.Name = ($"AI-{tag.tagName}");
        //    double value = 1;
        //    dictionaryAI.TryAdd(tag.tagName, Thread.CurrentThread);
        //    dictionaryAIvalues.TryAdd(tag.tagName, value);
        //    addToValueDatabase(tag.tagName, value, DateTime.Now);
        //    while (true)
        //    {
        //        value = SimulationDriver.SimulationDriver.ReturnValue(tag.IOaddress);

        //        if (value > tag.highLimit)
        //            value = tag.highLimit;
        //        if (value < tag.lowLimit)
        //            value = tag.lowLimit;

        //        double oldValue = 0;
        //        dictionaryAIvalues.TryGetValue(tag.tagName, out oldValue);
        //        dictionaryAIvalues.TryUpdate(tag.tagName, value, oldValue);
        //        updateValuesDatabase(tag.tagName, value);
        //        if (isScanOnOrOff(tag.tagName))
        //        {
        //            string answer = isOk(tag.tagName, value);
        //            if (answer != null)
        //            {
        //                Alarm.onAlarmMessageReceived?.Invoke(answer);
        //            }
        //            Trending.onRegularMessageReceived?.Invoke($"tag[{tag.tagName}]:value[{value}]");
        //        }
        //        Thread.Sleep(tag.scanTime);
        //    }
        //}

        public static bool isScanOnOrOff(string id)
        {
            string[] arrLine = File.ReadAllLines(PATH);

            foreach (var l in arrLine)
            {
                if (l.Split(';')[1] == id)
                {
                    if (l.Split(';')[6] == "True")
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        bool ValidateDI()
        {
            return true;
        }

        bool ValidateDO()
        {
            return true;
        }

        bool ValidateAI()
        {
            return true;
        }

        public static void updateValuesDatabase(string id,double value)
        {
            using (var db = new ValuesDatabase())
            {
                var result = db.values.SingleOrDefault(b => b.Id == id);
                if (result != null)
                {
                    result.Value = value;
                    db.SaveChanges();
                }
            }
        }

        public bool InputValuesDO(string id,string value)
        {
            dictionaryDO[id] = double.Parse(value);
            updateValuesDatabase(id, double.Parse(value));
            return true;//neka validacija
        }
        public bool InputValuesAO(string id,string value)
        {
            dictionaryAO[id] = double.Parse(value);
            updateValuesDatabase(id, double.Parse(value));
            return true;//neka validacija
        }

        public void onOffAITag(string id)
        {
            //mijenjanje unutar treda            
        }

        public void onOffDITag(string id)
        {
            //mijenjanje unutar treda
        }

        public void deleteAITag(string id)
        {
            Thread tr=null;
            dictionaryAI.TryRemove(id,out tr);
            double dou;
            dictionaryAIvalues.TryRemove(id, out dou);
            deleteFromFile("AI",id);
            deleteFromAIDatabase(id);
            deleteFromValuesDatabase(id);
        }

        public void deleteAOTag(string id)
        {
            dictionaryAO.Remove(id);
            deleteFromFile("AO", id);
            deleteFromAODatabase(id);
            deleteFromValuesDatabase(id);
        }

        public void deleteDITag(string id)
        {
            Thread tr = null;
            dictionaryAI.TryRemove(id, out tr);
            double dou;
            dictionaryDIvalues.TryRemove(id, out dou);
            deleteFromFile("DI", id);
            deleteFromDIDatabase(id);
            deleteFromValuesDatabase(id);
        }

        public void deleteDOTag(string id)
        {
            dictionaryDO.Remove(id);
            deleteFromFile("DO", id);
            deleteFromDODatabase(id);
            deleteFromValuesDatabase(id);
        }

        public string displayTagValueAI(string id)
        {
            return dictionaryAIvalues[id].ToString();
        }
        public string displayTagValueAO(string id)
        {
            return dictionaryAO[id].ToString();
        }
        public string displayTagValueDI(string id)
        {
            return dictionaryDIvalues[id].ToString();
        }
        public string displayTagValueDO(string id)
        {
            return dictionaryDO[id].ToString();
        }

        public void printToConfigAI(AI tag)
        {
            using (StreamWriter sw = File.AppendText(PATH))
            {
                sw.WriteLine("AI;" + tag.tagName + ";" + tag.description + ";" + tag.IOaddress + ";" + tag.driver + ";" + tag.scanTime  + ";" + tag.onOffscan + ";" + tag.lowLimit + ";" + tag.highLimit + ";" + tag.units);
            }
        }
        public void printToConfigAO(AO tag)
        {
            using (StreamWriter sw = File.AppendText(PATH))
            {
                sw.WriteLine("AO;" + tag.tagName + ";" + tag.description + ";" + tag.IOaddress + ";" + tag.initialValue + ";" + tag.lowLimit + ";" + tag.highLimit);
            }
        }
        public void printToConfigDI(DI tag)
        {
            using (StreamWriter sw = File.AppendText(PATH))
            {
                sw.WriteLine("DI;" + tag.tagName + ";" + tag.description + ";" + tag.IOaddress + ";" + tag.driver + ";" + tag.scanTime  + ";" + tag.onOffscan);
            }
        }
        public void printToConfigDO(DO tag)
        {
            using (StreamWriter sw = File.AppendText(PATH))
            {
                sw.WriteLine("DO;" + tag.tagName + ";" + tag.description +";" + tag.IOaddress + ";" + tag.initialValue);
            }
        }

        public void readConfig()
        {
            string[] lines = System.IO.File.ReadAllLines(PATH);
            foreach (string line in lines)
            {
                int num = 1;
                if (line.Split(';')[0] == "DI")
                    num = 1;
                else if (line.Split(';')[0] == "DO")
                    num = 2;
                else if (line.Split(';')[0] == "AI")
                    num = 3;
                else
                    num = 4;
                switch(num)
                {
                    case 1:
                        bool onoff = false;
                        if (line.Split(';')[6] == "True")
                            onoff = true;
                        DI tag1 = new DI()
                        {
                            tagName = line.Split(';')[1],
                            description = line.Split(';')[2],
                            IOaddress = line.Split(';')[3],
                            driver = line.Split(';')[4],
                            scanTime = Int32.Parse(line.Split(';')[5]),
                            onOffscan = onoff
                        };
                        inputDI(tag1);
                        
                        break;
                    case 2:                        
                        DO tag2 = new DO()
                        {
                            tagName = line.Split(';')[1],
                            description = line.Split(';')[2],
                            IOaddress = line.Split(';')[3],
                            initialValue = line.Split(';')[4]
                        };
                        inputDO(tag2);
                        break;
                    case 3:
                        bool onoff3 = false;
                        if (line.Split(';')[6] == "True")
                            onoff = true;
                        AI tag3 = new AI()
                        {
                            tagName = line.Split(';')[1],
                            description = line.Split(';')[2],
                            IOaddress = line.Split(';')[3],
                            driver = line.Split(';')[4],
                            scanTime = Int32.Parse(line.Split(';')[5]),
                            onOffscan = onoff3,
                            lowLimit = double.Parse(line.Split(';')[7]),
                            highLimit = double.Parse(line.Split(';')[8]),
                            units = line.Split(';')[9]
                        };
                        inputAI(tag3);
                        break;
                    default:
                        AO tag4 = new AO()
                        {
                            tagName = line.Split(';')[1],
                            description = line.Split(';')[2],
                            IOaddress = line.Split(';')[3],
                            initialValue = line.Split(';')[4],
                            lowLimit = double.Parse(line.Split(';')[5]),
                            highLimit = double.Parse(line.Split(';')[6])
                        };
                        inputAO(tag4);
                        break;
                }
            }
        }

        void deleteFromFile(string tag,string id)
        {
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(PATH).Where(l => l.Split(';')[0] != tag && l.Split(';')[1] != id);

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(PATH);
            File.Move(tempFile, PATH);
        }

        public void deleteAlarmFromFile(string tagId, string type, string value)
        {
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(PATHalarmtmp).Where(l => l.Split(';')[0] != type && l.Split(';')[3] != tagId && l.Split(';')[2] != value);

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(PATHalarmtmp);
            File.Move(tempFile, PATHalarmtmp);
        }

        public void turningOnOffScaning(string tagName,string tagId,string onoff)
        {
            string[] arrLine = File.ReadAllLines(PATH);
            int counter = 0;
            foreach (var l in arrLine)
            {
                if (l.Split(';')[1]==tagId)
                {
                    break;
                }
                counter++;
            }
            

            string line = arrLine[counter];
            if (tagName == "AI")
                arrLine[counter] = "AI;" + line.Split(';')[1] + ";" + line.Split(';')[2] + ";" + line.Split(';')[3] + ";" + line.Split(';')[4] + ";" + line.Split(';')[5] + ";" + onoff + ";" + line.Split(';')[7] + ";" + line.Split(';')[8] + ";" + line.Split(';')[9];
            else
                arrLine[counter] = "DI;" + line.Split(';')[1] + ";" + line.Split(';')[2] + ";" + line.Split(';')[3] + ";" + line.Split(';')[4] + ";" + line.Split(';')[5] + ";" + onoff;

            File.WriteAllLines(PATH, arrLine);
        }

        public void turningOnScaning(string tagName, string tagId)
        {
            turningOnOffScaning(tagName, tagId, "True");
        }

        public void turningOffScaning(string tagName, string tagId)
        {
            turningOnOffScaning(tagName, tagId, "False");
        }

        public void ConsoleTrendingInit()
        {
            Trending.onRegularMessageReceived += OperationContext.Current.GetCallbackChannel<ITrendingCallback>().WriteToConsole;

            Trending.onAlarmMessageReceived += OperationContext.Current.GetCallbackChannel<ITrendingCallback>().WriteToConsole;           
        }

        public void addAlarm(string type, int priority, double value, string tagName)
        {
            using (StreamWriter sw = File.AppendText(PATHalarmtmp))
            {
                sw.WriteLine($"{type};{priority};{value};{tagName}");
            }
        }


        public static void addAlarmToBase(string type, int priority, double value, string tagName,DateTime date,double tagvalue)
        {
            alarm al = new alarm()
            {
                Type = type,
                Priority = priority,
                Value = value,
                TagName = tagName,
                Date = date,
                TagValue=tagvalue
            };

            using (StreamWriter sw = File.AppendText(PATHalarm))
            {
                sw.WriteLine($"{type};{priority};{value};{tagName};{date}");
            }

            using (var database = new AlarmDatabase())
            {
                try
                {
                    database.AlarmBase.Add(al);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void addToDODatabase(DO tag)
        {
            using (var database = new DODatabase())
            {
                try
                {
                    database.DOs.Add(tag);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void addToDIDatabase(DI tag)
        {
            using (var database = new DIDatabase())
            {
                try
                {
                    database.DIs.Add(tag);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void addToAODatabase(AO tag)
        {
            using (var database = new AODatabase())
            {
                try
                {
                    database.AOs.Add(tag);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void addToAIDatabase(AI tag)
        {
            using (var database = new AIDatabase())
            {
                try
                {
                    database.AIs.Add(tag);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void deleteFromDODatabase(string tag)
        {
            using (var database = new DODatabase())
            {
                try
                {
                    DO tmp = new DO() { tagName = tag };
                    database.DOs.Attach(tmp);
                    database.DOs.Remove(tmp);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void deleteFromDIDatabase(string tag)
        {
            using (var database = new DIDatabase())
            {
                try
                {
                    DI tmp = new DI() { tagName = tag };
                    database.DIs.Attach(tmp);
                    database.DIs.Remove(tmp);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void deleteFromAODatabase(string tag)
        {
            using (var database = new AODatabase())
            {
                try
                {
                    AO tmp = new AO() { tagName = tag };
                    database.AOs.Attach(tmp);
                    database.AOs.Remove(tmp);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        void deleteFromAIDatabase(string tag)
        {
            using (var database = new AIDatabase())
            {
                try
                {
                    AI tmp = new AI() { tagName = tag };
                    database.AIs.Attach(tmp);
                    database.AIs.Remove(tmp);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        public static void addToValueDatabase(string id,double value,DateTime date)
        {
            Values tmp = new Values()
            {
                Id = id,
                Value = value,
                Date=date
            };
            using (var database = new ValuesDatabase())
            {
                try
                {
                    database.values.Add(tmp);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }
        void deleteFromValuesDatabase(string id)
        {
            using (var database = new ValuesDatabase())
            {
                try
                {
                    Values tmp = new Values() { Id = id };
                    database.values.Attach(tmp);
                    database.values.Remove(tmp);
                    database.SaveChanges();
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }
    }
}
