//functions that use for seeding ranker data
using HtmlAgilityPack;
using ImdbScraperApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ImdbScraperApi
{
    public class RankerScraperService
    {
        private readonly HttpClient _httpClient;

        public RankerScraperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Actor>> ScrapeActorsAsync(string url)
        {
            var actors = new List<Actor>();
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(HttpUtility.HtmlDecode(htmlContent));

                var actorsNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@data-testid='list-item-ul']/li[contains(@class, 'listItem_main')]");

                if (actorsNodes != null)
                {
                    int rank = 1;
                    foreach (var node in actorsNodes)
                    {
                        var rankNode = node.SelectSingleNode(".//div[contains(@class, 'RankBox_main')]/div");
                        int.TryParse(rankNode?.InnerText.Trim(), out int actorRank);

                        var nameNode = node.SelectSingleNode(".//div[contains(@class, 'NodeName_nameWrapper')]//a");
                        var actorName = nameNode?.InnerText.Trim();

                        var descriptionNode = node.SelectSingleNode(".//div[contains(@class, 'richText_container')]");
                        var actorDescription = descriptionNode?.InnerText.Trim();


                        actors.Add(new Actor
                        {
                            Rank = rank++,
                            Name = actorName,
                            Description = actorDescription,
                            Source = "Ranker",
                            Type= "Actor"  //Hard-coded since this does not exist in a ranker, a list of actors only 
                        });
                    }
                }
            }

            return actors;
        }


    }

}
