using Bubble.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Bubble
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            if (processes.Length > 1)
            {
                foreach (Process process in processes)
                {
                    if (!process.Equals(currentProcess) && process.MainWindowHandle != IntPtr.Zero)
                    {
                        Win32Util.SetForegroundWindow(process.MainWindowHandle);
                        Current.Shutdown(0);
                        return;
                    }
                }
            }
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.StartupUri = new Uri("View/MainWindow.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
