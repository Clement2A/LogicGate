using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class LogicXNORGate : LogicGateBase
    {
        public LogicXNORGate(DesignGrid _grid) : base(_grid)
        {
            InputConnectors = new InputConnector[2]
            {
                new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleTopConnectorXGateOffset),
                new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleDownConnectorXGateOffset)
            };
            InputConnectors[0].OnInputChanged += UpdateVisualFromOutput;
            InputConnectors[1].OnInputChanged += UpdateVisualFromOutput;
            Output.SetOffset(DefaultValuesLibrary.OutputConnectorNGateOffset);
            SetShape(new Path
                {
                    Stroke = DefaultValuesLibrary.LogicStrokeColor,
                    Fill = DefaultValuesLibrary.LogicOffColor,
                    StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                    Data = DefaultValuesLibrary.GateXNORData,
                }
            );
            UpdateVisualFromOutput();
        }

        public override bool OutputEquation()
        {
            return InputConnectors[0].IsOn == InputConnectors[1].IsOn;
        }
    }
}
