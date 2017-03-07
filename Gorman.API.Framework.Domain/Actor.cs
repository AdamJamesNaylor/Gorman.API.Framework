
namespace Gorman.API.Framework.Domain {
    using Newtonsoft.Json;

    public class Actor {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("activity_id")]
        public long ActivityId { get; set; }

        [JsonProperty("position_x")]
        public long PositionX { get; set; }

        [JsonProperty("position_y")]
        public long PositionY { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }
}