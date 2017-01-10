using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scalarm_client_csharp_monitor.AppLogic
{
    public class InterformSupervisorPassable
    {
        public Supervisor supervisor { set; get; }
        public NurbsIntermediateResult nir { set; get; }

 
        public InterformSupervisorPassable(Supervisor _s)
        {
            supervisor = _s;
        }
    }
}
