﻿using System;
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
            InputConnectors.Add(new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleTopConnectorXGateOffset));
            InputConnectors.Add(new InputConnector(_grid, this, DefaultValuesLibrary.InputDoubleDownConnectorXGateOffset));
            InputConnectors[0].OnInputChanged += UpdateVisualFromOutput;
            InputConnectors[1].OnInputChanged += UpdateVisualFromOutput;
            SetShape(new Path
            {
                Stroke = DefaultValuesLibrary.LogicStrokeColor,
                Fill = DefaultValuesLibrary.LogicOffColor,
                StrokeThickness = DefaultValuesLibrary.LogicStrokeThickness,
                Data = DefaultValuesLibrary.GateXORData,
            }
            );
            UpdateVisualFromOutput();
        }

        public override bool OutputEquation()
        {
            return InputConnectors[0].IsOn != InputConnectors[1].IsOn;
        }
    }
}
