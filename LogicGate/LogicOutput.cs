using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class LogicOutput : LogicElement
    {
        Ellipse outputShape;
        public InputConnector Input { get; }
        public LogicOutput(DesignGrid _grid) : base(_grid)
        {
            outputShape = new Ellipse
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
            MakeElementClickableOrDraggable(outputShape);
            AddElement(outputShape);

            outputShape.MouseRightButtonDown += (s, e) => { DeleteElement(); };

            Input = new InputConnector(_grid, this, DefaultValuesLibrary.InputSingleConnectorOffset);
            Input.OnInputChanged += UpdateVisualFromInput;
        }

        private void UpdateVisualFromInput(bool _result, Connector? _connector1, Connector? _connector2)
        {
            outputShape.Fill = _result ? DefaultValuesLibrary.OnColor : DefaultValuesLibrary.OffColor;
        }
    }
}
