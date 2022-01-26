using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace youxiaoxing
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "版权归技术中心";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //textBox1.Text = GetMACInfo();

          
            string code = null;
            SelectQuery query = new SelectQuery("select * from Win32_ComputerSystemProduct");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (var item in searcher.Get())
                {
                    using (item) code = item["UUID"].ToString();
                }
            }
         



        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
          


            foreach (NetworkInterface adapter in nics)
            {
                
                if (adapter.Name.Contains("Virtual")|| adapter.Name.Contains("VMware") || adapter.Name.Contains("Loopback") )// 
                {
                    
                    
                    //  sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
                else
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                  
                    sMacAddress += "\r\n" + adapter.Name + "  " + adapter.GetPhysicalAddress().ToString();
                }

            }
            return sMacAddress;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
