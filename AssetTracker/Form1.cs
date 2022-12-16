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
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Win32_ComputerSystem instance");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("UserName: {0}", queryObj["UserName"]);
                    label1.Text = queryObj["UserName"].ToString();
                }
            }
            catch (ManagementException )
            {
                MessageBox.Show("An error occurred while querying for WMI data. ");
            }
        }
    }
}
