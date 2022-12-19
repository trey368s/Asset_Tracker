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
                    labelUsername.Text = userName;  
                }

                ManagementObjectSearcher searcherModel =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherModel.Get())
                {
                    var model = queryObj["Model"].ToString();
                    labelModel.Text = model;    
                }

                ManagementObjectSearcher searcherSN =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_BIOS");
                foreach (ManagementObject queryObj in searcherSN.Get())
                {
                    var serialNumber = queryObj["SerialNumber"].ToString();
                    labelSN.Text = serialNumber;
                }

                ManagementObjectSearcher searcherManufacturer =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherManufacturer.Get())
                {
                    var manufacturer = queryObj["Manufacturer"].ToString();
                    labelManufacturer.Text = manufacturer;
                }

                ManagementObjectSearcher searcherName =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherName.Get())
                {
                    var computerName = queryObj["Name"].ToString();
                    string location = computerName.Substring(0, 3);
                    labelName.Text = computerName;
                    if (location == "COR")
                    {
                        labelLocation.Text = "Corporate";
                    }
                    if (location == "CIN")
                    {
                        labelLocation.Text = "Cincinnati";
                    }
                    if (location == "IRV")
                    {
                        labelLocation.Text = "Irving";
                    }
                    if (location == "KAL")
                    {
                        labelLocation.Text = "Kalamazoo";
                    }
                    if (location == "MAN")
                    {
                        labelLocation.Text = "Mankato";
                    }
                    if (location == "RED")
                    {
                        labelLocation.Text = "Redmond";
                    }
                    if (location == "SLC")
                    {
                        labelLocation.Text = "Salt Lake City";
                    }
                    if (location == "HMF")
                    {
                        labelLocation.Text = "HMF Express";
                    }
                }
                labelType.Enabled=true;
                comboBoxType.Enabled=true;  
                labelTicket.Enabled=true;
                textBoxTicket.Enabled=true;
                buttonPost.Enabled=true;
            }
            catch (ManagementException )
            {
                MessageBox.Show("An error occurred while querying for WMI data.");
            }
        }
    }
}
