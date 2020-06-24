using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        static string path = @"../../Routes.txt";
        //list contains routes
        static List<int> Routes = new List<int>();
        //list contains threads
        static List<Thread> threadList = new List<Thread>();
        static int waitingTime = 0;
        static object lockObject = new object();

        static void Main(string[] args)
        {
            GenerateRoutes();
            Thread RouteGenerator = new Thread(() => SelectRoutes());
            RouteGenerator.Start();
            RouteGenerator.Join();
        }
        static void GenerateRoutes()
        {
            Console.WriteLine("Manager is waiting for routes to generate...");
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(1000, 3000));
            StreamWriter sw = new StreamWriter(path, false);
            //writing 1000 random numbers to file
            for (int i = 0; i < 1000; i++)
            {
                sw.WriteLine(rnd.Next(1, 5001));
            }
            sw.Close();
        }
        static void SelectRoutes()
        {
            List<int> tempList = new List<int>();
            StreamReader sr = new StreamReader(path);
            string line = "";
            //reading numbers from file
            while ((line = sr.ReadLine()) != null)
            {
                tempList.Add(Convert.ToInt32(line));
            }
            sr.Close();
            //sorting list so that 10 smallest will be first
            tempList.Sort();

            int counter = 0;
            //collecting 10 numbers (that can be divided by 3) and puting them into list
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i] % 3 == 0)
                {
                    //only distinct numbers
                    if (Routes.Contains(tempList[i]))
                    {
                        continue;
                    }
                    else
                    {
                        Routes.Add(tempList[i]);
                        counter++;
                    }
                }
                //when there are 10 numbers => break
                if (Routes.Count() == 10)
                {
                    break;
                }
            }
            //displaying routes from list
            Console.WriteLine("Best routes:");
            foreach (int item in Routes)
            {
                Console.WriteLine("Route:" + item);
            }
            Console.WriteLine("\n Routes are chosen. Trucks can head to the loading site.\n");
        }
    }
}
