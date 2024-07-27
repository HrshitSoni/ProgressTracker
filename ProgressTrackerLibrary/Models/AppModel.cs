using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Drawing;

namespace ProgressTrackerLibrary.Models
{
    public class AppModel
    {
        public required string appName{ get; set; }
        public  string? appLogoPath { get; set; }
        public string? activeTime { get; set; }
        public string DayOfTheWeek { get; set; }

        public AppModel()
        {

        }
    } 
}
