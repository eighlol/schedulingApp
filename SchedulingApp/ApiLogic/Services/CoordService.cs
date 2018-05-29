using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace SchedulingApp.ApiLogic.Services
{
    public class CoordService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CoordService> _logger;

        public CoordService(IConfiguration configuration, ILogger<CoordService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CoordServiceResult> Lookup(string location)
        {
            var result = new CoordServiceResult()
            {
                Success = false,
                Message = "Undetermined failure while looking up coordinates"
            };

            // Lookup Coordinates
            string bingKey = _configuration["AppSettings:BingKey"];
            string encodedName = WebUtility.UrlEncode(location);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={bingKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                _logger.LogError("No resources were found.");
                result.Message = $"Nebija atrasta atrašanas vieta '{location}'";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High" && confidence != "Medium")
                {
                    _logger.LogError("Not confident enough resource.`");
                    result.Message = $"Nav iespējams atrast precīzu atrašanas vietu '{location}'";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }

            return result;
        }
    }
}
