using Xamarin.Forms;

namespace BigMission.RacePosition
{
    public class RacerViewModel
    {
        public string CarNumber { get; set; }
        public string Class { get; set; }
        private string team;
        public string Team 
        {
            get 
            { 
                if (!string.IsNullOrEmpty(team) && team.Length > 16)
                {
                    return team.Substring(0, 14) + "..";
                }
                return team;
            }
            set { team = value; }
        }
        public string PositionInClass { get; set; }
        public string PositionOverall { get; set; }
        public string LastLap { get; set; }
        public string GainLoss { get; set; }
        public string Gap { get; set; }

        public bool GainedTime { get; set; }
        public bool IsDriversCar { get; set; }
        public bool IsInClass { get; set; }

        public Color Background
        {
            get
            {
                if (IsDriversCar)
                {
                    return Color.DimGray;
                }

                if (!IsInClass)
                {
                    return Color.LightGray;
                }

                if (GainedTime)
                {
                    return Color.LightGreen;
                }
                else
                {
                    return Color.Red;
                }
            }
        }

        public Color ForegroundColor
        {
            get
            {
                if (GainedTime)
                {
                    return Color.Black;
                }
                else
                {
                    return Color.White;
                }
            }
        }
    }
}
