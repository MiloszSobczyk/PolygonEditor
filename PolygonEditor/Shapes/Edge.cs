using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Edge : Shape
    {
        public Vertex Vertex1;
        public Vertex Vertex2;
        public Vertex? FromVertex { get; set; }
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
        }
        public override void Move(int dX, int dY)
        {
            this.Vertex1.Move(dX, dY);
            this.Vertex2.Move(dX, dY);
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Pen pen)
        {
            e.Graphics.DrawLine(pen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);
        }

    }
}
