using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Polygon : Shape
    {
        private int vertexCount = 0;
        public SortedDictionary<int, Edge> Edges { get; private set; }
        public SortedDictionary<int, Vertex> Vertices { get; private set; }
        public Polygon(Vertex firstVertex)
        {
            Edges = new SortedDictionary<int, Edge>();
            Vertices = new SortedDictionary<int, Vertex>();
            AddVertex(firstVertex);
        }
        public void AddVertex(Vertex vertex)
        {
            Vertices.Add(++vertexCount, vertex);
        }
        public void AddVertex(int x, int y)
        {
            Vertices.Add(++vertexCount, new Vertex(x, y));
        }
        public override void Move(int dX, int dY)
        {
            foreach (Vertex vertex in this.Vertices.Values)
                vertex.Move(dX, dY);
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            foreach (Vertex vertex in this.Vertices.Values)
                vertex.Draw(bitmap, e);
        }
    }
}
