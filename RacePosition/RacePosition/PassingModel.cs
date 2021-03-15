using Newtonsoft.Json;
using System;

namespace BigMission.RacePosition
{
    public partial class Passing
    {
        [JsonProperty("racer_session_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? RacerSessionId { get; set; }

        [JsonProperty("current_lap", NullValueHandling = NullValueHandling.Ignore)]
        public long? CurrentLap { get; set; }

        [JsonProperty("start_position_in_run", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartPositionInRun { get; set; }

        [JsonProperty("start_position_in_class")]
        public object StartPositionInClass { get; set; }

        [JsonProperty("current_flag_type", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentFlagType { get; set; }

        [JsonProperty("latest_lap_number", NullValueHandling = NullValueHandling.Ignore)]
        public long? LatestLapNumber { get; set; }

        [JsonProperty("best_overall", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BestOverall { get; set; }

        [JsonProperty("position_in_run", NullValueHandling = NullValueHandling.Ignore)]
        public long? PositionInRun { get; set; }

        [JsonProperty("position_in_class", NullValueHandling = NullValueHandling.Ignore)]
        public long? PositionInClass { get; set; }

        [JsonProperty("best_lap_number", NullValueHandling = NullValueHandling.Ignore)]
        public long? BestLapNumber { get; set; }

        [JsonProperty("best_lap_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? BestLapTime { get; set; }

        [JsonProperty("best_lap_time_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public double? BestLapTimeSeconds { get; set; }

        [JsonProperty("laps_since_pit", NullValueHandling = NullValueHandling.Ignore)]
        public long? LapsSincePit { get; set; }

        [JsonProperty("last_pit_lap", NullValueHandling = NullValueHandling.Ignore)]
        public long? LastPitLap { get; set; }

        [JsonProperty("pit_stops", NullValueHandling = NullValueHandling.Ignore)]
        public long? PitStops { get; set; }

        [JsonProperty("is_current_pit_lap", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCurrentPitLap { get; set; }

        [JsonProperty("total_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalSeconds { get; set; }

        [JsonProperty("last_lap_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? LastLapTime { get; set; }

        [JsonProperty("last_lap_time_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public double? LastLapTimeSeconds { get; set; }

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public double? Timestamp { get; set; }

        [JsonProperty("lap_position_array", NullValueHandling = NullValueHandling.Ignore)]
        public string LapPositionArray { get; set; }

        [JsonProperty("lap_timing_array", NullValueHandling = NullValueHandling.Ignore)]
        public string LapTimingArray { get; set; }
    }

}
