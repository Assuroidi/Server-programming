using System;
using System.Threading.Tasks;

/// <summary>
/// The program reads the current amount of citybikes available in a given station.
/// If the first argument is "offline", a local file will be read.
/// If it is "online", the current amount will be gathered from an online API.
/// If no arguments are given or they are invalid,
/// the program will use the same API as with the argument "online."
/// </summary>

namespace ass1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool online = true;
            if (args[0] != null)
            {
                Console.WriteLine(args[0]);
                if (args[0] == "offline")
                    online = false;
                else if(args[0] == "online")
                    online = true;
            }
            else
                Console.WriteLine("No arguments given! Trying to use online sources\n");

            
            ICityBikeDataFethcer fetcher;
            if (online)
                fetcher = new RealTimeCityBikeDataFetcher();
            else
                fetcher = new OfflineCityBikeDataFetcher();

            Console.WriteLine("Minkä aseman haluat tarkistaa? ");
            string name = Console.ReadLine();  //ks. launch.json, ei toimi internalConsolella
            // Console.WriteLine("Trying with: " + name);
            var task = fetcher.GetBikeCountInStation(name);
            task.Wait();

            int result = task.Result;
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
