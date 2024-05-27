﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LogicGate
{
    class LogicNOTGate : LogicGateBase
    {
        public LogicNOTGate(DesignGrid _grid) : base(_grid)
        {
            InputConnectors = new InputConnector[1]
            {
                new InputConnector(_grid, this, DefaultValuesLibrary.InputSingleConnectorOffset)
            };
            InputConnectors[0].OnInputChanged += UpdateVisualFromOutput;
            Output.SetOffset(DefaultValuesLibrary.OutputConnectorNGateOffset);
            gateShape = new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateNOTData,
            };

            MakeElementClickableOrDraggable(gateShape);
            AddElement(gateShape);
        }

        public override bool OutputEquation()
        {
            return !InputConnectors[0].IsOn;
        }
    }
}
