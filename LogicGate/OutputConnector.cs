using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogicGate
{
    internal class OutputConnector : LinkedConnector
    {
        public OutputConnector(DesignGrid _grid, LogicElement _linkedElement, Point _offset) : base(_grid, _linkedElement, _offset)
        {
            IOutput? _linkedElementOutput = _linkedElement as IOutput;
            if (_linkedElementOutput == null)
                return;
            _linkedElementOutput.OnOutputChange += ChangeInputState;
        }

        public override List<Connector> Connectors 
        {
            get
            {
                Debug.WriteLine("Get connectors from output");
                LogicGateBase? _gate = linkedElement as LogicGateBase;
                if( _gate == null )
                    return base.Connectors;
                return _gate.InputConnectors;
            }
        }

        //public override void SetInput(LogicElement _input, Connector _connector)
        //{
        //    
        //}
    }
}
