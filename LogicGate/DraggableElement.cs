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
        public readonly MainWindow window;
        public readonly Ellipse ellipse;
        public DraggableElement(MainWindow _window)
        {
            ellipse = new Ellipse();
            ellipse.Fill = Brushes.Red;
            ellipse.Height = 100;
            ellipse.Width = 100;
            window = _window;
            ellipse.PreviewMouseLeftButtonDown += OnElementSelected;
            //_window.CanvasMain.Children.Add(ellipse);
            Canvas.SetTop(ellipse, 0);
            Canvas.SetLeft(ellipse, 0);
        }

        private void OnElementSelected(object _sender, MouseButtonEventArgs _e)
        {
            //window.currentObjectOffset = _e.GetPosition(window.CanvasMain);
            window.currentObjectOffset.Y -= Canvas.GetTop(ellipse);
            window.currentObjectOffset.X -= Canvas.GetLeft(ellipse);
            window.OnMouseMoveEvent += OnMoveAround;
            window.OnMouseUpEvent += OnUnselect;
            //window.CanvasMain.CaptureMouse();
        }

        private void OnMoveAround(Point _position)
        {
            Point _pos = _position;
            Canvas.SetTop(ellipse, _pos.Y - window.currentObjectOffset.Y);
            Canvas.SetLeft(ellipse, _pos.X - window.currentObjectOffset.X);
        }

        private void OnUnselect()
        {
            //window.CanvasMain.ReleaseMouseCapture();
            window.OnMouseMoveEvent -= OnMoveAround;
            window.OnMouseUpEvent -= OnUnselect;
        }
    }
}
