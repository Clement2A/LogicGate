using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace LogicGate
{
    class LogicInput : LogicElement, IOutput
    {
        Ellipse inputShape;
        public OutputConnector Output { get; }
        public bool OutputResult { get; set; } = false;

        public event Action<bool, Connector?, Connector?> OnOutputChange = delegate { };

        public LogicInput(DesignGrid _grid) : base(_grid)
        {
            inputShape = new Ellipse
            {
                Fill = DefaultValuesLibrary.OffColor,
                Height = DefaultValuesLibrary.InputSize,
                Width = DefaultValuesLibrary.InputSize,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Cursor = Cursors.Hand,
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
            };
            MakeElementClickableOrDraggable(inputShape);
            AddElement(inputShape);

            Output = new OutputConnector(_grid, this, DefaultValuesLibrary.InputConnectorOffset);

            OnOutputChange += UpdateVisualFromOutput;
        }

        public bool OutputEquation()
        {
            return OutputResult;
        }

        protected override void OnAction(Point point)
        {
            OutputResult = !OutputEquation();

            OnOutputChange.Invoke(OutputResult, null, Output);
        }

        public void UpdateVisualFromOutput(bool _output, Connector? _prevSource, Connector? _origin)
        {
            inputShape.Fill = OutputResult ? DefaultValuesLibrary.OnColor : DefaultValuesLibrary.OffColor;
        }
    }
}
