using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scalarm;

using AppLogic;



namespace scalarm_client_csharp_monitor
{
    public partial class MainForm : Form
    {
        Supervisor supervisor;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // model
            supervisor = new Supervisor();
            supervisor.register();
            
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void newFormBox_Click(object sender, EventArgs e)
        {
            MonitorForm frm = new MonitorForm();
            frm.Show();
        }

    }
}
