using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LogicGate
{
    internal class MoveableCanvas
    {
        Canvas staticCanvas;
        Canvas movingCanvas;

        public MoveableCanvas()
        {
            staticCanvas = new Canvas();
            movingCanvas = new Canvas();
            staticCanvas.Children.Add(staticCanvas);
        }
    }
}
