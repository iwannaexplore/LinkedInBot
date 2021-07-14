using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LinkedInLib
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var content = new StringContent(
                "{\n    \"query\": {\n        \"keyword\": [\n            \"`https://www.linkedin.com/search/results/people/?geoUrn=%5B%22101282230%22%5D&keywords=dpo&network=%5B%22S%22%2C%22O%22%5D&origin=FACETED_SEARCH`\"\n        ]\n    },\n    \"start\": 1,\n    \"page_size\": 1\n}",
                Encoding.UTF8, "application/json");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            HttpClient client = new HttpClient(clientHandler);

            client.BaseAddress = new Uri("https://api.rocketreach.co/v2/api/");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsync("search?api_key=7db301k3f066d34f416aa24ecf6354b7cd2c9ad", content);

            var result = response.Content.ReadAsStringAsync().Result;



            dynamic x = JsonConvert.DeserializeObject<dynamic>(result);
            List<profiles> c = JsonConvert.DeserializeObject<List<profiles>>(x.profiles.ToString());
            var z = c.Count;
            var name = x.profiles[0].name;
        }
    }

    class profiles
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
