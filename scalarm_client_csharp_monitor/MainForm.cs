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
        // supervises config reading and setting up the xperiment
        Supervisor supervisor;


        public MainForm()
        {
            InitializeComponent();
            supervisor = new Supervisor();
            supervisor.createClient();

            supervisor.FetchingExperimentsEvent += FetchingExperimentsEvent;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            supervisor.readConfig();
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

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void fetchButton_Click(object sender, EventArgs e)
        {
            supervisor.getExperimentsFromServer();
        }

        private void credentialsButton_Click(object sender, EventArgs e)
        {
            supervisor.createClient();
        }

        private void FetchingExperimentsEvent(object sender, FetchedExperimentsEventArgs e)
        {

            fetchedExperimentsListBox.DataSource = e.runningExperiments;
            fetchedExperimentsListBox.Refresh();
        }

        private void fetchedExperimentsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

    }
}
