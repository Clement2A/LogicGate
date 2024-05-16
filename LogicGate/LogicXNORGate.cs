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
            FirstInput.SetOffset(DefaultValuesLibrary.InputDoubleTopConnectorXGateOffset);
            SecondInput.SetOffset(DefaultValuesLibrary.InputDoubleDownConnectorXGateOffset);
            Output.SetOffset(DefaultValuesLibrary.OutputConnectorNGateOffset);
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateXNORData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
        }

        public override bool OutputEquation()
        {
            return FirstInput.IsOn == SecondInput.IsOn;
        }
    }
}
