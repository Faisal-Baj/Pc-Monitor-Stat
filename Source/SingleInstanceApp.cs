using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace PcMonitor
{
    public class SingleInstanceApp : WindowsFormsApplicationBase
    {
        public SingleInstanceApp()
        {
            // Enable single-instance behavior
            this.IsSingleInstance = true;

            // Hook the StartupNextInstance event
            this.StartupNextInstance += OnStartupNextInstance;
        }

        protected override void OnCreateMainForm()
        {
            // Set the main form
            this.MainForm = new Form1();
        }

        private void OnStartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            // Bring the main form to the front when a second instance starts
            if (this.MainForm != null)
            {
                var form = (Form1)this.MainForm;
                form.Invoke((Action)(() =>
                {
                    form.WindowState = FormWindowState.Normal;
                    form.Show();
                    form.BringToFront();
                }));
            }
        }
    }
}
