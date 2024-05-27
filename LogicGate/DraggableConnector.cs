using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicGate
{
    class DraggableConnector : Connector
    {
        public DraggableConnector(DesignGrid _grid) : base(_grid)
        {
            MakeElementDraggable(moveHandle);
            moveHandle.Cursor = Cursors.SizeAll;
            connectorShape.MouseRightButtonDown += (s, e) => { DeleteElement(); };
            moveHandle.MouseRightButtonDown += (s, e) => { DeleteElement(); };
        }
    }
}
