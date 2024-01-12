using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicGate
{
    internal class ConnectorLoopPrevention
    {
        public static event Action onStop = delegate { };

        public static void StartLoopPrevention(Connector _origin)
        {
            GetLinkedConnectors(_origin);
        }

        public static void StopLoopPrevention()
        { 
            onStop.Invoke();
            onStop = delegate { };
        }

        static void GetLinkedConnectors(Connector _origin)
        {
            foreach (Connector _linked in _origin.Connectors)
            {
                if (_linked.InCircuit)
                    continue;
                _linked.InCircuit = true;
                onStop += _linked.ResetInCircuit;
                GetLinkedConnectors(_linked);
            }
        }
    }
}
