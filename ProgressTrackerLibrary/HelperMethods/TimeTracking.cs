using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Management;
using System.IO;
using System.Windows.Threading;


namespace ProgressTrackerLibrary.HelperMethods
{
    public class TimeTracking
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        // Windows api methods that are not available in .Net

        private DispatcherTimer timer = new DispatcherTimer();

        private IntPtr currentWindow;

        private DateTime focusStartTime;

        public Dictionary<string, TimeSpan> focustimes;

        public TimeTracking()
        {
            focustimes = new Dictionary<string, TimeSpan>();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Elapsed;
            timer.Start();
            focusStartTime = DateTime.Now;
        }

        // Method the calculate time of each foreground window open and put it in the map with respective .exe name
        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            IntPtr foregroundWindow = GetForegroundWindow();

            if (foregroundWindow != currentWindow)
            {
                // Calculate time for the previous window
                if (currentWindow != IntPtr.Zero)
                {
                    string previousWindowName = GetGeneralName(currentWindow);
                    TimeSpan focusTime = DateTime.Now - focusStartTime;

                    if (focustimes.ContainsKey(previousWindowName))
                    {
                        focustimes[previousWindowName] += focusTime;
                    }
                    else
                    {
                        focustimes[previousWindowName] = focusTime;
                    }
                }

                // Update to the new window
                currentWindow = foregroundWindow;
                focusStartTime = DateTime.Now;
            }
        }

        // Method to stop Tracking
        public void StopTracking()
        {
            timer.Stop();

            if (currentWindow != IntPtr.Zero)
            {
                string currentWindowName = GetGeneralName(currentWindow);
                TimeSpan focusTime = DateTime.Now - focusStartTime;

                if (focustimes.ContainsKey(currentWindowName))
                {
                    focustimes[currentWindowName] += focusTime;
                }
                else
                {
                    focustimes[currentWindowName] = focusTime;
                }
            }
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
