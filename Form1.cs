using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace golemme2
{
    public partial class Form1 : Form
    {
        IWebDriver _chromedriver;
        public static string sound = Directory.GetCurrentDirectory() + @"\sound.wav";
        public static string driver = Directory.GetCurrentDirectory() + @"\chromedriver.exe"; // not so smart to embed an exe
        public static bool check()
        {
            if (File.Exists(driver))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Form1()
        {
            // checks
            while (!check())
            {
                MessageBox.Show("Insert chromedriver.exe in the current directory and press Ok", "Error", MessageBoxButtons.OK);
            }
            _chromedriver = new ChromeDriver(Directory.GetCurrentDirectory());
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _chromedriver.Navigate().GoToUrl("https://it.ikariam.gameforge.com");
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Stop")
            {
                timer1.Stop();
                label1.Text = "Start Monitoring Again";
                button2.Text = "Start Monitoring";
            }
            else {
                label1.Text = "Waiting...";
                timer1.Start();
                button2.Text = "Stop";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string src = _chromedriver.PageSource;
            if (src.Contains("normalalert"))
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(sound);  // play spooky song
                player.Play();
                timer1.Stop();
                DialogResult dialog = MessageBox.Show("U being attack'd m8", "U being attack'd m8", MessageBoxButtons.OK);
                if (dialog == DialogResult.OK)
                {
                    webBrowser1.Stop();
                    player.Stop();
                }
                label1.Text = "Start Monitoring Again";
                button2.Text = "Start Monitoring";
            }

        }
        // save embedded to disk
        public static void SaveToDisk(string resourceName, string fileName)  
        {
            Assembly assy = Assembly.GetExecutingAssembly();
            foreach (string resource in assy.GetManifestResourceNames())
            {
                if (resource.ToLower().IndexOf(resourceName.ToLower()) != -1)
                {
                    using (System.IO.Stream resourceStream = assy.GetManifestResourceStream(resource))
                    {
                        if (resourceStream != null)
                        {
                            using (BinaryReader reader = new BinaryReader(resourceStream))
                            {
                                // Read the bytes from the input stream.
                                byte[] buffer = reader.ReadBytes(Convert.ToInt32(resourceStream.Length));
                                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                                {
                                    using (BinaryWriter writer = new BinaryWriter(outputStream))
                                    {
                                        // Write the bytes to the output stream.
                                        writer.Write(buffer);
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(sound))
            {
                SaveToDisk("sound.wav", sound);
            }
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e) { 
            if (File.Exists(sound))
            {
                File.Delete(sound);
            }
        }
    }
}
