
namespace Gorman.API.Framework.Domain {
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class Activity {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("map_id")]
        public long MapId { get; set; }

        public Collection<Action> Actions { get; set; }
    }
}