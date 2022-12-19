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
using System.Threading;
using RestSharp;
using RestSharp.Authenticators;
using AssetTracker.Properties;
using System.Xml.Linq;

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

            try{
                ManagementObjectSearcher searcherUsername =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherUsername.Get()){
                    var domainName = queryObj["UserName"].ToString();
                    var userName = domainName.Substring(domainName.LastIndexOf(@"\") + 1);
                    labelUsername.Text = userName;
                }

                ManagementObjectSearcher searcherModel =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherModel.Get()){
                    var model = queryObj["Model"].ToString();
                    labelModel.Text = model;
                }

                ManagementObjectSearcher searcherSN =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_BIOS");
                foreach (ManagementObject queryObj in searcherSN.Get()){
                    var serialNumber = queryObj["SerialNumber"].ToString();
                    labelSN.Text = serialNumber;
                }

                ManagementObjectSearcher searcherManufacturer =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherManufacturer.Get()){
                    var manufacturer = queryObj["Manufacturer"].ToString();
                    labelManufacturer.Text = manufacturer;
                }

                ManagementObjectSearcher searcherName =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcherName.Get()){
                    var computerName = queryObj["Name"].ToString();
                    string location = computerName.Substring(0, 3);
                    labelName.Text = computerName;
                    if (location == "COR"){
                        labelLocation.Text = "Corporate";
                    }
                    if (location == "CIN"){
                        labelLocation.Text = "Cincinnati";
                    }
                    if (location == "IRV") {
                        labelLocation.Text = "Irving";
                    }
                    if (location == "KAL"){
                        labelLocation.Text = "Kalamazoo";
                    }
                    if (location == "MAN"){
                        labelLocation.Text = "Mankato";
                    }
                    if (location == "RED"){
                        labelLocation.Text = "Redmond";
                    }
                    if (location == "SLC"){
                        labelLocation.Text = "Salt Lake City";
                    }
                    if (location == "HMF"){
                        labelLocation.Text = "HMF Express";
                    }
                }
                labelType.Enabled = true;
                comboBoxType.Enabled = true;
                labelTicket.Enabled = true;
                textBoxTicket.Enabled = true;
                buttonPost.Enabled = true;
            }
            catch (ManagementException){
                MessageBox.Show("An error occurred while querying for WMI data.");
            }
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            var createClient = new RestClient("https://shsupport.jitbit.com/helpdesk/api/Asset"){
                Authenticator = new HttpBasicAuthenticator("tstegeman@senneca.com", "Password1")
            }; 
            var createRequest = new RestRequest(Method.POST);
            createRequest.AddParameter("modelName", "aaaTest");
            createRequest.AddParameter("manufacturer", labelManufacturer.Text);
            createRequest.AddParameter("type", comboBoxType.Text);
            createRequest.AddParameter("supplier", "Purchased");
            createRequest.AddParameter("serialNumber", labelSN.Text);
            createRequest.AddParameter("location", labelLocation.Text);
            createRequest.AddParameter("comments", labelName.Text);
            createRequest.AddParameter("company", "Senneca");
            IRestResponse createResponse = createClient.Execute(createRequest); 
            var createResp = createResponse.Content;
            var createJson = Newtonsoft.Json.Linq.JObject.Parse(createResp); 
            var id = (int)createJson["id"];

            var ticketClient = new RestClient("https://shsupport.jitbit.com/helpdesk/api/AddAssetToTicket") {
                Authenticator = new HttpBasicAuthenticator("tstegeman@senneca.com", "Password1")
            };
            var ticketRequest = new RestRequest(Method.POST);
            ticketRequest.AddParameter("assetId", id);
            ticketRequest.AddParameter("ticketId", textBoxTicket.Text);
            IRestResponse ticketResponse = ticketClient.Execute(ticketRequest);

            var userClient = new RestClient("https://shsupport.jitbit.com/helpdesk/api/UserByEmail"){
                Authenticator = new HttpBasicAuthenticator("tstegeman@senneca.com", "Password1")
            };
            var userRequest = new RestRequest(Method.GET);
            var email = "";
            if (labelLocation.Text == "Salt Lake City"){
                email = labelUsername.Text + "@subzeroeng.com";
            }
            if (labelLocation.Text == "Mankato") {
                email = labelUsername.Text + "@doorengineering.com";
            }
            else{
                email = labelUsername.Text + "@senneca.com";
            }
            userRequest.AddParameter("email",  email);
            IRestResponse userResponse = userClient.Execute(userRequest);
            var userResp = userResponse.Content;
            var userJson = Newtonsoft.Json.Linq.JObject.Parse(userResp);
            var userId = (int)userJson["UserID"];

            var assignClient = new RestClient("https://shsupport.jitbit.com/helpdesk/api/AssignAssetToUser"){
                Authenticator = new HttpBasicAuthenticator("tstegeman@senneca.com", "Password1")
            };
            var assignRequest = new RestRequest(Method.POST);
            assignRequest.AddParameter("assetId", id);
            assignRequest.AddParameter("userId", userId);
            IRestResponse assignResponse = assignClient.Execute(assignRequest);
            var assignResp = assignResponse.Content;
        }
    }
}