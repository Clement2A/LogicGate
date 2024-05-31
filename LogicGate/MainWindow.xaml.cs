using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogicGate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Action<Point>? OnMouseMoveEvent;
        public Action? OnMouseUpEvent;
        public Action? OnMouseRightClickDown;
        public Action? OnMouseRightClickDrag;
        public Action? OnMouseRightClickStopDrag;
        public Action? OnMouseRightClickUp;
        public Point currentObjectOffset = new(0,0);

        DesignGrid designGrid = new DesignGrid();

        public MainWindow()
        {
            InitializeComponent();
            mainGrid.Children.Add(designGrid.staticCanvas);
            mainGrid.MouseUp += CanvasOnMouseUp;

        }

        private void CanvasOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUpEvent?.Invoke();
        }

        private void CreateInput(object sender, RoutedEventArgs e)
        {
            new LogicInput(designGrid);
        }

        private void CreateOutput(object sender, RoutedEventArgs e)
        {
            new LogicOutput(designGrid);
        }

        private void CreateAndGate(object sender, RoutedEventArgs e)
        {
            new LogicANDGate(designGrid);
        }

        private void CreateNandGate(object sender, RoutedEventArgs e)
        {
            new LogicNANDGate(designGrid);
        }

        private void CreateOrGate(object sender, RoutedEventArgs e)
        {
            new LogicORGate(designGrid);
        }

        private void CreateNorGate(object sender, RoutedEventArgs e)
        {
            new LogicNORGate(designGrid);
        }

        private void CreateXorGate(object sender, RoutedEventArgs e)
        {
            new LogicXORGate(designGrid);
        }

        private void CreateXnorGate(object sender, RoutedEventArgs e)
        {
            new LogicXNORGate(designGrid);
        }

        private void CreateNotGate(object sender, RoutedEventArgs e)
        {
            new LogicNOTGate(designGrid);
        }
    }
}
