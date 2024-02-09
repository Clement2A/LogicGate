using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogicGate
{
    internal class OutputConnector : Connector
    {
        Point offset;
        public OutputConnector(DesignGrid _grid, LogicElement _linkedElement, Point _offset) : base(_grid)
        {
            _linkedElement.OnElementMove += UpdatePosition;
            offset = _offset;
            Point _parentPos = _linkedElement.GetPosition();
            SetPosition(new Point(_parentPos.X + offset.X, _parentPos.Y + offset.Y));
            IOutput? _linkedElementOutput = _linkedElement as IOutput;
        }

        void UpdatePosition(Thickness _pos)
        {
            SetPosition(new System.Windows.Point(_pos.Left + offset.X, _pos.Top + offset.Y));
        }

        //public override void SetInput(LogicElement _input, Connector _connector)
        //{
        //    
        //}
    }
}
