using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class DesignGrid
    {
        public readonly Canvas staticCanvas = new();
        Grid moveGrid = new();
        Border outerBorder = new();
        Border innerBorder = new();

        DesignElement? hoveredElement = null;

        int padding = 10;
        int margin = 10;

        Point selectionOffset = new Point(0, 0);
        public DesignElement? HoveredElement => hoveredElement;

        public event Action<Point> OnCanvasMove = delegate { };
        public event Action<Point> OnDrag = delegate { };
        public event Action<Point> OnLeftClickUp = delegate { };
        public event Action<Point> OnRightClickUp = delegate { };
        public event Action OnLeftClickDown = delegate { };
        public event Action OnRightClickDown = delegate { };
        public event Action<Point> OnMouseMove = delegate { };

        public event Action<DesignElement?> OnElementHovered = delegate { };

        Thickness mouseOffset = new();

        public Canvas StaticGrid => staticCanvas;
        public Point SelectionOffset { get => selectionOffset; set => selectionOffset = value; }

        public DesignGrid()
        {
            InitGrids();
            InitEvents();
            //Ellipse ellipse = new Ellipse();
            //ellipse.Fill = Brushes.Orange;
            //ellipse.Height = 150;
            //ellipse.Width = 150;
            //moveGrid.Children.Add(ellipse);
            //ellipse.Margin = new(50, 0, -50, 0);

        }

        public void AddElement(DesignElement _element)
        {
            moveGrid.Children.Add(_element.ElementGrid);
        }

        void InitGrids()
        {
            innerBorder.Background = DefaultValuesLibrary.GridBackground;
            outerBorder.Background = DefaultValuesLibrary.GridBorder;
            staticCanvas.Children.Add(outerBorder);
            outerBorder.Child = innerBorder;
            innerBorder.Child = moveGrid;
            staticCanvas.SizeChanged += AdaptMoveGridSize;
            innerBorder.Margin = new(padding);
            moveGrid.Margin = new(margin);
            moveGrid.Width = double.NaN;
            moveGrid.Height = double.NaN;
            moveGrid.MinWidth = 100;
            moveGrid.MinHeight = 100;
            moveGrid.HorizontalAlignment = HorizontalAlignment.Left;
            moveGrid.VerticalAlignment = VerticalAlignment.Top;
        }

        private void AdaptMoveGridSize(object sender, SizeChangedEventArgs e)
        {
            Size _newSize = e.NewSize;
            int _borderSize = (padding + margin)*2;
            _newSize.Width -= _borderSize;
            _newSize.Height -= _borderSize;
            moveGrid.MinWidth = _newSize.Width;
            moveGrid.MinHeight = _newSize.Height;
        }

        void InitEvents()
        {
            staticCanvas.MouseRightButtonDown += (sender, args) => { OnRightClickDown.Invoke(); };
            staticCanvas.MouseRightButtonUp += (sender, args) => { OnRightClickUp.Invoke(args.GetPosition(staticCanvas)); };
            staticCanvas.MouseLeftButtonDown += (sender, args) => { OnLeftClickDown.Invoke(); };
            staticCanvas.MouseLeftButtonUp += (sender, args) => { OnLeftClickUp.Invoke(args.GetPosition(staticCanvas)); };
            staticCanvas.MouseMove += (sender, args) => { OnMouseMove.Invoke(args.GetPosition(staticCanvas)); };
            staticCanvas.MouseLeave += (sender, args) => { ResetEvents(); };

            OnRightClickDown += RightDragOrClick;
            staticCanvas.SizeChanged += (sender, args) => { };
        }

        public Point MousePosToGridPos(Point _mousePos)
        {
            Point _gridPos = new(_mousePos.X - outerBorder.Margin.Left - innerBorder.Margin.Left - moveGrid.Margin.Left, _mousePos.Y - outerBorder.Margin.Top - innerBorder.Margin.Top - moveGrid.Margin.Top);
            return _gridPos;
        }

        void RightDragOrClick()
        {
            OnRightClickUp += OpenMenu;
            OnMouseMove += StartMovingGrid;
        }

        void ResetEvents()
        {
            ResetRightDragOrClick();
        }

        void ResetRightDragOrClick()
        {
            OnRightClickUp -= OpenMenu;
            OnMouseMove -= StartMovingGrid;
        }

        void OpenMenu(Point _mousePos)
        {
            ResetRightDragOrClick();
            Debug.WriteLine("Menu not implemented yet. Cursor is at " + _mousePos.X + " - " + _mousePos.Y);
        }

        void StartMovingGrid(Point _mousePos)
        {
            double _leftOffset = _mousePos.X - outerBorder.Margin.Left;
            double _topOffset = _mousePos.Y - outerBorder.Margin.Top;
            mouseOffset = new(_leftOffset, _topOffset, -_leftOffset, -_topOffset);
            ResetRightDragOrClick();
            OnMouseMove += MoveCanvas;
            OnRightClickUp += StopMovingGrid;
        }

        void StopMovingGrid(Point _mousePos)
        {
            OnMouseMove -= MoveCanvas;
            OnRightClickUp -= StopMovingGrid;
            mouseOffset = new(0);
        }

        void MoveCanvas(Point _mousePos)
        {
            double _leftOffset = 0, _topOffset = 0;

            if (outerBorder.ActualWidth <= staticCanvas.ActualWidth)
            {
                _leftOffset = 0;

            }
            else
            {
                _leftOffset = _mousePos.X - mouseOffset.Left;
                if (staticCanvas.ActualWidth < outerBorder.ActualWidth)
                {
                    if (_leftOffset > 0)
                    {
                        mouseOffset.Left += _leftOffset;
                        mouseOffset.Right -= _leftOffset;
                        _leftOffset = 0;
                    }
                    else if (_leftOffset < staticCanvas.ActualWidth - outerBorder.ActualWidth)
                    {
                        double _diff = (staticCanvas.ActualWidth - outerBorder.ActualWidth) - _leftOffset;
                        mouseOffset.Left -= _diff;
                        mouseOffset.Right += _diff;
                        _leftOffset = staticCanvas.ActualWidth - outerBorder.ActualWidth;
                    }
                }
            }

            if (outerBorder.ActualHeight <= staticCanvas.ActualHeight)
            {
                _topOffset = 0;
            }
            else
            {
                _topOffset = _mousePos.Y - mouseOffset.Top;
                if (staticCanvas.ActualHeight < outerBorder.ActualHeight)
                {
                    if (_topOffset > 0)
                    {
                        mouseOffset.Top += _topOffset;
                        mouseOffset.Bottom -= _topOffset;
                        _topOffset = 0;
                    }
                    else if (_topOffset < staticCanvas.ActualHeight - outerBorder.ActualHeight)
                    {
                        double _diff = staticCanvas.ActualHeight - outerBorder.ActualHeight - _topOffset;
                        mouseOffset.Top -= _diff;
                        mouseOffset.Bottom += _diff;
                        _topOffset = staticCanvas.ActualHeight - outerBorder.ActualHeight;
                    }
                }
            }

            outerBorder.Margin = new Thickness(_leftOffset, _topOffset, -_leftOffset, -_topOffset);
        }

        public void SetHoveredElement(DesignElement? _element)
        {
            hoveredElement = _element;
            OnElementHovered(hoveredElement);
        }

    }
}
