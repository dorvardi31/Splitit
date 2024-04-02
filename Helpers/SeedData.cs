//once the app is up it load the inner db from the DS
using ImdbScraperApi.Data;
using ImdbScraperApi.Services;
using ImdbScraperApi;
namespace ImdbScraperApi.Helpers
{
    public static class SeedData
    {
        public static async Task SeedActorDataAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();

                var imdbScraperService = services.GetRequiredService<ImdbScraperService>();
                var rankerScraperService = services.GetRequiredService<RankerScraperService>();
                var rankerScraperAPIService = services.GetRequiredService<RankerScraperAPIService>();
                // Seed IMDb actors if not already present
                if (!context.Actors.Any(a => a.Source == "IMDb"))
                {
                    var imdbActors = await imdbScraperService.ScrapeActorsAsync("https://www.imdb.com/list/ls000004615/?sort=list_order,asc&mode=detail");
                    foreach (var actor in imdbActors)
                    {
                        actor.Source = "IMDb";
                    }
                    context.Actors.AddRange(imdbActors);
                }

                // Seed Ranker actors if not already present
                /*
                if (!context.Actors.Any(a => a.Source == "RANKER"))
                {
                    var rankerActors = await rankerScraperService.ScrapeActorsAsync("https://www.ranker.com/crowdranked-list/best-actors");
                    foreach (var actor in rankerActors)
                    {
                        actor.Source = "RANKER";
                    }
                    context.Actors.AddRange(rankerActors);
                }
                */
                // Seed Ranker actors from API if not already present
                var rankerApiUrl = "https://cache-api.ranker.com/lists/679173/items?limit=5000";
                if (!context.Actors.Any(a => a.Source == "RANKER"))
                {
                    var rankerApiActors = await rankerScraperAPIService.ScrapeActorsFromApiAsync(rankerApiUrl);
                    foreach (var actor in rankerApiActors)
                    {
                        actor.Source = "RANKER";
                    }
                    context.Actors.AddRange(rankerApiActors);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
