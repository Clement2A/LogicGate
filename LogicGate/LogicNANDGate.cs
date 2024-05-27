using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class LogicNANDGate : LogicGateBase
    {
        public LogicNANDGate(DesignGrid _grid) : base(_grid)
        {
            InputConnectors = new InputConnector[2]
            {
                new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleTopConnectorOffset),
                new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleDownConnectorOffset)
            };
            InputConnectors[0].OnInputChanged += UpdateVisualFromOutput;
            InputConnectors[1].OnInputChanged += UpdateVisualFromOutput;
            Output.SetOffset(DefaultValuesLibrary.OutputConnectorNGateOffset);
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateNANDData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
        }

        public override bool OutputEquation()
        {
            return !(InputConnectors[0].IsOn && InputConnectors[1].IsOn);
        }
    }
}
