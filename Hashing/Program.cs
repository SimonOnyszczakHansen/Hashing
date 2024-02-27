using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hashing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vælg en algoritme:\n1: SHA256\n2: HMACSHA256");
            var choice = Console.ReadLine();

            if (choice != "1" || choice != "2")
            {
                Console.WriteLine("Ugyldigt input");
                Thread.Sleep(2000);
                return;
            }

            Console.WriteLine("Indtast en besked:");
            var message = Console.ReadLine();

            byte[] key = Encoding.UTF8.GetBytes("SuperSecretKey");
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            Stopwatch stopwatch = new Stopwatch();

            if (choice == "1")
            {
                byte[] hashValue;
                stopwatch.Start();
                using (SHA256 sha256 = SHA256.Create())
                {
                    using (MemoryStream ms = new MemoryStream(messageBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, sha256, CryptoStreamMode.Read))
                        {
                            cs.CopyTo(Stream.Null);
                        }
                    }
                    hashValue = sha256.Hash;
                    stopwatch.Stop();
                }
                Console.WriteLine($"SHA256 Hash: {BitConverter.ToString(hashValue)}");
                Console.WriteLine($"Tid brugt: {stopwatch.ElapsedMilliseconds} ms");
            }
            else if (choice == "2") 
            {
                byte[] hmacValue;
                stopwatch.Start();
                using (HMACSHA256 hmac = new HMACSHA256(key)) 
                {
                    using (MemoryStream ms = new MemoryStream(messageBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, hmac, CryptoStreamMode.Read))
                        {
                            cs.CopyTo(Stream.Null);
                        }
                    }
                    hmacValue = hmac.Hash;
                    stopwatch.Stop();
                }
                Console.WriteLine($"HMACSHA256: {BitConverter.ToString(hmacValue)}");
                Console.WriteLine($"Tid brugt: {stopwatch.ElapsedMilliseconds} ms");
            }
            Console.WriteLine($"Original besked: {message}");
            Console.ReadLine();
        }
    }
}
