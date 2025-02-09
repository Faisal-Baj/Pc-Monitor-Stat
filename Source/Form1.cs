using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;
using System.IO.Ports;
using System.Management;
using RJCodeAdvance.RJControls;
using System.Diagnostics;

namespace PcMonitor
{
    public partial class Form1 : Form
    {
        // Global variables
        private SerialPort serialPort;
        private bool isCommunicationActive = false;
        private Computer computer;
        private Timer timer1;

        public Form1()
        {
            InitializeComponent();
            InitializeHardwareMonitor();
            InitializeTimer();
            LoadAvailablePorts();
            PopulateTreeView();
            lblConnectionStatus.Text = "Disconnected";
            lblConnectionStatus.ForeColor = Color.Red;
        }

        public void InitializeHardwareMonitor()
        {
            // Initialize LibreHM 
            computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
            };
            computer.Open();
        }
        private void InitializeTimer()
        {
            timer1 = new Timer
            {
                Interval = 500,
                Enabled = false,
            };

            // Link timer to the Tick event
            timer1.Tick += timer1_Tick;
        }

        private void rjToggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (btnToggleCommunication.Checked)
            {
                // Toggle ON: Start communication
                StartCommunication();
                MessageBox.Show("Communication Started!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Toggle OFF: Stop communication
                StopCommunication();
                MessageBox.Show("Communication Stopped!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void StartCommunication()
        {
            try
            {
                // Check if a port is selected
                if (cmbPorts.SelectedItem == null)
                {
                    MessageBox.Show("Please select a COM port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Use the selected COM port
                string selectedPort = cmbPorts.SelectedItem.ToString();
                serialPort = new SerialPort(selectedPort, 9600);
                serialPort.Open();

                timer1.Enabled = true;
                isCommunicationActive = true;

                // Update connection status
                lblConnectionStatus.Text = "Connected";
                lblConnectionStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopCommunication()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                // Close the serial port
                serialPort.Close();
            }

            timer1.Enabled = false;
            isCommunicationActive = false;

            // Update connection status
            lblConnectionStatus.Text = "Disconnected";
            lblConnectionStatus.ForeColor = Color.Red;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //check communication
            if (isCommunicationActive && serialPort != null && serialPort.IsOpen)
            {
                // Sends data to Arduino
                string dataToSend = CollectFormattedData(computer);
                serialPort.WriteLine(dataToSend);

                // Debugging: Check what is being sent
               /* Console.WriteLine(dataToSend);  
                Debug.WriteLine($"Data sent: {dataToSend}"); */
            }
        }

        private string CollectFormattedData(Computer computer)
        {
            float cpuTemp = 0, cpuLoad = 0, gpuTemp = 0, gpuLoad = 0, ramUsage = 0, cpuFan = 0, gpuFan = 0;

            foreach (IHardware hardware in computer.Hardware)
            {
                hardware.Update();
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    cpuTemp = GetSensorValue(hardware, SensorType.Temperature, "Average");
                    cpuLoad = GetSensorValue(hardware, SensorType.Load, "Total");
                    cpuFan = GetSensorValue(hardware, SensorType.Fan, "CPU");
                }
                else if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                {
                    gpuTemp = GetSensorValue(hardware, SensorType.Temperature, "Core");
                    gpuLoad = GetSensorValue(hardware, SensorType.Load, "GPU Video Engine");
                    gpuFan = GetSensorValue(hardware, SensorType.Fan, "GPU");
                }
            }

            ramUsage = GetRamUsagePercentage();

            // Build data string based on checked nodes in TreeView
            StringBuilder dataBuilder = new StringBuilder();
            foreach (TreeNode category in treeViewStats.Nodes)
            {
                foreach (TreeNode stat in category.Nodes)
                {
                    if (stat.Checked)
                    {
                        switch (stat.Text)
                        {
                            case "Temperature":
                                if (category.Text == "CPU") dataBuilder.Append($"{cpuTemp} ");
                                if (category.Text == "GPU") dataBuilder.Append($"{gpuTemp} ");
                                break;
                            case "Load":
                                if (category.Text == "CPU") dataBuilder.Append($"{cpuLoad:F0} ");
                                if (category.Text == "GPU") dataBuilder.Append($"{gpuLoad:F0} ");
                                break;
                            case "Fan Speed":
                                if (category.Text == "CPU") dataBuilder.Append($"{cpuFan:F0} ");
                                if (category.Text == "GPU") dataBuilder.Append($"{gpuFan:F0} ");
                                break;
                            case "Usage":
                                if (category.Text == "RAM") dataBuilder.Append($"{ramUsage:F0} ");
                                break;
                        }
                    }
                }
            }

            // Return the final data string
            return dataBuilder.ToString().Trim();
        }


        private float GetSensorValue(IHardware hardware, SensorType type, string nameFilter)
        {
            // Search through sensors and return if matched
            foreach (ISensor sensor in hardware.Sensors)
            {
                if (sensor.SensorType == type && sensor.Name.Contains(nameFilter))
                {
                    return sensor.Value.GetValueOrDefault();
                }
            }
            return 0;
        }

        private float GetRamUsagePercentage()
        {
            // calculate and return RAM usages
            float totalMemory = 0;
            float availableMemory = 0;
            var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                totalMemory = float.Parse(obj["TotalVisibleMemorySize"].ToString()) / 1024;
                availableMemory = float.Parse(obj["FreePhysicalMemory"].ToString()) / 1024;
            }
            return ((totalMemory - availableMemory) / totalMemory) * 100;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCommunicationActive)
            {
                // Cancel the close event and minimize to tray
                e.Cancel = true;
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000, "PcMonitor", "The app is still running in the background.", ToolTipIcon.Info);
            }
            else
            {
                // Allow the app to close if communication is inactive
                Application.Exit();
            }
        }

        private void notifyIcon1_DoubleClick_1(object sender, EventArgs e)
        {
            // Show the form and bring it to the front
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void startStopMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle communication status
            if (!isCommunicationActive)
            {
                StartCommunication();
                startStopMenuItem.Text = "Stop Communication";
            }
            else
            {
                StopCommunication();
                startStopMenuItem.Text = "Start Communication";
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            // Close the app
            Application.Exit();
        }

        private void LoadAvailablePorts()
        {
            // Get all available COM ports
            string[] ports = SerialPort.GetPortNames();

            // Clear and repopulate the ComboBox
            cmbPorts.Items.Clear();
            cmbPorts.Items.AddRange(ports);

            // Automatically select the first port (if available)
            if (cmbPorts.Items.Count > 0)
            {
                cmbPorts.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No COM ports found. Please check your connections.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            LoadAvailablePorts();
        }

        private void PopulateTreeView()
        {
            // CPU stats
            TreeNode cpuNode = new TreeNode("CPU")
            {
                Nodes =
        {
            new TreeNode("Temperature") { Checked = true },
            new TreeNode("Load") { Checked = true },
            new TreeNode("Fan Speed"),
        }
            };

            // GPU stats
            TreeNode gpuNode = new TreeNode("GPU")
            {
                Nodes =
        {
            new TreeNode("Temperature") { Checked = true },
            new TreeNode("Load") { Checked = true },
            new TreeNode("Fan Speed"),
        }
            };

            // RAM stats
            TreeNode ramNode = new TreeNode("RAM")
            {
                Nodes =
        {
            new TreeNode("Usage") { Checked = true },
        }
            };

            // Add nodes to the TreeView
            treeViewStats.Nodes.Add(cpuNode);
            treeViewStats.Nodes.Add(gpuNode);
            treeViewStats.Nodes.Add(ramNode);

            // Expand the top-level nodes by default
            treeViewStats.ExpandAll();
        }

    }
}
