using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeUnit
{
    class Program
    {

        static ServiceReference5.RealTimeUnitClient proxy = new ServiceReference5.RealTimeUnitClient();
        static CspParameters csp;
        static RSACryptoServiceProvider rsa;

        const string KEY_DIR = @"C:\Users\Dusan\source\repos\SnusProjekat\DatabaseManager\bin\Debug\";


        static Random rnd = new Random();

        static void generateKeys()
        {
            rsa = new RSACryptoServiceProvider(csp);
        }

        static byte[] signMessage(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = null;

            using (SHA256 sha = SHA256Managed.Create())
                hash = sha.ComputeHash(messageBytes);

            var formatter = new RSAPKCS1SignatureFormatter(rsa);
            formatter.SetHashAlgorithm("SHA256");
            return formatter.CreateSignature(hash);
        }

        static string exportKey()
        {
            if (!Directory.Exists(KEY_DIR))
                Directory.CreateDirectory(KEY_DIR);

            string fullPath = Path.Combine(KEY_DIR, "keys.txt");

            using (StreamWriter sout = new StreamWriter(fullPath))
                sout.Write(rsa.ToXmlString(false));

            return fullPath;
        }

        static void doStuff(int millis)
        {
            while (true)
            {
                int val = rnd.Next(0, 10);
                string message = $"{val}";
                byte[] hash = signMessage(message);
                proxy.Write(message, hash);
                Thread.Sleep(millis);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Id:");
            int id = Int32.Parse(Console.ReadLine());
            Console.WriteLine("High limit [0-9]:");
            double highLimit = Double.Parse(Console.ReadLine());
            Console.WriteLine("Low limit [0-9]:");
            double lowLimit = Double.Parse(Console.ReadLine());
            Console.WriteLine("Address [1-3]:");
            int address = Int32.Parse(Console.ReadLine());

            generateKeys();
            proxy.PubInit(exportKey(),id,lowLimit,highLimit,address);
            Thread t1 = new Thread(() => doStuff(1000));
            t1.Start();
        }
    }
}
