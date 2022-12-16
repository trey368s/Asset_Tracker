using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Reflection;

namespace AssetTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                ManagementObjectSearcher searcherUsername =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherUsername.Get())
                {
                    var domainName = queryObj["UserName"].ToString();
                    var userName = domainName.Substring(domainName.LastIndexOf(@"\") + 1); 
                }
                ManagementObjectSearcher searcherModel =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcherModel.Get())
                {
                    var model = queryObj["Model"].ToString();
                }
                ManagementObjectSearcher searcherSN =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_BIOS");

                foreach (ManagementObject queryObj in searcherSN.Get())
                {
                    var serialNumber = queryObj["SerialNumber"].ToString();
                }
                ManagementObjectSearcher searcherName =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcherName.Get())
                {
                    var computerName = queryObj["Name"].ToString();
                }
                ManagementObjectSearcher searcherMan =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcherMan.Get())
                {
                    var manufacturer = queryObj["Manufacturer"].ToString();
                }

            }
            catch (ManagementException )
            {
                MessageBox.Show("An error occurred while querying for WMI data.");
            }
        }
    }
}
