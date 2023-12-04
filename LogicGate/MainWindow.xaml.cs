using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogicGate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Action<Point>? OnMouseMoveEvent;
        public Action? OnMouseUpEvent;
        public Action? OnMouseRightClickDown;
        public Action? OnMouseRightClickDrag;
        public Action? OnMouseRightClickStopDrag;
        public Action? OnMouseRightClickUp;
        public Point currentObjectOffset = new(0,0);
        public MainWindow()
        {
            InitializeComponent();
            DesignGrid _designGrid = new DesignGrid();
            Grid _grid = _designGrid.StaticGrid;
            mainGrid.Children.Add(_grid);
            _grid.Margin = new Thickness(0);

            //DraggableElement _de = new(this);
            //Path _path = new Path();
            //_path.Data = Geometry.Parse("");
        }

        private void CanvasOnMouseMove(object sender, MouseEventArgs e)
        {
            //Point _position = e.GetPosition(CanvasMain);
            //if (_position.X < 0)
            //    _position.X = 0;
            //else if (_position.X > CanvasMain.ActualWidth)
            //    _position.X = CanvasMain.ActualWidth;
            //if (_position.Y < 0)
            //    _position.Y = 0;
            //else if (_position.Y > CanvasMain.ActualHeight)
            //    _position.Y = CanvasMain.ActualHeight;
            //OnMouseMoveEvent?.Invoke(_position);
        }

        private void CanvasOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUpEvent?.Invoke();
        }

        private void CanvasRightClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
