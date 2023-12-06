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
using System.Windows.Shapes;

namespace LogicGate
{
    internal class DesignGrid
    {
        Canvas staticCanvas = new();
        Grid moveGrid = new();
        Border outerBorder = new();
        Border innerBorder = new();

        int padding = 50;
        int margin = 20;

        public event Action<Point> OnCanvasMove = delegate { };
        public event Action<Point> OnDrag = delegate { };
        public event Action<Point> OnLeftClickUp = delegate { };
        public event Action<Point> OnRightClickUp = delegate { };
        public event Action OnLeftClickDown = delegate { };
        public event Action OnRightClickDown = delegate { };
        public event Action<Point> OnMouseMove = delegate { };

        Thickness mouseOffset = new();

        public Canvas StaticGrid => staticCanvas;

        public DesignGrid()
        {
            InitGrids();
            InitEvents();
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.Red;
            ellipse.Height = 150;
            ellipse.Width = 150;
            moveGrid.Children.Add(ellipse);
            ellipse.Margin = new(50, 0, -50, 0);

        }

        void InitGrids()
        {
            innerBorder.Background = Brushes.Gray;
            staticCanvas.Children.Add(outerBorder);
            outerBorder.Child = innerBorder;
            innerBorder.Child = moveGrid;
            staticCanvas.Background = Brushes.Black;
            innerBorder.Margin = new(padding);
            moveGrid.Margin = new(margin);
            moveGrid.Width = double.NaN;
            moveGrid.Height = double.NaN;
            moveGrid.MinWidth = 100;
            moveGrid.MinHeight = 100;
        }

        void InitEvents()
        {
            staticCanvas.MouseRightButtonDown += (sender, args) => { OnRightClickDown.Invoke(); };
            staticCanvas.MouseRightButtonUp += (sender, args) => { OnRightClickUp.Invoke(args.GetPosition(staticCanvas)); };
            staticCanvas.MouseLeftButtonDown += (sender, args) => { OnLeftClickDown.Invoke(); };
            staticCanvas.MouseMove += (sender, args) => { OnMouseMove.Invoke(args.GetPosition(staticCanvas)); };
            staticCanvas.MouseLeave += (sender, args) => { ResetEvents(); };
            OnRightClickDown += RightDragOrClick;
            staticCanvas.SizeChanged += (sender, args) => { MoveCanvas(new(0, 0)); };
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
            OnRightClickUp = delegate { };
            OnMouseMove = delegate { };
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
            ResetRightDragOrClick();
            mouseOffset = new(0);
        }

        void MoveCanvas(Point _mousePos)
        {
            Size _staticGridSize = staticCanvas.RenderSize;
            Size _outerBorderSize = outerBorder.DesiredSize;

            double _leftOffset = _mousePos.X - mouseOffset.Left;
            double _topOffset = _mousePos.Y - mouseOffset.Top;
            if (_staticGridSize.Width < outerBorder.DesiredSize.Width)
            {
                if (_leftOffset > 0)
                {
                    mouseOffset.Left += _leftOffset;
                    mouseOffset.Right -= _leftOffset;
                    _leftOffset = 0;
                }
                else if (_leftOffset < _staticGridSize.Width - _outerBorderSize.Width)
                {
                    double _diff = (_staticGridSize.Width - _outerBorderSize.Width) - _leftOffset;
                    mouseOffset.Left -= _diff;
                    mouseOffset.Right += _diff;
                    _leftOffset = _staticGridSize.Width - _outerBorderSize.Width;
                }
            }
            else
            {
                _leftOffset = ((_staticGridSize.Width - _outerBorderSize.Width)) / 2;
            }
            if (_staticGridSize.Height < outerBorder.DesiredSize.Height)
            {
                if (_topOffset > 0)
                {
                    mouseOffset.Top += _topOffset;
                    mouseOffset.Bottom -= _topOffset;
                    _topOffset = 0;
                }
                else if (_topOffset < _staticGridSize.Height - _outerBorderSize.Height)
                {
                    double _diff = _staticGridSize.Height - _outerBorderSize.Height - _topOffset;
                    mouseOffset.Top -= _diff;
                    mouseOffset.Bottom += _diff;
                    _topOffset = _staticGridSize.Height - _outerBorderSize.Height;
                }
            }
            else
            {
                _topOffset = ((_staticGridSize.Height - _outerBorderSize.Height)) / 2;
            }
            outerBorder.Margin = new Thickness(_leftOffset, _topOffset, -_leftOffset, -_topOffset);
            Debug.WriteLine("Margins are " + outerBorder.Margin);
            Debug.WriteLine("Size is " + _staticGridSize);
            Debug.WriteLine("Move size is " + _outerBorderSize);
        }

        void MouseLock(Point _mousePos)
        {
            
        }
    }
}
