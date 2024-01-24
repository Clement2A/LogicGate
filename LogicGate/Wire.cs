using System;
using System.Diagnostics;
using System.Resources;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class Wire : DesignElement
    {
        Connector firstConnector;
        Connector? secondConnector;
        Line wire;
        Line flow;
        bool isOn = false;
        bool isActive = false;

        public bool IsActive => isActive;

        public Wire(DesignGrid _grid, Connector _origin) : base(_grid)
        {
            //Creating wire
            firstConnector = _origin;

            wire = new Line
            {
                StrokeThickness = DefaultValuesLibrary.WireThickness,
                Stroke = DefaultValuesLibrary.WireColor,
                X1 = firstConnector.ElementGrid.Margin.Left + firstConnector.ElementGrid.ActualWidth / 2,
                Y1 = firstConnector.ElementGrid.Margin.Top + firstConnector.ElementGrid.ActualHeight / 2,
                X2 = firstConnector.ElementGrid.Margin.Left + firstConnector.ElementGrid.ActualWidth / 2,
                Y2 = firstConnector.ElementGrid.Margin.Top + firstConnector.ElementGrid.ActualHeight / 2,
                Cursor = Cursors.Hand,
            };
            flow = new Line
            {
                StrokeThickness = DefaultValuesLibrary.FlowThickness,
                Stroke = DefaultValuesLibrary.FlowOffColor,
                X1 = firstConnector.ElementGrid.Margin.Left + firstConnector.ElementGrid.ActualWidth / 2,
                Y1 = firstConnector.ElementGrid.Margin.Top + firstConnector.ElementGrid.ActualHeight / 2,
                X2 = firstConnector.ElementGrid.Margin.Left + firstConnector.ElementGrid.ActualWidth / 2,
                Y2 = firstConnector.ElementGrid.Margin.Top + firstConnector.ElementGrid.ActualHeight / 2,
                StrokeDashArray = DefaultValuesLibrary.FlowDashArray,
                StrokeDashCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeDashOffset = DefaultValuesLibrary.FlowDashOffset,
                Cursor = Cursors.Hand,
                IsHitTestVisible = false,
            };

            //TODO Simplify this
            DoubleAnimation _anim = DefaultValuesLibrary.FlowAnimation;
            _anim.RepeatBehavior = RepeatBehavior.Forever;
            flow.BeginAnimation(Shape.StrokeDashOffsetProperty, _anim);

            ConnectorLoopPrevention.StartLoopPrevention(firstConnector);
            AddElement(wire);
            AddElement(flow);
            grid.SelectionOffset = new Point(0, 0);
            grid.OnMouseMove += UpdateSecondPositionFromMouse;
            grid.OnElementHovered += UpdateVisual;
            grid.OnLeftClickUp += SetWire;

            DisplayFlow(firstConnector.IsOn);
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
            grid.OnMouseMove -= UpdateSecondPositionFromMouse;
            grid.OnLeftClickUp -= SetWire;

            Connector? _connector = (Connector?)grid.HoveredElement;
            if (_connector == null)
            {
                DraggableConnector _newConnector = new(grid);
                _mousePos = grid.MousePosToGridPos(_mousePos);
                _mousePos.X -= DefaultValuesLibrary.ConnectorHandleSize / 2;
                _mousePos.Y -= DefaultValuesLibrary.ConnectorHandleSize / 2;
                _newConnector.SetPosition(_mousePos);
                ConnectSecondPosition(_newConnector);
            }
            else if (_connector == firstConnector || _connector.InCircuit)
            {
                DeleteElement();
            }
            else
                ConnectSecondPosition(_connector);

            secondConnector.ChangeInputState(firstConnector.IsOn);
            ConnectorLoopPrevention.StopLoopPrevention();
            grid.OnElementHovered -= UpdateVisual;
        }

        void ConnectSecondPosition(Connector _connector)
        {
            secondConnector = _connector;
            wire.X2 = secondConnector.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            wire.Y2 = secondConnector.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;
            flow.X2 = secondConnector.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            flow.Y2 = secondConnector.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;

            Debug.WriteLine("Setting up wire " + firstConnector.id + "-" + secondConnector.id);

            SetupEvent();
            secondConnector.AddConnector(firstConnector);
            firstConnector.AddConnector(secondConnector);
            wire.MouseRightButtonDown += (s, e) => { DeleteElement(); };
        }

        private void SetupEvent()
        {
            firstConnector.OnElementMove += UpdateFirstPosition;
            secondConnector!.OnElementMove += UpdateSecondPosition;
            firstConnector.OnInputChanged += DisplayFlow;
            secondConnector.OnInputChanged += DisplayFlow;
        }

        void UpdateFirstPosition(Point _pos)
        {
            double _x1 = _pos.X + firstConnector.ElementGrid.ActualWidth / 2;
            double _y1 = _pos.Y + firstConnector.ElementGrid.ActualHeight / 2;
            wire.X1 = _x1;
            wire.Y1 = _y1;
            flow.X1 = _x1;
            flow.Y1 = _y1;
        }

        void UpdateFirstPosition(Thickness _pos)
        {
            UpdateFirstPosition(new Point(_pos.Left, _pos.Top));
        }

        void UpdateSecondPosition(Point _pos)
        {
            double _x2 = _pos.X + secondConnector!.ElementGrid.ActualWidth / 2;
            double _y2 = _pos.Y + secondConnector.ElementGrid.ActualHeight / 2;
            wire.X2 = _x2;
            wire.Y2 = _y2;
            flow.X2 = _x2;
            flow.Y2 = _y2;
        }

        void UpdateSecondPosition(Thickness _pos)
        {
            UpdateSecondPosition(new Point(_pos.Left, _pos.Top));
        }

        void UpdateSecondPositionFromMouse(Point _mousePos)
        {
            Point _gridPos = firstConnector.Grid.MousePosToGridPos(_mousePos);
            wire.X2 = _gridPos.X;
            wire.Y2 = _gridPos.Y;
            flow.X2 = _gridPos.X;
            flow.Y2 = _gridPos.Y;
        }

        protected override void DeleteElement()
        {
            base.DeleteElement();


            firstConnector.OnElementMove -= UpdateFirstPosition;
            if (secondConnector == null)
                return;
            firstConnector.RemoveConnector(secondConnector);
            secondConnector.RemoveConnector(firstConnector);
        }

        void DisplayFlow(bool _isOn)
        {
            Debug.WriteLine("I am wire " + firstConnector.id + " - " + secondConnector?.id + ".Result from my input number " + firstConnector.id + " has changed");
            isOn = _isOn;
            flow.Stroke = _isOn ? DefaultValuesLibrary.FlowOnColor : DefaultValuesLibrary.FlowOffColor;
        }
        private void SwitchAndDisplayFlow(bool _input)
        {
            SwapConnectors();
            DisplayFlow(_input);
        }

        void SwapConnectors()
        {
            //Swap test
            firstConnector.OnElementMove -= UpdateFirstPosition;
            secondConnector!.OnElementMove -= UpdateSecondPosition;
            firstConnector.OnInputChanged -= DisplayFlow;
            secondConnector.OnInputChanged -= SwitchAndDisplayFlow;

            Connector _temp = firstConnector;
            firstConnector = secondConnector;
            secondConnector = _temp;

            SetupEvent();

        }
    }
}
