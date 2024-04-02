using Newtonsoft.Json;

namespace ImdbScraperApi.Models
{
    public class RankerApiResponse
    {
        [JsonProperty("listItems")]
        public List<RankerListItem> ListItems { get; set; }
    }

    public class RankerListItem
    {
        public int Rank { get; set; }
        public string Blather { get; set; }
        public RankerNode Node { get; set; }
    }

    public class RankerNode
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

}
