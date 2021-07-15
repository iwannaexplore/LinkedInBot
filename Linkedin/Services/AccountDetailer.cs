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
        public AccountDetailer(List<string> accountLinks, string rocketApiKey)
        {
            _accountLinks = accountLinks;
            this.rocketApiKey = rocketApiKey;
        }
        static object locker = new object();
        private List<string> _accountLinks;
        private string rocketApiKey;
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
                .GetAsync($"lookupProfile?api_key={rocketApiKey}&li_url=" + url).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            Profile profile = JsonConvert.DeserializeObject<Profile>(result);
            if (profile?.current_title == null)
            {
                profile.name = "Not available, get more contacts";
            }
            _profiles.Add(profile);
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