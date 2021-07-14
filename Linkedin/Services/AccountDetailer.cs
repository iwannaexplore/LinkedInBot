using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Linkedin.Entities;
using Newtonsoft.Json;

namespace Linkedin.Services
{
    public class AccountDetailer
    {
        public AccountDetailer(List<string> accountLinks)
        {
            _accountLinks = accountLinks;
        }

        private List<string> _accountLinks;
        private List<Profile> _profiles = new List<Profile>();

        private void GetDetails(string url)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            HttpClient client = new HttpClient(clientHandler);

            client.BaseAddress = new Uri("https://api.rocketreach.co/v2/api/");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = client
                .GetAsync("lookupProfile?api_key=7db301k3f066d34f416aa24ecf6354b7cd2c9ad&li_url=" + url).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            Profile profile = JsonConvert.DeserializeObject<Profile>(result);
            lock (_profiles)
            {
                _profiles.Add(profile);
            }
        }

        public List<Profile> GetDetailsForLinks()
        {
            List<Task> tasks = new List<Task>();
            foreach (var accountLink in _accountLinks)
            {
                var task = new Task(() => GetDetails(accountLink));
                task.Start();
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            return _profiles;
        }
    }
}