﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace LogicGate
{
    interface IOutput
    {
        Connector Output { get; }
        bool OutputResult { get; set; }

        public event Action<bool> OnOutputChange;

        bool OutputEquation();

        void UpdateVisualFromOutput(bool _output);
    }
}