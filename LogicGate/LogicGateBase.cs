using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LogicGate
{
    internal abstract class LogicGateBase : LogicElement, IOutput
    {
        protected Shape gateShape;
        public event Action<bool, Connector?, Connector?> OnOutputChange = delegate { };

        //public InputConnector FirstInput { get; }
        //public InputConnector SecondInput { get; }
        public InputConnector[] InputConnectors { get; protected set; }
        public OutputConnector Output {get;}

        public bool OutputResult { get; set; } = false;

        public LogicGateBase(DesignGrid _grid) : base(_grid)
        {
            Output = new OutputConnector(_grid, this, DefaultValuesLibrary.OutputConnectorOffset);
            //FirstInput = new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleTopConnectorOffset);
            //SecondInput = new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleDownConnectorOffset);
            //FirstInput.OnInputChanged += UpdateVisualFromOutput;
            //SecondInput.OnInputChanged += UpdateVisualFromOutput;
        }

        public abstract bool OutputEquation();

        public virtual void UpdateVisualFromOutput(bool _output, Connector? _prevSource, Connector? _origin)
        {
            OutputResult = OutputEquation();
            OnOutputChange.Invoke(OutputResult, null, Output);

            gateShape.Fill = OutputResult ? DefaultValuesLibrary.LogicOnColor : DefaultValuesLibrary.LogicOffColor;
        }
    }
}
