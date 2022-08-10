using EnlightedWorkService.Data;
using EnlightedWorkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnlightedWorkService.Services
{
    public interface IFetchData
    {
        void FetchFloorData();
    }


    public class FetchData : IFetchData
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FetchData> _logger;
        private readonly IConfiguration _configuration;

        public FetchData(HttpClient httpClient, ILogger<FetchData> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async void FetchFloorData()
        {
            try
            {
                var ts = DateTime.Now.Ticks; // / 1000000
                var apiKey = _configuration.GetValue<string>("apiKey");
                var userName = _configuration.GetValue<string>("user-name");
                var authToken = GetAuthToken($"{userName}{apiKey}{ts}");
                var baseUrl = _configuration.GetValue<string>("baseUrl");

                var context = new DbOperations(_configuration.GetConnectionString("DefaultConnection"));

                //_logger.LogInformation($"ts username apikey auth  {ts} {userName} {apiKey} {authToken}");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("ApiKey",apiKey);
                _httpClient.DefaultRequestHeaders.Add("Authorization",authToken);
                _httpClient.DefaultRequestHeaders.Add("ts", ts.ToString());

                var reaponse = await _httpClient.GetAsync($"{baseUrl}/floor/list");

                if (!reaponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Request Failed with code {reaponse.StatusCode}");
                    return;
                }

                var result = await reaponse.Content.ReadAsStringAsync();
                //_logger.LogInformation("Result from floor list :: {result}",result);

                var floors = new List<Models.Floor>();
                XmlSerializer serializer = new(typeof(Floors));
                using (StringReader reader = new(result))
                {
                    var test = serializer.Deserialize(reader) as Floors;
                    floors = test?.Floor;
                    Console.WriteLine(floors?.Count.ToString());
                }
                var sFloors = new List<Data.Floor>();
                floors?.ForEach(f =>
                {
                    //_logger.LogInformation("Floor ID --> {id}", f.Id);
                    if(context.GetFloorById(f.Id) is null)
                    {
                        sFloors.Add(new Data.Floor
                        {
                            Id = f.Id,
                            Building = f.Building,
                            Campus = f.Campus,
                            Company = f.Company,
                            Description = f.Description.ToString(),
                            FloorPlanUrl = f.FloorPlanUrl,
                            Name = f.Name,
                            ParentFloorId =f.ParentFloorId
                        });
                    }
                });

                if(await context.SaveFloors(sFloors) > 0)
                {
                    foreach (var f in sFloors)
                    {
                        reaponse = await _httpClient.GetAsync($"{baseUrl}/fixture/location/list/floor/{f.Id}/fixtured");
                        if (!reaponse.IsSuccessStatusCode)
                        {
                            _logger.LogInformation($"Request Failed with code {reaponse.StatusCode}");
                            continue;
                        }

                        result = await reaponse.Content.ReadAsStringAsync();
                        //_logger.LogInformation("Result from fixture :: {result}", result);

                        var fixtures = new List<Models.Fixture>();
                        XmlSerializer serializer1 = new(typeof(Fixtures));
                        using (StringReader reader = new(result))
                        {
                            var r = serializer1.Deserialize(reader) as Fixtures;
                            fixtures = r?.Fixture;
                            //Console.WriteLine(fixtures?.Count.ToString());
                        }
                        var sFixture = new List<Data.Fixture>();
                        fixtures?.ForEach(fx =>
                        {
                            //_logger.LogInformation("Fixtue ID --> {id}", fx.Id);
                            if (context.GetFixtureById(fx.Id) is null)
                            {
                                sFixture.Add(new Data.Fixture
                                {
                                    Id = fx.Id,
                                    FloorId =f.Id,
                                    Class = fx.Class,
                                    GroupId = fx.GroupId,
                                    MacAddress = fx.MacAddress,
                                    Name = fx.Name,
                                    Xaxis = fx.Xaxis,
                                    Yaxis = fx.Yaxis
                                });
                            }
                        });

                        await context.SaveFixtures(sFixture);
                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(FetchFloorData)} :: {ex.Message}");
            }
        }


        private string GetAuthToken(string value)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(value));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }

            // return hexadecimal string
            return returnValue.ToString();
        }
    }
}
