using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicGate
{
    internal class LoopPreventionSystem
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
            List<Connector> connectors = new List<Connector> { _origin };
            while (connectors.Count > 0)
            { 
                Connector _currentConnector = connectors[connectors.Count-1];
                connectors.RemoveAt(connectors.Count-1);
                if (_currentConnector.IsLocked)
                    continue;
                _currentConnector.SetLock();
                onStop += _currentConnector.ResetInCircuit;
                foreach (Connector connector in _currentConnector.Connectors) 
                { 
                    connectors.Add(connector);
                }
            }
        }
    }
}
