using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogicGate
{
    internal class LinkedConnector : Connector
    {
        Point offset;
        LogicElement linkedElement;
        public LinkedConnector(DesignGrid _grid, LogicElement _linkedElement, Point _offset) : base(_grid)
        {
            linkedElement = _linkedElement;
            _linkedElement.OnElementMove += UpdatePosition;
            SetOffset(_offset);
        }

        void UpdatePosition(Thickness _pos)
        {
            SetPosition(new System.Windows.Point(_pos.Left + offset.X, _pos.Top + offset.Y));
        }

        public void SetOffset(Point _offset)
        {
            offset = _offset;
            Point _parentPos = linkedElement.GetPosition();
            SetPosition(new Point(_parentPos.X + offset.X, _parentPos.Y + offset.Y));
        }
    }
}
