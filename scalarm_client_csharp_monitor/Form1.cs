﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scalarm;



namespace scalarm_client_csharp_monitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
  
            //ExperimentStatistics h;
            // model
            Supervisor supervisor = new Supervisor();
            supervisor.register();
            
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
