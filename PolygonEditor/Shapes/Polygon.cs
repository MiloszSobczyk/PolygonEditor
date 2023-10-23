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
        public int VertexCount { get; private set; } = 0;
        public int EdgeCount { get; private set; } = 0;
        public Vertex RecentVertex { get; private set; }
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public Polygon(Vertex firstVertex)
        {
            Edges = new List<Edge>();
            Vertices = new List<Vertex>
            {
                firstVertex
            };
            ++VertexCount;
            RecentVertex = firstVertex;
        }
        public void AddVertex(int x, int y, bool connectToFirst = false)
        {
            if (connectToFirst)
            {
                ++EdgeCount;
                Edges.Add(new Edge(RecentVertex, Vertices[0]));
                Vertices[0].Selected = false;
            }
            else if(!Vertices.Any(v => v.X == x && v.Y == y))
            {
                ++VertexCount;
                ++EdgeCount;
                Vertices.Add(new Vertex(x, y));
                Edges.Add(new Edge(RecentVertex, Vertices[VertexCount - 1]));
                RecentVertex = Vertices[VertexCount - 1];
            }
        }
        // For now removes only last vertex. Returns true if there are no more vertices left
        public bool RemoveVertex() 
        {
            if(VertexCount == 1) return true;
            Edges.RemoveAt(--EdgeCount);
            Vertices.RemoveAt(--VertexCount);
            RecentVertex = Vertices.Last();
            return false;
        }
        public override void Move(int dX, int dY)
        {
            foreach (Vertex vertex in this.Vertices)
                vertex.Move(dX, dY);
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            foreach (Vertex vertex in this.Vertices)
                vertex.Draw(bitmap, e);
            foreach (Edge edge in this.Edges)
                edge.Draw(bitmap, e); ;
        }
    }
}
