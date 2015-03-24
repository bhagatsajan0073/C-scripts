using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace geocoding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var reader = new StreamReader(File.OpenRead(@"C:\Users\jungli\Desktop\HospAdd.csv"));
            List<string> listA = new List<string>();
            //List<string> listB = new List<string>();

            

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                listA.Add(values[0]);
              //  listB.Add(values[1]);
              //  MessageBox.Show(values[0]);
                var   address = values[0];

                getGeocodes(address);
                System.Threading.Thread.Sleep(12000);
     
            }

        }

        public void getGeocodes(string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));
            var csv = new StringBuilder();
            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var status = xdoc.Element("GeocodeResponse").Element("status").Value;
            if(status=="ZERO_RESULTS")
            {
                return;
            }
            else
           {

            var result = xdoc.Element("GeocodeResponse").Element("result");
            var formatted_address = result.Element("formatted_address").Value;
            var locationElement = result.Element("geometry").Element("location");
            var lat = locationElement.Element("lat").Value;
            var lng = locationElement.Element("lng").Value;

            var newline = string.Format("\"\"{0}\"\",\"{1}\",\"{2}\",\"{3}\"", address.Replace("\t","").Replace(";",""), formatted_address, lat, lng);
            csv.AppendLine(newline);
            File.AppendAllText(@"C:\Users\jungli\Desktop\HospAddress.csv", csv.ToString());
        }
            
        }
    }
}
