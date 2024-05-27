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

            DisplayFlow(firstConnector.IsOn, null, null);
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
                return;
            }
            else
                ConnectSecondPosition(_connector);

            secondConnector.ChangeInputState(firstConnector.IsOn,null,firstConnector);
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

            SetupEvent();
            secondConnector.AddConnector(firstConnector);
            firstConnector.AddConnector(secondConnector);
            wire.MouseRightButtonDown += (s, e) => { DeleteElement(); };

            if(secondConnector.IsOn)
            {
                SwapConnectors();
                DisplayFlow(true, null, null);
            }

        }

        private void SetupEvent()
        {
            firstConnector.OnElementMove += UpdateFirstPosition;
            secondConnector!.OnElementMove += UpdateSecondPosition;
            firstConnector.OnInputChanged += DisplayFlow;
            secondConnector.OnInputChanged += CheckForSwitch;
            firstConnector.OnDelete += DeleteElement;
            secondConnector.OnDelete += DeleteElement;
        }

        private void UnsetEvents()
        {
            firstConnector.OnElementMove -= UpdateFirstPosition;
            firstConnector.OnInputChanged -= DisplayFlow;
            firstConnector.OnDelete -= DeleteElement;
            if (secondConnector == null)
                return;
            secondConnector.OnElementMove -= UpdateSecondPosition;
            secondConnector.OnInputChanged -= CheckForSwitch;
            secondConnector.OnDelete -= DeleteElement;
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

            UnsetEvents();

            if (secondConnector == null)
                return;
            firstConnector.RemoveConnector(secondConnector);
            secondConnector.RemoveConnector(firstConnector);

            if (!isOn)
                return;

            secondConnector.ChangeInputState(false, null, null);
        }

        void DisplayFlow(bool _isOn, Connector? _prevSource, Connector? _origin)
        {
            isOn = _isOn;
            flow.Stroke = _isOn ? DefaultValuesLibrary.FlowOnColor : DefaultValuesLibrary.FlowOffColor;

            //if(secondConnector != null)
            //    secondConnector.ChangeInputState(isOn);
        }
        private void CheckForSwitch(bool _input, Connector? _prevSource, Connector? _origin)
        {
            if(_prevSource == firstConnector) 
                return;
            SwapConnectors();
            DisplayFlow(_input, _prevSource, _origin);
        }

        void SwapConnectors()
        {
            //Swap test
            firstConnector.OnElementMove -= UpdateFirstPosition;
            secondConnector!.OnElementMove -= UpdateSecondPosition;
            firstConnector.OnInputChanged -= DisplayFlow;
            secondConnector.OnInputChanged -= CheckForSwitch;

            Connector _temp = firstConnector;
            firstConnector = secondConnector;
            secondConnector = _temp;


            flow.X1 = firstConnector.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            flow.Y1 = firstConnector.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;
            flow.X2 = secondConnector.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            flow.Y2 = secondConnector.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;

            wire.X1 = firstConnector.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            wire.Y1 = firstConnector.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;
            wire.X2 = secondConnector.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            wire.Y2 = secondConnector.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;

            SetupEvent();

        }
    }
}
