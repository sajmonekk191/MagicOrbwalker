using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MagicOrbwalker1.Essentials.API
{
    class API
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "https://127.0.0.1:2999/liveclientdata/";

        public API()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "API");
        }

        public async Task<JObject> GetActivePlayerDataAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync(baseUrl + "activeplayer");
                var jsonData = JObject.Parse(response);

                return jsonData;
            }
            catch
            {
                return null;
            }
        }

        public async Task<float> GetAttackSpeedAsync()
        {
            var data = await GetActivePlayerDataAsync();
            return data?["championStats"]["attackSpeed"].ToObject<float>() ?? -1;
        }

        public async Task<float> GetAttackRangeAsync()
        {
            var data = await GetActivePlayerDataAsync();
            return data?["championStats"]["attackRange"].ToObject<float>() ?? -1;
        }
        /*public async Task<string> GetChampionNameAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync(baseUrl + "playerlist");
                var playerList = JArray.Parse(response);

                if (playerList.Count > 0)
                {
                    return playerList[0]["championName"].ToString();
                }
                else
                {
                    Console.WriteLine("No player data found.");
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }*/
        public async Task<bool?> IsChampionOrEntityDeadAsync()
        {
            try
            {
                var response = await httpClient.GetStringAsync(baseUrl + "playerlist");
                var playerList = JArray.Parse(response);

                if (playerList.Count > 0)
                {
                    bool isDead = playerList[0]["isDead"].ToObject<bool>();
                    return isDead;
                }
                else
                {
                    Console.WriteLine("No player data found.");
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
