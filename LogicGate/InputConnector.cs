using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogicGate
{
    internal class InputConnector : LinkedConnector
    {
        public InputConnector(DesignGrid _grid, LogicElement _linkedElement, Point _offset) : base(_grid, _linkedElement, _offset)
        {

        }
    }
}
