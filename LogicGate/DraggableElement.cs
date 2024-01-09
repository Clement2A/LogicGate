using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal abstract class DraggableElement : INotifyPropertyChanged
    {
        protected DesignGrid grid;
        protected UIElement dragElement;
        protected Grid elementGrid;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DesignGrid Grid => grid;
        public UIElement DragShape => dragElement;
        public Grid ElementGrid => elementGrid;
        public bool CanBeDragged { get; set; }

        public event Action<Thickness> OnElementMove = delegate { };

        public DraggableElement(DesignGrid _grid)
        {
            Debug.WriteLine("DraggableElement construct");
            grid = _grid;
            elementGrid = new Grid
            {
                Background = null,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new(0, 0, 0, 0),
            };
            grid.AddElement(this);
        }

        protected void SetDragElement(UIElement _dragElement)
        {
            dragElement = _dragElement;
            dragElement.PreviewMouseLeftButtonDown += OnElementSelected;
            AddElement(dragElement);
        }

        protected void AddElement(UIElement _shape)
        {
            elementGrid.Children.Add(_shape);
        }

        protected void OnElementSelected(object _sender, MouseButtonEventArgs _e)
        {
            Point _gridPos = grid.MousePosToGridPos(_e.GetPosition(grid.StaticGrid));
            grid.SelectionOffset = new Point(elementGrid.Margin.Left - _gridPos.X, elementGrid.Margin.Top - _gridPos.Y);
            if(CanBeDragged)
                grid.OnMouseMove += OnMoveAround;
            grid.OnLeftClickUp += OnUnselect;
            Debug.WriteLine("selected");
        }

        protected void OnMoveAround(Point _position)
        {
            Point _gridPos = grid.MousePosToGridPos(_position);
            _gridPos.X += grid.SelectionOffset.X;
            _gridPos.Y += grid.SelectionOffset.Y;
            if(_gridPos.X < 0)
                _gridPos.X = 0;
            if(_gridPos.Y < 0)
                _gridPos.Y = 0;

            elementGrid.Margin = new(_gridPos.X, _gridPos.Y, 0 ,0);
            OnElementMove.Invoke(elementGrid.Margin);
            Debug.WriteLine("Move element");
            Debug.WriteLine("Position is " + _position.X + " - " + _position.Y);
            Debug.WriteLine("Offset is " + grid.SelectionOffset.X + " - " + grid.SelectionOffset.Y);
            Debug.WriteLine("Grid is " + _gridPos.X + " - " + _gridPos.Y);
        }

        protected void OnUnselect(Point _position)
        {
            grid.OnMouseMove -= OnMoveAround;
            grid.OnLeftClickUp -= OnUnselect;
            Debug.WriteLine("Unselect");
        }
    }
}
