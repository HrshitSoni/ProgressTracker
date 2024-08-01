using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System;
using Microsoft.VisualBasic;
using System.Diagnostics;

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
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            string foregroundWindowTitle = GetWindowTitle(foregroundWindow);

            if(foregroundWindow != currentWindow)
            {
                if(currentWindow != IntPtr.Zero)
                {
                    TimeSpan focusTime = DateTime.Now - focusStartTime;
                    string previousWindowTitle = GetWindowTitle(currentWindow);

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
        }

        public string GetWindowTitle(IntPtr window)
        {
            StringBuilder title = new StringBuilder(256);
            if (GetWindowText(window,title,256)>0)
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
            return "devenv";
        }
    }
}
