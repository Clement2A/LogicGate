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
        Connector input;
        Connector? output;
        Line wire;
        Line flow;
        bool isOn = false;
        bool isActive = false;

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
            flow = new Line
            {
                StrokeThickness = DefaultValuesLibrary.FlowThickness,
                Stroke = DefaultValuesLibrary.FlowOffColor,
                X1 = input.ElementGrid.Margin.Left + input.ElementGrid.ActualWidth / 2,
                Y1 = input.ElementGrid.Margin.Top + input.ElementGrid.ActualHeight / 2,
                X2 = input.ElementGrid.Margin.Left + input.ElementGrid.ActualWidth / 2,
                Y2 = input.ElementGrid.Margin.Top + input.ElementGrid.ActualHeight / 2,
                StrokeDashArray = new DoubleCollection() { 2, 2.5 },
                StrokeDashCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeDashOffset = 4.5,
                Cursor = Cursors.Hand,
                IsHitTestVisible = false,
            };

            //TODO Simplify this
            DoubleAnimation _anim = DefaultValuesLibrary.FlowAnimation;
            _anim.RepeatBehavior = RepeatBehavior.Forever;
            flow.BeginAnimation(Shape.StrokeDashOffsetProperty, _anim);

            

            ConnectorLoopPrevention.StartLoopPrevention(input);
            //input.OnElementMove += UpdateFirstPosition;
            AddElement(wire);
            AddElement(flow);
            grid.SelectionOffset = new Point(0, 0);
            grid.OnMouseMove += UpdateSecondPositionFromMouse;
            grid.OnElementHovered += UpdateVisual;
            grid.OnLeftClickUp += SetWire;

            IOutput? _logic = input.GetInput() as IOutput;
            if (_logic == null)
                return;

            SwitchFlow(_logic.OutputResult);
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
            flow.X2 = output.ElementGrid.Margin.Left + DefaultValuesLibrary.ConnectorHandleSize / 2;
            flow.Y2 = output.ElementGrid.Margin.Top + DefaultValuesLibrary.ConnectorHandleSize / 2;

            
            bool _isOutputOrigin = output.GetInput() != null;
            bool _isInputOrigin = input.GetInput() != null;
            output.AddConnector(input, _isInputOrigin);
            input.AddConnector(output, _isOutputOrigin);
            SetupEvent();
            if (_isInputOrigin)
            {
                Debug.WriteLine("Input is already input");
                ConnectInput();
            }
            else if (_isOutputOrigin)
            {
                Debug.WriteLine("Input is output, swapping");
                SwapConnectors();
            }
            else
            {
                Debug.WriteLine("No one is input, waiting");
                InitInputEvents();
            }
            wire.MouseRightButtonDown += (s, e) => { DeleteElement(); };
        }

        private void InitInputEvents()
        {
            input.OnGetInput += RemoveInitInputEvents;
            output.OnGetInput += RemoveInitInputEvents;
            input.OnGetInput += ConnectInput;
            output.OnGetInput += SwapThenConnectInput;
        }

        private void RemoveInitInputEvents(Connector _notNeeded)
        {
            input.OnGetInput -= RemoveInitInputEvents;
            output.OnGetInput -= RemoveInitInputEvents;
            input.OnGetInput -= ConnectInput;
            output.OnGetInput -= SwapThenConnectInput;
        }

        private void SwapThenConnectInput(Connector _notNeeded)
        {
            SwapConnectors();
            ConnectInput(_notNeeded);
        }

        private void ConnectInput(Connector _notNeeded)
        {
            ConnectInput();
        }
        private void ConnectInput()
        {
            IOutput? _origin = input.GetInput() as IOutput;
            if(_origin == null)
            {
                Debug.WriteLine("ERROR: Given connector input doesn't actually have an input");
                return;
            }
            Debug.WriteLine("Got origin! It's " + _origin.Id);
            _origin.OnOutputChange += SwitchFlow;
            SetupInputLostEvents();
        }

        private void SetupInputLostEvents()
        {
            input.OnLoseInput += SetupInputEvent;
        }

        private void SetupInputEvent()
        {
            IOutput? _origin = input.GetInput() as IOutput;
            if (_origin == null)
            {
                //Triggers afeter input loss
                Debug.WriteLine("ERROR: Given connector input doesn't actually have an input");
                return;
            }
            _origin.OnOutputChange -= SwitchFlow;
            input.OnLoseInput -= SetupInputEvent;
            InitInputEvents();
        }

        private void SetupEvent()
        {
            input.OnElementMove += UpdateFirstPosition;
            output!.OnElementMove += UpdateSecondPosition;
        }

        void UpdateFirstPosition(Thickness _pos)
        {
            wire.X1 = _pos.Left + input.ElementGrid.ActualWidth / 2;
            wire.Y1 = _pos.Top + input.ElementGrid.ActualHeight / 2;
            flow.X1 = _pos.Left + input.ElementGrid.ActualWidth / 2;
            flow.Y1 = _pos.Top + input.ElementGrid.ActualHeight / 2;
        }

        void UpdateSecondPosition(Thickness _pos)
        {
            wire.X2 = _pos.Left + output!.ElementGrid.ActualWidth / 2;
            wire.Y2 = _pos.Top + output.ElementGrid.ActualHeight / 2;
            flow.X2 = _pos.Left + output!.ElementGrid.ActualWidth / 2;
            flow.Y2 = _pos.Top + output.ElementGrid.ActualHeight / 2;
        }

        void UpdateSecondPositionFromMouse(Point _mousePos)
        {
            Point _gridPos = input.Grid.MousePosToGridPos(_mousePos);
            wire.X2 = _gridPos.X;
            wire.Y2 = _gridPos.Y;
            flow.X2 = _gridPos.X;
            flow.Y2 = _gridPos.Y;
        }

        protected override void DeleteElement()
        {
            base.DeleteElement();
            
            input.OnElementMove -= UpdateFirstPosition;
            if (output == null)
                return;
            //Debug.WriteLine("Removing output from input, shouldn't trigger anything");
            input.RemoveConnector(output);
            output.OnElementMove -= UpdateSecondPosition;
            //Debug.WriteLine("Removing input from output, remove events");
            output.RemoveConnector(input);
        }

        void SwitchFlow(bool _isOn)
        {
            isOn = _isOn;
            flow.Stroke = _isOn ? DefaultValuesLibrary.FlowOnColor : DefaultValuesLibrary.FlowOffColor;
        }

        void SwapConnectors()
        {
            //IOutput? _origin = input.GetInput() as IOutput;
            //if (_origin != null)
            //    _origin.OnOutputChange -= SwitchFlow;

            input.OnElementMove -= UpdateFirstPosition;
            output!.OnElementMove -= UpdateSecondPosition;

            Connector _temp = input;
            input = output;
            output = _temp;

            //_origin = input.GetInput() as IOutput;
            //if (_origin != null)
            //    _origin.OnOutputChange -= SwitchFlow;

            input.OnElementMove += UpdateFirstPosition;
            output.OnElementMove += UpdateSecondPosition;
        }
        
    }
}
