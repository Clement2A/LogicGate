using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class DraggableElement
    {
        public readonly DesignGrid grid;
        public readonly Ellipse ellipse;
        public DraggableElement(DesignGrid _grid)
        {
            grid = _grid;
            Random rdm = new Random();
            ellipse = new Ellipse
            {
                Fill = new SolidColorBrush(Color.FromRgb((byte)rdm.Next(0,256), (byte)rdm.Next(0, 256), (byte)rdm.Next(0, 256))),
                Height = 150,
                Width = 150,
                Margin = new(150, 150, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            ellipse.PreviewMouseLeftButtonDown += OnElementSelected;
            //_window.CanvasMain.Children.Add(ellipse);
            Canvas.SetTop(ellipse, 0);
            Canvas.SetLeft(ellipse, 0);
            grid.AddElement(this);
        }

        private void OnElementSelected(object _sender, MouseButtonEventArgs _e)
        {
            //window.currentObjectOffset = _e.GetPosition(window.CanvasMain);
            //window.currentObjectOffset.Y -= Canvas.GetTop(ellipse);
            //window.currentObjectOffset.X -= Canvas.GetLeft(ellipse);
            grid.OnMouseMove += OnMoveAround;
            grid.OnLeftClickUp += OnUnselect;
            Debug.WriteLine("selected");
            //window.CanvasMain.CaptureMouse();
        }

        private void OnMoveAround(Point _position)
        {
            ellipse.Margin = new(_position.X, _position.Y, 0 ,0);
            Debug.WriteLine("Move element");
            Debug.WriteLine("Position is " + _position.X + " - " + _position.Y);
        }

        private void OnUnselect(Point _position)
        {
            grid.OnMouseMove -= OnMoveAround;
            grid.OnLeftClickUp -= OnUnselect;
            Debug.WriteLine("Unselect");
        }
    }
}
