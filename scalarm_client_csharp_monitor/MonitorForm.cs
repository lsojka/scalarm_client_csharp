using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using scalarm_client_csharp_monitor.AppLogic;

namespace scalarm_client_csharp_monitor
{
    public partial class MonitorForm : Form 
    {
        private Supervisor supervisor;
        public MonitorForm()
        {
            InitializeComponent();
        }

        public MonitorForm(InterformSupervisorPassable p)
        {
            InitializeComponent();
            supervisor = p.supervisor;
        }

        public void readFromQueue()
        {
            foreach (var x in supervisor.buffer1.GetConsumingEnumerable())
            {

                this.richTextBox1.Text += x.min_error;
                richTextBox1.Refresh();
            }
            
        }

        private void MonitorForm_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
