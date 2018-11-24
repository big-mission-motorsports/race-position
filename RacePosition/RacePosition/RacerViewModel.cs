using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RacePosition
{
    public class RacerViewModel
    {
        public string CarNumber { get; set; }
        public string Class { get; set; }
        public string Team { get; set; }
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
                    return Color.Aqua;
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
    }
}
