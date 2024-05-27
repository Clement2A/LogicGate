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
        Shape gateShape;
        public event Action<bool, Connector?, Connector?> OnOutputChange = delegate { };

        public List<Connector> InputConnectors { get; protected set; }
        public OutputConnector Output {get;}

        public bool OutputResult { get; set; } = false;

        public LogicGateBase(DesignGrid _grid) : base(_grid)
        {
            InputConnectors = new List<Connector>();
            Output = new OutputConnector(_grid, this, DefaultValuesLibrary.OutputConnectorOffset);
        }

        protected void SetShape(Shape _shape)
        {
            gateShape = _shape;
            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
            gateShape.MouseRightButtonDown += (s, e) => { DeleteElement(); };
        }

        public abstract bool OutputEquation();

        public virtual void UpdateVisualFromOutput(bool _output = false, Connector? _prevSource = null, Connector? _origin = null)
        {
            OutputResult = OutputEquation();
            OnOutputChange.Invoke(OutputResult, null, Output);

            gateShape.Fill = OutputResult ? DefaultValuesLibrary.LogicOnColor : DefaultValuesLibrary.LogicOffColor;
        }
    }
}
