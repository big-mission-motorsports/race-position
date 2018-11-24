using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RacePosition
{
    public partial class RacerSession
    {
        [JsonProperty("racer_session_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? RacerSessionId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("racer_class", NullValueHandling = NullValueHandling.Ignore)]
        public string RacerClass { get; set; }

        [JsonProperty("racer_number", NullValueHandling = NullValueHandling.Ignore)]
        public string RacerNumber { get; set; }

        [JsonProperty("start_position", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartPosition { get; set; }

        [JsonProperty("racer_attributes", NullValueHandling = NullValueHandling.Ignore)]
        public RacerAttributes RacerAttributes { get; set; }
    }

    public partial class RacerAttributes
    {
        [JsonProperty("Nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }

        [JsonProperty("Wins(1st)", NullValueHandling = NullValueHandling.Ignore)]
        public string Wins1St { get; set; }

        [JsonProperty("Podium", NullValueHandling = NullValueHandling.Ignore)]
        public string Podium { get; set; }

        [JsonProperty("Poles", NullValueHandling = NullValueHandling.Ignore)]
        public string Poles { get; set; }
    }

}
