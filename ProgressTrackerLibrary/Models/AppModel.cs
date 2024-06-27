using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ProgressTrackerLibrary.Models
{
    public class AppModel
    {
        public int id { get; set; }
        public required string appName{ get; set; }
        public required string appLogoPath { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public required DayOfWeek dayAppUsed { get; set; }

        public TimeSpan CalculateTotalTime()
        {
            TimeSpan totalTime = endTime - startTime;

           return totalTime;
        }
    }
}
