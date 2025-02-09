using System;
using System.Windows.Forms;

namespace PcMonitor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the single-instance application
            SingleInstanceApp app = new SingleInstanceApp();
            app.Run(Environment.GetCommandLineArgs());
        }
    }
}
