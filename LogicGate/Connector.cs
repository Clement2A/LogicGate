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
    internal class Connector : DraggableElement
    {
        Ellipse connectorShape;


        public Connector(DesignGrid _grid) : base(_grid)
        {
            Debug.WriteLine("Connector construct");
            Ellipse _moveHandle = new Ellipse
            {
                Fill = ColorLibrary.ConnectorColor,
                Height = 20,
                Width = 20,
                Margin = new(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            SetDragElement(_moveHandle);
            connectorShape = new Ellipse
            {
                Fill = ColorLibrary.InactiveConnectorColor,
                Height = 10,
                Width = 10,
                Margin = new(5, 5, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Panel.SetZIndex(ElementGrid, 1);
            connectorShape.PreviewMouseLeftButtonDown += OnConnectorClick;
            AddElement(connectorShape);
            CanBeDragged = false;
        }

        void OnConnectorClick(object _sender, MouseButtonEventArgs _e)
        {
            grid.OnMouseMove += OnCreateWire;
        }

        void OnCreateWire(Point _position)
        {
            grid.OnMouseMove -= OnCreateWire;
            Debug.WriteLine("Create a wire and move it around");
            Wire _wire = new Wire(this);
        }
    }
}
