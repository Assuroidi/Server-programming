using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ass1
{
    public class NotFoundException : System.Exception
    {
        public NotFoundException() : base() {}
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, System.Exception inner) : base(message, inner) { }

    }

    public interface ICityBikeDataFethcer
    {
        Task<int> GetBikeCountInStation(string stationName);
    }

    public class RealTimeCityBikeDataFetcher : ICityBikeDataFethcer
    {
        HttpClient client = new HttpClient();
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            try
            {
                foreach(char c in stationName)
                {
                    if (char.IsDigit(c))
                        throw new ArgumentException("Nimi ei voi sisältää numeroita!");
                }
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                string URL = "http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";
                var stringData = await client.GetStringAsync(URL);
                var test = JsonConvert.DeserializeObject<BikeRentalStationList>(stringData);

                foreach(BikeRentalStation station in test.stations)
                {
                    // Console.WriteLine(station.name);
                    if (station.name == stationName)
                        return station.bikesAvailable;
                }
                throw new NotFoundException();
            }
            catch(NotFoundException ex)
            {
                Console.WriteLine("Not found: " + ex.Message);
            }


            return -1;
        }

        
    }
}