using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class LogicXORGate : LogicGateBase
    {
        public LogicXORGate(DesignGrid _grid) : base(_grid)
        {
            FirstInput.SetOffset(DefaultValuesLibrary.InputDoubleTopConnectorXGateOffset);
            SecondInput.SetOffset(DefaultValuesLibrary.InputDoubleDownConnectorXGateOffset);
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateXORData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
        }

        public override bool OutputEquation()
        {
            return FirstInput.IsOn != SecondInput.IsOn;
        }
    }
}
