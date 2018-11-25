using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace RacePosition
{
    class RaceHeroDataProvider
    {
        const string SITE = "http://racehero.io";

        public string CarNumber { get; set; }
        public string EventUrl { get; set; }
        private string dataPath;

        public bool IsInialized
        {
            get { return !string.IsNullOrEmpty(dataPath); }
        }

        public string LastError { get; private set; }

        public RaceHeroDataProvider(string carNumber, string eventUrl)
        {
            CarNumber = carNumber;
            EventUrl = eventUrl;
            dataPath = null;
            LastError = string.Empty;
        }


        public bool InitilaizePath()
        {
            dataPath = null;
            LastError = string.Empty;

            try
            {
                dataPath = GetDataPath();
                var exists = string.IsNullOrEmpty(dataPath);
                if (!exists)
                {
                    LastError = "Unable to locate data path in event.";
                }
                return exists;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        public RacerViewModel[] RequestUpdate()
        {
            LastError = string.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(dataPath);
                using (var response = request.GetResponse())
                {
                    using (var eventDataStream = new StreamReader(response.GetResponseStream()))
                    {
                        var eventData = eventDataStream.ReadToEnd();
                        var rhevent = Event.FromJson(eventData);

                        if (rhevent.CurrentLap == null || rhevent.CurrentLap == 0)
                        {
                            LastError = "Awaiting event to start";
                        }

                        return ProcessData(rhevent);
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }

            return null;
        }

        private RacerViewModel[] ProcessData(Event rhevent)
        {
            var rvms = new List<RacerViewModel>();

            var driverSession = rhevent.RacerSessions.FirstOrDefault(r => string.Compare(r.RacerNumber, CarNumber, true) == 0);
            if (driverSession != null)
            {
                // Get drivers in class
                var driversInClass = rhevent.RacerSessions.Where(r => r.RacerClass == driverSession.RacerClass);
                var passingsInClass = (from passings in rhevent.Passings
                                       join rs in driversInClass on passings.RacerSessionId equals rs.RacerSessionId
                                       orderby passings.PositionInRun
                                       select passings).ToArray();

                // Get overall data
                var passingsOverall = rhevent.Passings.OrderBy(p => p.PositionInRun).ToList();
                var driverOverall = passingsOverall.First(p => p.RacerSessionId == driverSession.RacerSessionId);
                var overallDriverIndex = passingsOverall.IndexOf(driverOverall);
                Passing overallDriverAhead = null;
                if (overallDriverIndex > 0)
                {
                    overallDriverAhead = passingsOverall[overallDriverIndex - 1];
                }

                for (int i = 0; i < passingsInClass.Length; i++)
                {
                    var driversCar = passingsInClass[i];
                    if (driversCar.RacerSessionId == driverSession.RacerSessionId)
                    {
                        if (driversCar.LastLapTime.HasValue)
                        {
                            Passing inClassAhead = null;
                            if (i > 0)
                            {
                                inClassAhead = passingsInClass[i - 1];
                                var rvm = FindTimeDiff(rhevent, driversCar, inClassAhead);
                                rvm.IsInClass = true;
                                rvms.Add(rvm);
                            }

                            if (overallDriverAhead != null && overallDriverAhead != inClassAhead)
                            {
                                var rvm = FindTimeDiff(rhevent, driversCar, overallDriverAhead);
                                rvm.IsInClass = false;
                                rvms.Add(rvm);
                            }

                            var driversVm = new RacerViewModel
                            {
                                CarNumber = CarNumber,
                                Class = driverSession.RacerClass,
                                PositionInClass = driversCar.PositionInClass.ToString(),
                                PositionOverall = driversCar.PositionInRun.ToString(),
                                LastLap = driversCar.LastLapTime.Value.ToString("m:ss.ff"),
                                IsDriversCar = true
                            };
                            rvms.Add(driversVm);
                            //Console.WriteLine("P{0} Overall:{1} LastLap:{2}", driversCar.PositionInClass, driversCar.PositionInRun, driversCar.LastLapTime.Value.ToString("m:ss.ff"), Math.Round(driversCar.TotalSeconds.Value, 2));

                            if ((i + 1) < passingsInClass.Length)
                            {
                                var behind = passingsInClass[i + 1];
                                var rvm = FindTimeDiff(rhevent, driversCar, behind);
                                rvm.IsInClass = true;
                                rvms.Add(rvm);
                            }
                        }
                    }
                }
            }
            else
            {
                LastError = "Unable to find specificed car number.";
            }

            return rvms.ToArray();
        }

        private static RacerViewModel FindTimeDiff(Event rhevent, Passing driversCar, Passing adjacentCar)
        {
            var rvm = new RacerViewModel();

            var adjacentRacer = rhevent.RacerSessions.FirstOrDefault(r => r.RacerSessionId == adjacentCar.RacerSessionId);
            rvm.CarNumber = adjacentRacer.RacerNumber;

            var gainLoss = adjacentCar.LastLapTimeSeconds - driversCar.LastLapTimeSeconds;
            string term = "Gain";
            rvm.GainedTime = true;
            if (gainLoss < 0)
            {
                term = "Loss";
                rvm.GainedTime = false;
            }
            rvm.GainLoss = string.Format("{0}: {1}", term, Math.Round(gainLoss.Value, 2));
            rvm.LastLap = adjacentCar.LastLapTime.Value.ToString("m:ss.ff");
            rvm.PositionInClass = adjacentCar.PositionInClass.ToString();
            rvm.PositionOverall = adjacentCar.PositionInRun.ToString();

            var rs = rhevent.RacerSessions.FirstOrDefault(r => r.RacerSessionId == adjacentCar.RacerSessionId);
            if (rs != null)
            {
                rvm.Class = rs.RacerClass;
                rvm.Team = rs.Name;
            }

            double gap = 0;
            if (driversCar.TotalSeconds.Value > adjacentCar.TotalSeconds.Value)
            {
                gap = driversCar.TotalSeconds.Value - adjacentCar.TotalSeconds.Value;
            }
            else
            {
                gap = adjacentCar.TotalSeconds.Value - driversCar.TotalSeconds.Value;
            }

            var lap = Math.Abs(driversCar.CurrentLap.Value - adjacentCar.CurrentLap.Value);
            if (lap == 0)
            {
                //Console.WriteLine("{0} {1}: {2} {3} Gap:{4}", adjacentRacer.RacerNumber, term, Math.Round(gainLoss.Value, 2), adjacentCar.LastLapTime.Value.ToString("m:ss.ff"), Math.Round(gap, 1));
                rvm.Gap = string.Format("Gap: {0}", Math.Round(gap, 1));
            }
            else
            {
                //Console.WriteLine("{0} {1}: {2} {3} Lap:{4}", adjacentRacer.RacerNumber, term, Math.Round(gainLoss.Value, 2), adjacentCar.LastLapTime.Value.ToString("m:ss.ff"), lap);
                rvm.Gap = string.Format("Lap: {0}", lap);
            }

            return rvm;
        }

        public string GetDataPath()
        {
            string htmlData = string.Empty;
            string path = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(EventUrl);
            using (var response = request.GetResponse())
            {
                using (var eventHtmlStream = new StreamReader(response.GetResponseStream()))
                {
                    htmlData = eventHtmlStream.ReadToEnd();
                    eventHtmlStream.Close();
                }
            }

            var dataPathRegex = new Regex("json_path_for_run: '(?<path>.*)'");
            var nnmatch = dataPathRegex.Match(htmlData);
            if (nnmatch.Success)
            {
                path = nnmatch.Groups["path"].Value.Trim();
                path = SITE + path;
            }

            return path;
        }
    }
}
