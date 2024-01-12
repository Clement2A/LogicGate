﻿using System;
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
    internal abstract class DesignElement
    {
        protected DesignGrid grid;
        protected Grid elementGrid;

        public DesignGrid Grid => grid;
        public Grid ElementGrid => elementGrid;

        public event Action<Thickness> OnElementMove = delegate { };

        public DesignElement(DesignGrid _grid)
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

        protected void MakeElementDraggable(UIElement _dragElement)
        {
            _dragElement.PreviewMouseLeftButtonDown += StartDraggableBehaviour;
        }

        protected void MakeElementHoverable(UIElement _hoverElement)
        {
            _hoverElement.MouseEnter += StartHoverBehaviour;
            _hoverElement.MouseLeave += EndHoverBehaviour;
        }

        private void EndHoverBehaviour(object sender, MouseEventArgs e)
        {
            if(grid.HoveredElement == this)
                grid.SetHoveredElement(null);
        }

        private void StartHoverBehaviour(object sender, MouseEventArgs e)
        {
            grid.SetHoveredElement(this);
        }

        protected void AddElement(UIElement _shape)
        {
            elementGrid.Children.Add(_shape);
        }

        protected void StartDraggableBehaviour(object _sender, MouseButtonEventArgs _e)
        {
            Point _gridPos = grid.MousePosToGridPos(_e.GetPosition(grid.StaticGrid));
            grid.SelectionOffset = new Point(elementGrid.Margin.Left - _gridPos.X, elementGrid.Margin.Top - _gridPos.Y);
            grid.OnMouseMove += OnMoveAround;
            grid.OnLeftClickUp += OnUnselect;
            //Debug.WriteLine("selected");
        }

        protected void OnMoveAround(Point _position)
        {
            Point _gridPos = grid.MousePosToGridPos(_position);
            _gridPos.X += grid.SelectionOffset.X;
            _gridPos.Y += grid.SelectionOffset.Y;
            SetPosition(_gridPos);
            OnElementMove.Invoke(elementGrid.Margin);
            //Debug.WriteLine("Move element");
            //Debug.WriteLine("Position is " + _position.X + " - " + _position.Y);
            //Debug.WriteLine("Offset is " + grid.SelectionOffset.X + " - " + grid.SelectionOffset.Y);
            //Debug.WriteLine("Grid is " + _gridPos.X + " - " + _gridPos.Y);
        }

        protected void OnUnselect(Point _position)
        {
            grid.OnMouseMove -= OnMoveAround;
            grid.OnLeftClickUp -= OnUnselect;
            //Debug.WriteLine("Unselect");
        }

        protected virtual void DeleteElement()
        {
            elementGrid.Children.Clear();
            Panel _parent = (Panel)VisualTreeHelper.GetParent(elementGrid);
            _parent.Children.Remove(elementGrid);
            Debug.WriteLine("Element deletion is done");
        }

        public void SetPosition(Point _position)
        {
            if (_position.X < 0)
                _position.X = 0;
            if (_position.Y < 0)
                _position.Y = 0;
            elementGrid.Margin = new(_position.X, _position.Y, 0, 0);
        }
    }
}