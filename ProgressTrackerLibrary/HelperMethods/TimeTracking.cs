using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Net.Http;

namespace ProgressTrackerLibrary.HelperMethods
{
    public class TimeTracking
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public System.Timers.Timer timer;
        public IntPtr currentWindow;
        public DateTime focusStartTime;
        public Dictionary<string, TimeSpan> focustimes;

        public TimeTracking()
        {
            focustimes = new Dictionary<string, TimeSpan>();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            focusStartTime = DateTime.Now;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            string foregroundWindowTitle = GetWindowTitle(foregroundWindow);

            if (foregroundWindow != currentWindow)
            {
                if(currentWindow != IntPtr.Zero)
                {
                    string previousWindowTitle = GetWindowTitle(currentWindow);
                    TimeSpan focusTime = DateTime.Now - focusStartTime;

                    if (!string.IsNullOrEmpty(previousWindowTitle))
                    {
                        string generalName = GetGeneralName(previousWindowTitle);

                        if (focustimes.ContainsKey(generalName))
                        {
                            
                            focustimes[generalName] += focusTime;
                        }
                        else
                        {
                            focustimes[generalName] = focusTime;
                        }
                    }
                }

                currentWindow = foregroundWindow;
                focusStartTime = DateTime.Now;
            }
            else
            {
                if (currentWindow != IntPtr.Zero)
                {
                    string generalName = GetGeneralName(GetWindowTitle(currentWindow));
                    if (focustimes.ContainsKey(generalName))
                    {
                        focustimes[generalName] += TimeSpan.FromSeconds(1);
                    }
                    else
                    {
                        focustimes[generalName] = TimeSpan.FromSeconds(1);
                    }
                }
            }
        }

        public string GetWindowTitle(IntPtr window)
        {
            StringBuilder title = new StringBuilder(10000);
            if (GetWindowText(window,title,10000)>0)
            {
                return title.ToString();
            }
            return "";
        }

        public void StopTracking()
        {
            timer.Stop();
        }

        public string GetGeneralName(string windowTitle)
        {
            if(windowTitle.Contains("Microsoft Visual Studio") == true)
            {
                return "devenv";
            }
            if(windowTitle.Contains("Brave"))
            {
                return "brave";
            }
            if(windowTitle.Contains("Google Chrome"))
            {
                return "chrome";
            }
            if (windowTitle.Contains("Visual Studio Code"))
            {
                return "Code";
            }
            return "";
        }
    }
}
