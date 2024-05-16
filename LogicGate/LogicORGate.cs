using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class LogicORGate : LogicGateBase
    {
        public LogicORGate(DesignGrid _grid) : base(_grid)
        {
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateORData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
        }

        public override bool OutputEquation()
        {
            return FirstInput.IsOn || SecondInput.IsOn;
        }
    }
}
