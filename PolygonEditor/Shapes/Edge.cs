using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Edge : Shape
    {
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
    }
}
