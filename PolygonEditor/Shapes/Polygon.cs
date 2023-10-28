using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Polygon
    {
        public static readonly Dictionary<string, Pen> pens =
            new Dictionary<string, Pen>()
            {
                { "black", new Pen(Color.Black) },
                { "blue", new Pen(Color.Blue) },
                { "red", new Pen(Color.Red) },
            };
        public static readonly Dictionary<string, Brush> brushes =
            new Dictionary<string, Brush>()
            {
                { "black", new SolidBrush(Color.Black) },
                { "blue", new SolidBrush(Color.Blue) },
                { "red", new SolidBrush(Color.Red) },
            };
        public Point CenterOfMass { get; private set; }
        public int VertexCount { get; private set; } = 0;
        public int EdgeCount { get; private set; } = 0;
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public bool Selected { get; set; } = false;
        public bool Hovered { get; set; } = false;
        public bool Finished { get; private set; } = false;
        public Polygon(Vertex firstVertex)
        {
            Edges = new List<Edge>();
            Vertices = new List<Vertex>
            {
                firstVertex
            };
            ++VertexCount;
            CenterOfMass = firstVertex.Point;
        }
        public void AddVertex(int x, int y, bool connectToFirst = false)
        {
            if (connectToFirst)
            {
                ++EdgeCount;
                Edges.Add(new Edge(Vertices.Last(), Vertices[0]));
                Finished = true;
            }
            else if(!Vertices.Any(v => v.X == x && v.Y == y))
            {
                ++VertexCount;
                ++EdgeCount;
                Vertex newVertex = new Vertex(x, y);
                Vertices.Add(newVertex);
                Edges.Add(new Edge(Vertices[Vertices.Count - 2], newVertex));
                CalculateCenterOfMass();
            }
        }
        public void CalculateCenterOfMass()
        {
            int xSum = 0;
            int ySum = 0;
            Vertices.ForEach(v => { xSum += v.X; ySum += v.Y; });
            CenterOfMass = new Point(xSum / Vertices.Count, ySum / Vertices.Count);
        }
        public void RemoveVertex(Vertex vertex)
        {
            if(Vertices.Count <= 2)
            {
                Vertices.Remove(vertex);
                Edges.Clear();
                if(Vertices.Count != 0) CalculateCenterOfMass();
                Finished = false;
                return;
            }
            int index = Vertices.IndexOf(vertex);
            if (index == -1) return;
            Vertex beforeVertex = index == 0 ? Vertices.Last() : Vertices[index - 1];
            Vertex afterVertex = index == Vertices.Count - 1 ? Vertices.First() : Vertices[index + 1];
            Edge beforeEdge = index == 0 ? Edges.Last() : Edges[index - 1];
            if(Finished)
            {
                Edge afterEdge = Edges[index];
                Edges.Insert(index == 0 ? Edges.Count - 1 : index - 1, new Edge(beforeVertex, afterVertex));
                Edges.Remove(afterEdge);
            }
            Edges.Remove(beforeEdge);
            Vertices.RemoveAt(index);
            CalculateCenterOfMass();
        }
        public void Move(int dX, int dY)
        {
            foreach (Vertex vertex in this.Vertices)
                vertex.Move(dX, dY);
            CalculateCenterOfMass();
        }
        public void AddInBetween(Edge selectedEdge)
        {
            int edgeIndex = Edges.IndexOf(selectedEdge);
            if(edgeIndex == -1) return;

            Vertex vertex1 = selectedEdge.Vertex1!;
            Vertex vertex2 = selectedEdge.Vertex2!;
            
            Vertex newVertex = new Vertex((vertex1.X + vertex2.X) / 2, (vertex1.Y + vertex2.Y) / 2);
            Edge newEdge1 = new Edge(vertex1, newVertex);
            Edge newEdge2 = new Edge(newVertex, vertex2);

            Vertices.Insert(edgeIndex + 1, newVertex);
            Edges.Insert(edgeIndex, newEdge1);
            Edges.Insert(edgeIndex + 1, newEdge2);
            Edges.Remove(selectedEdge);

            CalculateCenterOfMass();
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            if(Hovered || Selected)
            {
                foreach (Vertex vertex in this.Vertices)
                    vertex.Draw(bitmap, e, brushes["blue"]);
                foreach (Edge edge in this.Edges)
                    edge.Draw(bitmap, e, pens["blue"]);
            }
            else
            {
                foreach (Vertex vertex in this.Vertices)
                    vertex.Draw(bitmap, e, brushes["black"]);
                foreach (Edge edge in this.Edges)
                    edge.Draw(bitmap, e, pens["black"]);
            }
            e.Graphics.FillEllipse(brushes["red"], CenterOfMass.X - 5, CenterOfMass.Y - 5,
                10, 10);
        }
    }
}
