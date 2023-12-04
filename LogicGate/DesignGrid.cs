using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogicGate
{
    internal class DesignGrid
    {
        Grid staticGrid = new Grid();
        Grid moveGrid = new Grid();
        public event Action<Point>? onCanvasMove;
        public event Action<Point>? onDrag;
        public event Action? onLeftClickUp;
        public event Action? onRightClickUp;

        public Grid StaticGrid => staticGrid;

        public DesignGrid()
        {
            staticGrid.Children.Add(moveGrid);
            staticGrid.MouseLeftButtonUp += (sender, args) => { onLeftClickUp?.Invoke(); };
            staticGrid.MouseRightButtonUp += (sender, args) => { onRightClickUp?.Invoke(); };
        }

        void RightDragOrClick()
        {
            staticGrid.MouseRightButtonUp += (sender, args) => { OpenMenu(args.GetPosition(staticGrid)); };
            staticGrid.MouseRightButtonUp += (sender, args) => { ResetRightDragOrClick(); };
            staticGrid.MouseMove += (sender, args) => { };
            staticGrid.MouseMove += (sender, args) => { ResetRightDragOrClick(); };
        }

        void ResetRightDragOrClick()
        {

            staticGrid.MouseRightButtonUp -= (sender, args) => { ResetRightDragOrClick(); };
            staticGrid.MouseMove -= (sender, args) => { ResetRightDragOrClick(); };
        }

        void OpenMenu(Point _position)
        {
            Debug.WriteLine("Menu not implemented yet. Cursor is at " + _position.X + " - " + _position.Y);
        }

        void StartMovingGrid()
        {
            staticGrid.MouseMove += (sender, args) => { MoveCanvas(); };
            onRightClickUp += StopMovingGrid;
        }

        void StopMovingGrid()
        {
            staticGrid.MouseMove -= (sender, args) => { MoveCanvas(); };
            onRightClickUp -= StopMovingGrid;
        }

        void MoveCanvas()
        {
            Debug.WriteLine("Moving Canvas");
        }
    }
}
