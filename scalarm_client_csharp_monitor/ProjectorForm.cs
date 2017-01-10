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

using scalarm_client_csharp_monitor.AppLogic;

namespace scalarm_client_csharp_monitor
{
    public partial class ProjectorForm : Form
    {
        public Supervisor supervisor { set; get; } 

        public ProjectorForm()
        {
            InitializeComponent();
        }

        public ProjectorForm(InterformSupervisorPassable p)
        {
            InitializeComponent();

            supervisor = p.supervisor;
            string curDir = Directory.GetCurrentDirectory();
            var uri = new Uri(String.Format("file:///{0}/index.html", curDir));
            //file:///C:/Users/Ponyman/Documents/NetBeansProjects/NurbsProjector/index.html}
            //var nburi = new Uri("file:///C:/Users/Ponyman/Documents/NetBeansProjects/NurbsProjector/index.html");
            this.webBrowser1.Url = uri;

        }

        public void update(String text)
        {
            textBox1.Text = text;
            textBox1.Refresh();
            //SetUpProjector();
            var obj = webBrowser1.Document.InvokeScript("HelloComm");
            var ver = webBrowser1.Version.Major;
            if (obj != null)
            {
                MessageBox.Show("Ok");
            }
            else
            {
                MessageBox.Show("Not ok");
            }
        }

        public void SetUpProjector()
        {
            if (webBrowser1.Url != null)
            {
                HtmlDocument doc = webBrowser1.Document;
                HtmlElement head = doc.GetElementsByTagName("head")[0];
                HtmlElement s = doc.CreateElement("script");
                string okay = "its okay";
                s.SetAttribute("text", "function Okay() { alert('" + okay + "'); } ");
                head.AppendChild(s);
                webBrowser1.Document.InvokeScript("Okay");    
            }
            
            
        }

    }
}
