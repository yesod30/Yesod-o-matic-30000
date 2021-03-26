using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Yesod_o_matic_30000
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string CRASH_FOLDER = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        "CrashReports");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
        }
        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            WriteException(e.Exception);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteException((Exception)e.ExceptionObject);
        }

        public static void WriteException(Exception ex)
        {
            if (!File.Exists(CRASH_FOLDER))
            {
                Directory.CreateDirectory(CRASH_FOLDER);
            }
            DateTime localDate = DateTime.Now;
            string crashFile = Path.Combine(CRASH_FOLDER, "CrashReport" + "_" + localDate.Day + "_" + localDate.Month + "_" + localDate.Hour + "_" + localDate.Minute + "_" + localDate.Second + ".log");
            File.WriteAllText(crashFile, ex.ToString());
            Console.WriteLine(ex);
        }
    }
}
