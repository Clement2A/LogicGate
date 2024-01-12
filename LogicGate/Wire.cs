using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class Wire : DesignElement
    {
        Connector input;
        Connector? output;
        Line wire;
        bool isActive;

        public bool IsActive => isActive;

        public Wire(DesignGrid _grid, Connector _origin) : base(_grid)
        {
            //Creating wire
            input = _origin;

            wire = new Line
            {
                StrokeThickness = DefaultValuesLibrary.WireThickness,
                Stroke = DefaultValuesLibrary.WireColor,
                X1 = input.ElementGrid.Margin.Left + input.ElementGrid.ActualWidth / 2,
                Y1 = input.ElementGrid.Margin.Top + input.ElementGrid.ActualHeight / 2,
                X2 = input.ElementGrid.Margin.Left + input.ElementGrid.ActualWidth / 2,
                Y2 = input.ElementGrid.Margin.Top + input.ElementGrid.ActualHeight / 2,
                Cursor = Cursors.Hand,
            };
            ConnectorLoopPrevention.StartLoopPrevention(input);
            input.OnElementMove += UpdateFirstPosition;
            AddElement(wire);
            grid.SelectionOffset = new Point(0, 0);
            grid.OnMouseMove += UpdateSecondPosition;
            grid.OnElementHovered += UpdateVisual;
            grid.OnLeftClickUp += SetWire;
        }

        private void UpdateVisual(DesignElement? element)
        {
            Connector? _otherElement = (Connector?)element;
            if (element == null || _otherElement != null && !_otherElement.InCircuit)
            {
                wire.Stroke = DefaultValuesLibrary.WireColor;
                return;
            }

            wire.Stroke = DefaultValuesLibrary.WireInvalidColor;
        }

        void SetWire(Point _mousePos)
        {
            grid.OnMouseMove -= UpdateSecondPosition;
            grid.OnLeftClickUp -= SetWire;

            Connector? _connector = (Connector?)grid.HoveredElement;
            if (_connector == null)
            {
                Connector _newConnector = new(grid);
                _mousePos = grid.MousePosToGridPos(_mousePos);
                _mousePos.X -= DefaultValuesLibrary.ConnectorHandleSize / 2;
                _mousePos.Y -= DefaultValuesLibrary.ConnectorHandleSize / 2;
                _newConnector.SetPosition(_mousePos);
                ConnectSecondPosition(_newConnector);
                ConnectorLoopPrevention.StopLoopPrevention();
                grid.OnElementHovered -= UpdateVisual;
                return;
            }

            if (_connector == input || _connector.InCircuit)
            {
                DeleteElement();
                ConnectorLoopPrevention.StopLoopPrevention();
                return;
            }

            ConnectSecondPosition(_connector);
            ConnectorLoopPrevention.StopLoopPrevention();
            grid.OnElementHovered -= UpdateVisual;
        }

        void ConnectSecondPosition(Connector _connector)
        {
            output = _connector;
            wire.X2 = output.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            wire.Y2 = output.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;
            output.OnElementMove += UpdateSecondPosition;
            output.AddConnector(input);
            input.AddConnector(output);
            wire.MouseRightButtonDown += (s, e) => { DeleteElement(); };
        }

        void UpdateFirstPosition(Thickness _pos)
        {
            wire.X1 = _pos.Left + input.ElementGrid.ActualWidth / 2;
            wire.Y1 = _pos.Top + input.ElementGrid.ActualHeight / 2;
        }

        void UpdateSecondPosition(Thickness _pos)
        {
            wire.X2 = _pos.Left + output!.ElementGrid.ActualWidth / 2;
            wire.Y2 = _pos.Top + output.ElementGrid.ActualHeight / 2;
        }

        void UpdateSecondPosition(Point _mousePos)
        {
            Point _gridPos = input.Grid.MousePosToGridPos(_mousePos);
            wire.X2 = _gridPos.X;
            wire.Y2 = _gridPos.Y;
        }

        protected override void DeleteElement()
        {
            base.DeleteElement();
            
            input.OnElementMove -= UpdateFirstPosition;
            if (output == null)
                return;
            input.RemoveConnector(output);
            output.OnElementMove -= UpdateSecondPosition;
            output.RemoveConnector(input);
            //Panel _parent = (Panel)VisualTreeHelper.GetParent(wire);
            //_parent.Children.Remove(wire);
            //GC.Collect();
        }

        
    }
}
