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
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateANDData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
        }

        public override bool OutputEquation()
        {
            return FirstInput.IsOn && SecondInput.IsOn;
        }
    }
}
