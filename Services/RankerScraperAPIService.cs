using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ImdbScraperApi.Models;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using System.Numerics;
namespace ImdbScraperApi.Services
{
    public class RankerScraperAPIService
    {
        private readonly HttpClient _httpClient;

        public RankerScraperAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Actor>> ScrapeActorsFromApiAsync(string apiUrl)
        {
            var actors = new List<Actor>();
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var rankerResponse = JsonConvert.DeserializeObject<RankerApiResponse>(content);

                if (rankerResponse?.ListItems != null)
                {
                    foreach (var listItem in rankerResponse.ListItems)
                    {
                        string description = string.IsNullOrEmpty(listItem.Blather) ? "No description available" : listItem.Blather;
                        description = StripHtmlTags(description);

                        actors.Add(new Actor
                        {
                            Rank = listItem.Rank,
                            Name = listItem.Node.Name,
                            Description = description,
                            Source = "RANKER",
                            Type = "Actor" 
                        });
                    }
                }
            }

            return actors;
        }


        public static string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

}
}
