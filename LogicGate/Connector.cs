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
        protected LogicElement? input = null;

        public int id = 0;

        static int idCount = 0;

        public event Action<Connector> OnGetInput = delegate { };
        public event Action OnLoseInput = delegate { };

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

        public void AddConnector(Connector _connector, bool _isInputOrigin)
        {
            connectors.Add(_connector);
            if(_isInputOrigin)
            {
                Debug.WriteLine("Number " + _connector.id + " is the input of number " + id);
                SetInput(_connector);
                _connector.OnLoseInput += RemoveInput;
            }
            else
            {
                _connector.OnGetInput += SetInput;
            }
        }

        public void RemoveConnector(Connector _connector)
        {
            Debug.WriteLine("This is number " + id + ", removing connector number " + _connector.id + " from my list");
            connectors.Remove(_connector);
            if(input != null && inputConnector == _connector)
            {
                Debug.WriteLine(_connector.id + " is also my input, removing that aswell");
                RemoveInput();
            }
        }

        public void ResetInCircuit()
        {
            InCircuit = false;
        }

        public LogicElement? GetInput()
        {
            return input;
        }

        public virtual void SetInput(Connector _input)
        {
            inputConnector = _input;
            input = _input.GetInput();
            OnGetInput.Invoke(inputConnector);
        }

        public virtual void RemoveInput()
        {
            Debug.WriteLine("Number " + id + " lost its input");
            input = null;
            OnLoseInput.Invoke();
        }
    }
}
