using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RacePosition
{
    public partial class Event
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("current_lap", NullValueHandling = NullValueHandling.Ignore)]
        public long? CurrentLap { get; set; }

        [JsonProperty("total_laps", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalLaps { get; set; }

        [JsonProperty("total_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TotalTime { get; set; }

        [JsonProperty("ended_at", NullValueHandling = NullValueHandling.Ignore)]
        public string EndedAt { get; set; }

        [JsonProperty("latest_flag", NullValueHandling = NullValueHandling.Ignore)]
        public LatestFlag LatestFlag { get; set; }

        [JsonProperty("passings", NullValueHandling = NullValueHandling.Ignore)]
        public Passing[] Passings { get; set; }

        [JsonProperty("racer_sessions", NullValueHandling = NullValueHandling.Ignore)]
        public RacerSession[] RacerSessions { get; set; }
    }

    public partial class LatestFlag
    {
        [JsonProperty("flag_type", NullValueHandling = NullValueHandling.Ignore)]
        public string FlagType { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
    }

    public partial class Event
    {
        public static Event FromJson(string json) => JsonConvert.DeserializeObject<Event>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Event self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
