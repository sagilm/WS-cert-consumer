using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSConsumer
{
    public partial class Login : Form
    {
        public Login() => InitializeComponent();


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void populateGroupList()
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://t0ejeq7vd3.execute-api.us-east-1.amazonaws.com/Stage/getCertificates");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            String userText = textBox1.Text;
            String userPass = textBox2.Text;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"user\":\"" + userText + "\"," +
                              "\"password\":\"" + userPass + "\"}";
                Console.WriteLine(json);

                streamWriter.Write(json);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    Console.WriteLine(result.Length);
                    Console.WriteLine(result);
                    if (result.Length > 2)
                    {
                        List<x509Cert> items = JsonConvert.DeserializeObject<List<x509Cert>>(result);
                        Form2 myForm = new Form2();
        
                        foreach(x509Cert cert in items){
                            long dateini = cert.x509CertificateValidFrom;
                            DateTime dt = new DateTime(dateini);
                            long datefin = cert.x509CertificateValidTo;
                            DateTime dtf = new DateTime(datefin);
                            String subject = cert.x509CertificateSubject["CN"];
                            String prov = cert.x509CertificateIssuer["O"];
                            myForm.dataGridView.Rows.Add(new object[] { subject, prov, dt.ToString(), dtf.ToString() });
                        
                        }
                       

                        

                        myForm.Show();

                        Console.WriteLine(items.ToString());
                    }
                    else {
                        string message = "Usuario invalido, desea cancelar la operacion ?";
                        string caption = "Error Detected in Input";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result1;
                        result1 = MessageBox.Show(message, caption, buttons);
                        if (result1 == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.Close();
                        }
                        else {
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                    }
                }
            }
            catch (WebException wex)
            {
            }
            
        }
    }
}
