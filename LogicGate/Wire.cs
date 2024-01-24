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

            ConnectorLoopPrevention.StartLoopPrevention(input);
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
            }
            else if (_connector == input || _connector.InCircuit)
            {
                DeleteElement();
            }
            else
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

            Debug.WriteLine("Setting up wire " + input.id + "-" + output.id);

            bool _isOutputOrigin = output.GetInput() != null;
            bool _isInputOrigin = input.GetInput() != null;
            SetupEvent();
            output.AddConnector(input, _isInputOrigin);
            input.AddConnector(output, _isOutputOrigin);
            if (_isInputOrigin)
            {
                Debug.WriteLine("Wire " + input.id + " - " + output.id + " ---> Input is input, normal behaviour");
                ConnectInput();
            }
            else if (_isOutputOrigin)
            {
                Debug.WriteLine("Wire " + input.id + " - " + output.id + " ---> Output is input, swapping");
                SwapConnectors();
                ConnectInput();
            }
            else
            {
                Debug.WriteLine("Wire " + input.id + " - " + output.id + " ---> No one is input, waiting");
                InitInputEvents();
            }
            wire.MouseRightButtonDown += (s, e) => { DeleteElement(); };
        }

        private void InitInputEvents()
        {
            input.OnGetInput += RemoveInitInputEvents;
            output!.OnGetInput += RemoveInitInputEvents;
            input.OnGetInput += CheckOrderInput;
            output.OnGetInput += CheckOrderOutput;
        }

        private void RemoveInitInputEvents(Connector _notNeeded)
        {
            input.OnGetInput -= RemoveInitInputEvents;
            output!.OnGetInput -= RemoveInitInputEvents;
            input.OnGetInput -= CheckOrderInput;
            output.OnGetInput -= CheckOrderOutput;
        }

        private void CheckOrderOutput(Connector _notNeeded)
        {
            Debug.WriteLine("Wire " + input.id + "-" + output.id + " received new input from output, it is " + _notNeeded.id);
            if (_notNeeded.InputConnector == input)
                return;
            SwapConnectors();
            ConnectInput();
        }

        private void CheckOrderInput(Connector _notNeeded)
        {
            Debug.WriteLine("Wire " + input.id + "-" + output.id + " received new input from output, it is " + _notNeeded.id);
            if (_notNeeded.InputConnector == output)
                return;
            ConnectInput();
        }

        private void ConnectInput(Connector _notNeeded)
        {
            ConnectInput();
        }
        private void ConnectInput()
        {
            Debug.WriteLine("Wire between number " + input.id + " and " + output!.id + " found an input again! It's the first one");
            IOutput? _origin = input.GetInput() as IOutput;
            if(_origin == null)
            {
                Debug.WriteLine("ERROR: Given connector input doesn't actually have an input");
                return;
            }
            SwitchFlow(_origin.OutputResult);
            _origin.OnOutputChange += SwitchFlow;
            SetupInputLostEvents();
        }

        private void SetupInputLostEvents()
        {
            input.OnLoseInput += SetupInputEvent;
        }

        private void SetupInputEvent()
        {
            Debug.WriteLine("Wire between number " + input.id + " and " + output!.id + " does not have an actual input anymore");
            SwitchFlow(false);
            IOutput? _origin = input.GetInput() as IOutput;
            if (_origin == null)
            {
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

        void UpdateFirstPosition(Point _pos)
        {
            double _x1 = _pos.X + input.ElementGrid.ActualWidth / 2;
            double _y1 = _pos.Y + input.ElementGrid.ActualHeight / 2;
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
            double _x2 = _pos.X + output!.ElementGrid.ActualWidth / 2;
            double _y2 = _pos.Y + output.ElementGrid.ActualHeight / 2;
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
            CancelAllPossibleEvents();
            input.RemoveConnector(output);
            output.RemoveConnector(input);
        }

        void SwitchFlow(bool _isOn)
        {
            Debug.WriteLine("I am wire " + input.id + " - " + output?.id + ".Result from my input number " + input.id + " has changed");
            isOn = _isOn;
            flow.Stroke = _isOn ? DefaultValuesLibrary.FlowOnColor : DefaultValuesLibrary.FlowOffColor;
        }

        void SwapConnectors()
        {
            input.OnElementMove -= UpdateFirstPosition;
            output!.OnElementMove -= UpdateSecondPosition;
            
            Connector _temp = input;
            input = output;
            output = _temp;
            
            input.OnElementMove += UpdateFirstPosition;
            output.OnElementMove += UpdateSecondPosition;
            
            UpdateFirstPosition(input.GetPosition());
            UpdateSecondPosition(output.GetPosition());
        }

        void CancelAllPossibleEvents()
        {
            input.OnElementMove -= UpdateFirstPosition;
            output.OnElementMove -= UpdateSecondPosition;
            input.OnElementMove -= UpdateFirstPosition;
            output!.OnElementMove -= UpdateSecondPosition;
            input.OnLoseInput -= SetupInputEvent;
            input.OnGetInput -= RemoveInitInputEvents;
            output!.OnGetInput -= RemoveInitInputEvents;
            input.OnGetInput -= CheckOrderInput;
            output.OnGetInput -= CheckOrderOutput;
            grid.OnMouseMove -= UpdateSecondPositionFromMouse;
            grid.OnElementHovered -= UpdateVisual;
            grid.OnLeftClickUp -= SetWire;

            IOutput? _origin = input.GetInput() as IOutput;
            if (_origin == null)
                return;
            _origin.OnOutputChange -= SwitchFlow;
        }
    }
}
