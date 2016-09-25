using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using scalarm_client_csharp_monitor.AppLogic;

namespace scalarm_client_csharp_monitor
{
    public partial class ProjectorForm : Form
    {
        public ProjectorForm()
        {
            InitializeComponent();
        }

        public ProjectorForm(InterformSupervisorPassable p)
        {
            InitializeComponent();

        }
    }
}
