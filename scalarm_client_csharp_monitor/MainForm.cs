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

//using AppLogic;
using scalarm_client_csharp_monitor.AppLogic;

namespace scalarm_client_csharp_monitor
{
    public partial class MainForm : Form
    {
        // supervises config reading and setting up the xperiment
        protected Supervisor supervisor;
        protected MonitorForm monitorForm;


        public MainForm()
        {
            InitializeComponent();
            supervisor = new Supervisor();
            supervisor.createClient();

            // add handlers to events delegates
            supervisor.FetchingExperimentsEvent += FetchingExperimentsEvent;
            supervisor.IntermediateResultUpdateEvent += IntermediateResultUpdateEvent;
            
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

        private void fetchDetailsButton_Click(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated && !this.IsDisposed) return;

            // order updates
            var index = fetchedExperimentsListBox.SelectedIndex;
            var Id = fetchedExperimentsListBox.Items[index].ToString();
            supervisor.StartMonitoring(Id);
            Application.DoEvents();
            
 
            // launchPerdiodicalUpdates
            /*
            Task.Run (new Action ( delegate {
                supervisor.LaunchPeriodicalUpdates(Id);
            }));
             * 
             * 
             * 
             *
            Task.Run (new Action ( delegate {
                supervisor.LaunchPipelineMethodThatStacksNurbsIntermediateResults(Id);
            }));
             */
        }

        private void resultTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        // EVENT HANDLERS
        public void IntermediateResultUpdateEvent(object sender, EventArgs e)
        {
            InterformSupervisorPassable passable = new InterformSupervisorPassable(this.supervisor);
            // call comes from worker Supervisor thread
            // form can be chagned only by gui thread
            // MethodInvoker enables execution on gui thread
            Invoke ((MethodInvoker) delegate
            {
                this.monitorForm = new MonitorForm(passable);
                monitorForm.Show();

                monitorForm.readFromQueue();
            }) ;           
            
            
            
        }

    }
}
