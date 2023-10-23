using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Edge : Shape
    {
        public static readonly Pen blackPen = new Pen(Color.Black);
        private Vertex vertex1;
        private Vertex vertex2;
        public Vertex Vertex1
        {
            get => this.vertex1;
            set
            {
                this.vertex1.RemoveEdge(this);
                this.vertex1 = value;
                this.vertex1.AddEdge(this);
            }
        }
        public Vertex Vertex2
        {
            get => this.vertex2;
            set
            {
                this.vertex2.RemoveEdge(this);
                this.vertex2 = value;
                this.vertex2.AddEdge(this);
            }
        }
        public Vertex? FromVertex { get; set; }
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
        public override void Move(int dX, int dY)
        {
            this.Vertex1.Move(dX, dY);
            this.Vertex2.Move(dX, dY);
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            // possibly change pen to static field in a class to avoid
            // creating new one every time
            e.Graphics.DrawLine(blackPen, vertex1.X, vertex1.Y, vertex2.X, vertex2.Y);
        }

    }
}
