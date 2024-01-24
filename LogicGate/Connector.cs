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

        bool isOn = false;

        public bool IsOn => isOn;

        public event Action<bool> OnInputChanged = delegate { };

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
            _connector.OnInputChanged += ChangeInputState;
        }

        public void ChangeInputState(bool _input)
        {
            Debug.WriteLine("Connector " + id + " changing state, going from " + isOn + " to " + _input);
            if (isOn == _input)
                return;
            isOn = _input;
            OnInputChanged.Invoke(isOn);
        }

        public void RemoveConnector(Connector _connector)
        {
            connectors.Remove(_connector);
            _connector.OnInputChanged -= ChangeInputState;
        }

        public void ResetInCircuit()
        {
            InCircuit = false;
        }
    }
}
