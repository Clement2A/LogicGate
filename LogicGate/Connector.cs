using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LogicGate
{
    internal class Connector : DesignElement
    {
        Ellipse connectorShape;
        protected Ellipse moveHandle;
        List<Connector> connectors = new();

        Connector? inputConnector = null;
        IOutput? inputSource = null;

        public IOutput? InputSource => inputSource;

        public event Action<Connector?, IOutput?> OnInputChanged = delegate { };
        public event Action OnInputReset = delegate { };

        bool isOn = false;

        public bool IsOn => isOn;

        public int id = 0;

        static int idCount = 0;

        public List<Connector> Connectors => connectors;

        public bool InCircuit { get; set; } = false;

        public Connector(DesignGrid _grid) : base(_grid)
        {
            id = idCount++;
            moveHandle = new Ellipse
            {
                Fill = DefaultValuesLibrary.ConnectorHandleColor,
                Height = DefaultValuesLibrary.ConnectorHandleSize,
                Width = DefaultValuesLibrary.ConnectorHandleSize,
                Margin = DefaultValuesLibrary.ConnectorHandleOffset,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Cursor = Cursors.Hand,
            };
            MakeElementHoverable(moveHandle);
            AddElement(moveHandle);
            connectorShape = new Ellipse
            {
                Fill = DefaultValuesLibrary.ConnectorInactiveColor,
                Height = DefaultValuesLibrary.ConnectorShapeSize,
                Width = DefaultValuesLibrary.ConnectorShapeSize,
                Margin = DefaultValuesLibrary.ConnectorShapeOffset,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Cursor = Cursors.Hand,
            };
            Panel.SetZIndex(ElementGrid, 1);
            connectorShape.PreviewMouseLeftButtonDown += OnConnectorClick;
            MakeElementHoverable(connectorShape);
            AddElement(connectorShape);
        }

        void OnConnectorClick(object _sender, MouseButtonEventArgs _e)
        {
            grid.OnMouseMove += OnCreateWire;
        }

        void OnCreateWire(Point _position)
        {
            grid.OnMouseMove -= OnCreateWire;
            Wire _wire = new Wire(grid, this);
        }

        public void AddConnector(Connector _connector)
        {
            connectors.Add(_connector);

            if(_connector.InputSource != null)
                SetInput(_connector, _connector.InputSource);
        }

        public void RemoveConnector(Connector _connector)
        {
            connectors.Remove(_connector);

            if (_connector == inputSource)
                ResetInput();
        }

        public void ResetInCircuit()
        {
            InCircuit = false;
        }
        
        public void SetInput(Connector _connector, IOutput _output)
        {
            if (inputSource != null)
            {
                Debug.WriteLine("Connector " + id + " received a new input, but had already one, ignoring new input");
                return;
            }

            inputConnector = _connector;
            inputSource = _output;

            OnInputChanged.Invoke(_connector, _output);

            PropagateInput(_connector, _output);
        }

        public void ResetInput()
        {
            if (inputSource == null)
            {
                Debug.WriteLine("Connector " + id + " tried to reset input, but already has none");
                return;
            }

            Connector _previousInputConnector = inputConnector!;

            inputConnector = null; 
            inputSource = null;

            OnInputReset.Invoke();

            PropagateInputReset(_previousInputConnector);

        }

        private void PropagateInput(Connector _connector, IOutput _output)
        {
            connectors.Remove(_connector);

            foreach (Connector _linkedConnector in connectors)
                _linkedConnector.SetInput(_connector, _output);

            connectors.Add(_connector);
        }
        private void PropagateInputReset(Connector _connector)
        {
            connectors.Remove(_connector);

            foreach (Connector _linkedConnector in connectors)
                _linkedConnector.ResetInput();

            connectors.Add(_connector);
        }
    }
}
