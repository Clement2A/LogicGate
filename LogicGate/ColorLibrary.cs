using System.Windows.Media;

namespace LogicGate
{
    internal class ColorLibrary
    {
        static public Brush GetWireColor() { return new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff)); }
        static public Brush GetInvalidWireColor() { return new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00)); }
        static public Brush GetPoweredWireColor() { return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00)); }
        static public Brush GetOnColor() { return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00)); }
        static public Brush GetOffColor() { return new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00)); }
    }
}
