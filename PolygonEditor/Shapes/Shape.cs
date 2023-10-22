using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public abstract class Shape
    {
        public abstract void Move(int dX, int dY);
    }
}
