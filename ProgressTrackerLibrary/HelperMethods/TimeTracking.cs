using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Management;
using System.IO;


namespace ProgressTrackerLibrary.HelperMethods
{
    public class TimeTracking
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        // Windows api methods that are not available in .Net

        private System.Timers.Timer timer;

        private IntPtr currentWindow;

        private DateTime focusStartTime;

        public Dictionary<string, TimeSpan> focustimes;

        public TimeTracking()
        {
            focustimes = new Dictionary<string, TimeSpan>();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            focusStartTime = DateTime.Now;
        }

        // Method the calculate time of each foreground window open and put it in the map with respective .exe name
        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            IntPtr foregroundWindow = GetForegroundWindow();

            if (foregroundWindow != currentWindow)
            {
                if(currentWindow != IntPtr.Zero)
                {
                    IntPtr previousWindow = currentWindow;

                    TimeSpan focusTime = DateTime.Now - focusStartTime;

                    if (previousWindow != IntPtr.Zero)
                    {
                        string generalName = GetGeneralName(previousWindow);

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
                    string generalName = GetGeneralName(currentWindow);
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

        // Method to stop Tracking
        public void StopTracking()
        {
            timer.Stop();
        }

        // Method to get the .exe name for the window 
        private string GetGeneralName(IntPtr Window)
        {
            GetWindowThreadProcessId(Window, out uint processId);
            Process process = Process.GetProcessById((int)processId);
            string path = ProcessExecutablePath(process);
            return Path.GetFileNameWithoutExtension(path);
        }

        // Method to get the file path of the process of the current window
        private static string ProcessExecutablePath(Process process)
        {
            try
            {
               return process.MainModule.FileName;
            }
            catch
            {
                string query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach(ManagementObject item in searcher.Get())
                {
                    object id = item["ProcessId"];
                    object path = item["ExecutablePath"];

                    if(path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString();
                    }
                }
            }
            return "";
        }
    }
}
