using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class LogicANDGate : LogicGateBase
    {
        public LogicANDGate(DesignGrid _grid) : base(_grid)
        {
            InputConnectors = new InputConnector[2] 
            {
                new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleTopConnectorOffset),
                new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleDownConnectorOffset)
            };
            InputConnectors[0].OnInputChanged += UpdateVisualFromOutput;
            InputConnectors[1].OnInputChanged += UpdateVisualFromOutput;
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateANDData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
            UpdateVisualFromOutput();
        }

        public override bool OutputEquation()
        {
            return InputConnectors[0].IsOn && InputConnectors[1].IsOn;
        }
    }
}
