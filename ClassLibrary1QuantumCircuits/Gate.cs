using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantumCircuits
{
    public enum GateTypes
    {
        XGT, // Not Gate
        HGT, // Hadamard Gate
        CXT, // Controlled Not Gate Target
        CXC, // Controlled Not Gate Control
        TOF, // Toffoli Gate Target
        TOC, // Toffoli Gate Target
        NOP, // No Operation or Identity Gate
    }

    public class Gate
    {

        public GateTypes type;
        public int targetline;

        // not used, might remove
        public List<int> control_line;
        // not used, might remove
        public int columnposition;


        public Gate(GateTypes op, int target, List<int> control, int columnpos)
        {
            type = op;
            targetline = target;

            // not used, might remove
            control_line = control;
            // not used, might remove
            columnposition = columnpos;
        }


        public override string ToString()
        {
            return type.ToString();
        }


    }
}
