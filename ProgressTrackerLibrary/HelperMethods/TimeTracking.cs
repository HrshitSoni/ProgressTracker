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

        private DispatcherTimer timer = new DispatcherTimer();
        private IntPtr currentWindow;
        private DateTime focusStartTime;
        public Dictionary<string, TimeSpan> focustimes;

        public TimeTracking()
        {
            focustimes = new Dictionary<string, TimeSpan>();
            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Tick += Timer_Elapsed;
            timer.Start();
            focusStartTime = DateTime.Now;
        }

        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            IntPtr foregroundWindow = GetForegroundWindow();

            if (foregroundWindow != currentWindow)
            {
                if (currentWindow != IntPtr.Zero)
                {
                    string previousWindowName = GetGeneralName(currentWindow);
                    TimeSpan focusTime = DateTime.Now - focusStartTime;

                    if (!string.IsNullOrEmpty(previousWindowName))
                    {
                        if (focustimes.ContainsKey(previousWindowName))
                        {
                            focustimes[previousWindowName] += focusTime;
                        }
                        else
                        {
                            focustimes[previousWindowName] = focusTime;
                        }
                    }
                }

                currentWindow = foregroundWindow;
                focusStartTime = DateTime.Now;
            }
        }

        public void StopTracking()
        {
            timer.Stop();

            if (currentWindow != IntPtr.Zero)
            {
                string currentWindowName = GetGeneralName(currentWindow);
                TimeSpan focusTime = DateTime.Now - focusStartTime;

                if (!string.IsNullOrEmpty(currentWindowName))
                {
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
        }

        private string GetGeneralName(IntPtr window)
        {
            GetWindowThreadProcessId(window, out uint processId);
            Process process = Process.GetProcessById((int)processId);
            string path = ProcessExecutablePath(process);
            if (!string.IsNullOrEmpty(path))
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return string.Empty;
        }

        private static string ProcessExecutablePath(Process process)
        {
            try
            {
                return process.MainModule?.FileName ?? string.Empty;
            }
            catch (UnauthorizedAccessException)
            {
                // Handle specific exceptions or log them
                return string.Empty;
            }
            catch
            {
                string query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject item in searcher.Get())
                {
                    object id = item["ProcessId"];
                    object path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString() ?? string.Empty;
                    }
                }
            }
            return string.Empty;
        }
    }
}
