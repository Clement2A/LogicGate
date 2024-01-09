using System.Windows.Media;

namespace LogicGate
{
    internal class ColorLibrary
    {
        static public Brush WireColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush InvalidWireColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));
        static public Brush PoweredWireColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00));
        static public Brush OnColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00));
        static public Brush OffColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));
        static public Brush ConnectorColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush InactiveConnectorColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
        static public Brush EoLConnectorColor => new SolidColorBrush(Color.FromArgb(0xff, 0xBD, 0x7A, 0x29));
        static public Brush ConnectedConnectorColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
    }
}
