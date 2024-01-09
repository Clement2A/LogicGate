using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class Wire
    {
        Connector input;
        Connector output;
        Line wire;
        public Wire(Connector _origin) 
        {
            //Creating wire
            input = _origin;
            output = _origin;

            wire = new Line
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                X1 = input.ElementGrid.Margin.Left + input.ElementGrid.ActualWidth / 2,
                Y1 = input.ElementGrid.Margin.Top + input.ElementGrid.ActualHeight / 2,
                X2 = input.ElementGrid.Margin.Left + input.ElementGrid.ActualWidth / 2,
                Y2 = input.ElementGrid.Margin.Top + input.ElementGrid.ActualHeight / 2,
            };
            input.OnElementMove += UpdateFirstPosition;
            input.Grid.AddElement(wire);
            input.Grid.SelectionOffset = new Point(0, 0);
            input.Grid.OnMouseMove += UpdateSecondPosition;
            input.Grid.OnLeftClickUp += SetWire;
            wire.MouseRightButtonDown += DeleteWire;
        }

        void SetWire(Point _mousePos)
        {
            input.Grid.OnMouseMove -= UpdateSecondPosition;
            input.Grid.OnLeftClickUp -= SetWire;

            //HitTestResult _result = VisualTreeHelper.HitTest(input.Grid.StaticGrid, _mousePos);
            //if(_result != null)
            //{
            //    Debug.WriteLine("There is something");
            //    //Connector _connector = (Connector)(_result.VisualHit);
            //}


        }

        void UpdateFirstPosition(Thickness _pos)
        {
            wire.X1 = _pos.Left + input.ElementGrid.ActualWidth / 2;
            wire.Y1 = _pos.Top + input.ElementGrid.ActualHeight / 2;
        }

        void UpdateSecondPosition(Thickness _pos)
        {
            wire.X2 = _pos.Left;
            wire.Y2 = _pos.Top;
        }

        void UpdateSecondPosition(Point _mousePos)
        {
            Point _gridPos = input.Grid.MousePosToGridPos(_mousePos);
            wire.X2 = _gridPos.X;
            wire.Y2 = _gridPos.Y;
        }

        private void DeleteWire(object sender, MouseButtonEventArgs e)
        {
            input.OnElementMove -= UpdateFirstPosition;
            Panel _parent = (Panel)VisualTreeHelper.GetParent(wire);
            _parent.Children.Remove(wire);
        }
    }
}
