﻿using System;
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
            InputConnectors.Add(new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleTopConnectorOffset));
            InputConnectors.Add(new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleDownConnectorOffset));
            InputConnectors[0].OnInputChanged += UpdateVisualFromOutput;
            InputConnectors[1].OnInputChanged += UpdateVisualFromOutput;
            SetShape(new Path
                {
                    Stroke = DefaultValuesLibrary.LogicStrokeColor,
                    Fill = DefaultValuesLibrary.LogicOffColor,
                    StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                    Data = DefaultValuesLibrary.GateORData,
                }
            );
            UpdateVisualFromOutput();
        }

        public override bool OutputEquation()
        {
            return InputConnectors[0].IsOn || InputConnectors[1].IsOn;
        }
    }
}
