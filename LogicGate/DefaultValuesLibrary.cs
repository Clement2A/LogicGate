﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LogicGate
{
    internal class DefaultValuesLibrary
    {
        #region Wire
        static public Brush WireColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush WireInvalidColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));
        static public Brush WirePoweredColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00));
        static public double WireThickness => 3;
        #endregion

        #region Connector
        static public Brush ConnectorHandleColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public Brush ConnectorInactiveColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
        static public Brush ConnectorEoLColor => new SolidColorBrush(Color.FromArgb(0xff, 0xBD, 0x7A, 0x29));
        static public Brush ConnectorConnectedColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
        static public double ConnectorHandleSize => 20;
        static public Thickness ConnectorHandleOffset => new Thickness(0);
        static public double ConnectorShapeSize => 10;
        static public Thickness ConnectorShapeOffset => new Thickness(ConnectorShapeSize/2, ConnectorShapeSize/2, 0, 0);

        #endregion
        #region Misc
        static public Brush OnColor => new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0xff, 0x00));
        static public Brush OffColor => new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));
        #endregion
    }
}