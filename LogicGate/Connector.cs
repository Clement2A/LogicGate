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

        public Connector? InputConnector => inputConnector;

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
                SetInput(_connector);
                _connector.OnLoseInput += RemoveInput;
            }
            else if(input == null)
            {
                _connector.OnGetInput += SetInput;
            }
        }

        public void RemoveConnector(Connector _connector)
        {
            connectors.Remove(_connector);
            if(input != null && inputConnector == _connector)
            {
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
            if(input != null)
                return;
            foreach (Connector _connector in connectors) 
            {
                _connector.OnGetInput -= SetInput;
            }
            inputConnector = _input;
            input = _input.GetInput();
            Debug.WriteLine("Number " + id + " has a new input: " + _input.id);
            OnGetInput.Invoke(this);
        }

        public virtual void RemoveInput()
        {
            if (input == null)
                return;
            Debug.WriteLine("Number " + id + " has lost its input");
            foreach (Connector _connector in connectors)
            {
                _connector.OnGetInput += SetInput;
                Debug.WriteLine("Number " + id + " looks at " + _connector.id + " for a new input");
            }
            inputConnector!.OnLoseInput -= RemoveInput;
            OnLoseInput.Invoke();
            inputConnector = null;
            input = null;
        }
    }
}
