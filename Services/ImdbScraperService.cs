//functions that use for seeding imdb data
using HtmlAgilityPack;

using ImdbScraperApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImdbScraperApi.Services
{
    public class ImdbScraperService
    {
        private readonly HttpClient _httpClient;

        public ImdbScraperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Actor>> ScrapeActorsAsync(string baseUrl, int page = 1)
        {
            var actors = new List<Actor>();
            var url = $"{baseUrl}&page={page}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                var actorsNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'lister-item mode-detail')]");
                if (actorsNodes != null && actorsNodes.Any())
                {
                    var rank = 1;
                    foreach (var node in actorsNodes)
                    {
                        var nameNode = node.SelectSingleNode(".//h3/a");
                        var actorName = nameNode?.InnerText.Trim();

                        var descriptionNode = node.SelectSingleNode(".//p[not(contains(@class, 'text-muted')) and not(contains(@class, 'text-small'))]");
                        var actorDescription = descriptionNode?.InnerText.Trim();

                        var typeNode = node.SelectSingleNode(".//p[contains(@class, 'text-muted text-small')]");
                        var actorType = typeNode?.InnerText.Split('|').FirstOrDefault()?.Trim();

                        actors.Add(new Actor
                        {
                            Name = actorName,
                            Rank = rank++,
                            Description = actorDescription,
                            Source = "IMDb",
                            Type = actorType
                        }) ;
                    }
                }
                else
                {
                    return actors;
                }

                // Check if there is next page 
                var nextPageLink = htmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class, 'next-page') or contains(text(), 'Next')]");
                bool nextPageExists = nextPageLink != null;
                if (nextPageExists)
                {
                    var nextPageActors = await ScrapeActorsAsync(baseUrl, page + 1);
                    actors.AddRange(nextPageActors);
                }
            }

            return actors;
        }
    }
}
