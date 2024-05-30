using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LogicGate
{
    internal static class DefaultValuesLibrary
    {
        #region Wire
        static public Brush WireColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush WireInvalidColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));
        static public Brush WirePoweredColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00));
        static public double WireThickness => 3;
        static public Brush FlowOnColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xcc, 0x00));
        static public Brush FlowOffColor => new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0xff, 0x00));
        static public double FlowThickness => 8;
        static public double FlowDashSize => .5;
        static public double FlowSpaceSize => 4.5;
        static public DoubleCollection FlowDashArray => new DoubleCollection() { FlowDashSize, FlowSpaceSize };
        static public double FlowDashOffset => FlowDashSize + FlowSpaceSize;
        static public DoubleAnimation FlowAnimation => new DoubleAnimation(0, TimeSpan.FromSeconds(.75));
        #endregion

        #region Connector
        static public Brush ConnectorHandleColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush ConnectorInactiveColor => new SolidColorBrush(Color.FromArgb(0xff, 0xee, 0xaa, 0x11));
        static public Brush ConnectorBlockedColor => new SolidColorBrush(Color.FromArgb(0xff, 0x11, 0x11, 0x11));
        static public Brush ConnectorEoLColor => new SolidColorBrush(Color.FromArgb(0xff, 0xBD, 0x7A, 0x29));
        static public Brush ConnectorConnectedColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
        static public double ConnectorHandleSize => 20;
        static public Thickness ConnectorHandleOffset => new Thickness(0);
        static public double ConnectorShapeSize => 10;
        static public Thickness ConnectorShapeOffset => new Thickness(ConnectorShapeSize / 2, ConnectorShapeSize / 2, 0, 0);
        #endregion

        #region Logic
        static public Brush LogicStrokeColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush LogicOffColor => new SolidColorBrush(Color.FromArgb(0xff, 0x33, 0x33, 0x44));
        static public Brush LogicOnColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xcc, 0x00));
        static public double LogicStrokeThickness => 5;
        static public double InputSize => 50;
        static public Point OutputConnectorOffset => new Point(40, 15);
        static public Point OutputConnectorNGateOffset => new Point(60, 15);
        static public Point InputSingleConnectorOffset => new Point(-10, 15);
        static public Point InputDoubleTopConnectorOffset => new Point(-10, 25);
        static public Point InputDoubleDownConnectorOffset => new Point(-10, 5);
        static public Point InputDoubleTopConnectorXGateOffset => new Point(-25, 25);
        static public Point InputDoubleDownConnectorXGateOffset => new Point(-25, 5);
        #endregion

        #region Gate
        static public Brush GateFillColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Geometry GateANDData => Geometry.Parse("M 0 0 L 25 0 A 25 25 0 1 1 25 50 L 0 50 Z");
        static public Geometry GateNANDData => Geometry.Parse("M 0 0 L 25 0 A 25 25 0 1 1 25 50 L 0 50 Z M 50 25 A 5 5 0 1 1 50 25.01");
        static public Geometry GateORData => Geometry.Parse("M 0 0 L 10 0 Q 37.5 0 50 25 Q 37.5 50 6 50 L 0 50 Q 12 25 0 0 Z");
        static public Geometry GateNORData => Geometry.Parse("M 0 0 L 10 0 Q 37.5 0 50 25 Q 37.5 50 6 50 L 0 50 Q 12 25 0 0 Z M 50 25 A 5 5 0 1 1 50 25.01");
        static public Geometry GateXORData => Geometry.Parse("M 0 0 L 10 0 Q 37.5 0 50 25 Q 37.5 50 6 50 L 0 50 Q 12 25 0 0 Z M -12 0  Q 5 25 -12 50 L -12 50 Q 5 25 -12 0");
        static public Geometry GateXNORData => Geometry.Parse("M 0 0 L 10 0 Q 37.5 0 50 25 Q 37.5 50 6 50 L 0 50 Q 12 25 0 0 Z M -12 0  Q 5 25 -12 50 L -12 50 Q 5 25 -12 0 M 50 25 A 5 5 0 1 1 50 25.01");
        static public Geometry GateNOTData => Geometry.Parse("M 0 0 L 48 25 L 0 50 Z M 50 25 A 5 5 0 1 1 50 25.01");
        #endregion

        #region Misc
        static public Brush OnColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00));
        static public Brush OffColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));

        public static Brush GridBackground => new SolidColorBrush(Color.FromArgb(0x55, 0x55, 0x55, 0x55));
        public static Brush GridBorder => new SolidColorBrush(Color.FromArgb(0xff, 0x11, 0x11, 0x11));
        #endregion



    }
}
