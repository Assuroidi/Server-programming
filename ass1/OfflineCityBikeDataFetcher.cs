using System;
using System.Threading.Tasks;

namespace ass1
{
    public class OfflineCityBikeDataFetcher : ICityBikeDataFethcer
    {
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            int conversion = -1;
            try
            {
                // string filepath = "${workspaceFolder}/ass1/bikedata.txt";
                string filepath = "bikedata.txt";
                var file = await System.IO.File.ReadAllLinesAsync(filepath);
                
                foreach(var line in file)
                {
                    string[] split = line.Split(" : ");
                    if (split[0] == stationName)
                    {
                        if (Int32.TryParse(split[1], out conversion))
                            break;
                        else
                            throw new ArithmeticException("Error converting integer from bikedata.txt");

                    }
                }
                if (conversion == -1)
                    throw new NotFoundException();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }

            return conversion;

        }
    }
}