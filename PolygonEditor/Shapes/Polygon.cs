using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        public static int offsetDistance = 5;
        public Point CenterOfMass { get; private set; }
        public int VertexCount { get; private set; } = 0;
        public int EdgeCount { get; private set; } = 0;
        public List<Edge> Edges { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public List<Edge> InflatedEdges { get; private set; }
        public List<Vertex> InflatedVertices { get; private set; }
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
            InflatedEdges = new List<Edge>();
            InflatedVertices = new List<Vertex>();
        }
        public void AddVertex(int x, int y, bool connectToFirst = false)
        {
            if (connectToFirst)
            {
                ++EdgeCount;
                Edges.Add(new Edge(Vertices.Last(), Vertices[0]));
                Finished = true;
                RotateVertices();
                CalculateOffset();
            }
            else if (!Vertices.Any(v => v.X == x && v.Y == y))
            {
                ++VertexCount;
                ++EdgeCount;
                Vertex newVertex = new Vertex(x, y);
                Vertices.Add(newVertex);
                Edges.Add(new Edge(Vertices[Vertices.Count - 2], newVertex));
                CalculateCenterOfMass();
            }
        }
        private void RotateVertices()
        {
            if (!IsClockwiseOffset()) return;
            List<Vertex> copy = new List<Vertex>();
            foreach (Vertex v in Vertices) copy.Add(v);
            Vertices.Clear();
            Edges.Clear();
            Vertices.Add(copy.Last());
            for (int i = copy.Count - 2; i >= 0; --i)
            {
                Vertex vertex = copy[i];
                AddVertex(vertex.X, vertex.Y);
            }
            AddVertex(0, 0, true);
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
            int index = Vertices.IndexOf(vertex);
            if (Vertices.Count == 3)
            {
                Vertices.RemoveAt(index);
                Edges.RemoveAt(index);
                Edges.RemoveAt(index == 0 ? Edges.Count - 1 : index - 1);
                Finished = false;
                return;
            }
            if (Vertices.Count <= 2)
            {
                Vertices.RemoveAt(index);
                Edges.Clear();
                if (Vertices.Count != 0) CalculateCenterOfMass();
                Finished = false;
                InflatedVertices.Clear();
                InflatedEdges.Clear();
                return;
            }
            if (index == -1) return;
            Vertex beforeVertex = index == 0 ? Vertices.Last() : Vertices[index - 1];
            Vertex afterVertex = index == Vertices.Count - 1 ? Vertices.First() : Vertices[index + 1];
            Edge beforeEdge = index == 0 ? Edges.Last() : Edges[index - 1];
            if (Finished)
            {
                Edge afterEdge = Edges[index];
                Edges.Insert(index == 0 ? Edges.Count - 1 : index - 1, new Edge(beforeVertex, afterVertex));
                Edges.Remove(afterEdge);
            }
            Edges.Remove(beforeEdge);
            Vertices.RemoveAt(index);
            CalculateCenterOfMass();
            CalculateOffset();
        }
        public void Move(int dX, int dY)
        {
            foreach (Vertex vertex in this.Vertices)
            {
                vertex.X += dX;
                vertex.Y += dY;
            }
            CalculateCenterOfMass();
            CalculateOffset();
        }
        public void AddInBetween(Edge selectedEdge)
        {
            int edgeIndex = Edges.IndexOf(selectedEdge);
            if (edgeIndex == -1) return;

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
            CalculateOffset();
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, bool useBresenham = false)
        {
            if (Hovered || Selected)
            {
                foreach (Vertex vertex in this.Vertices)
                    vertex.Draw(bitmap, e, brushes["blue"]);
                foreach (Edge edge in this.Edges)
                    edge.Draw(bitmap, e, true, useBresenham);
            }
            else
            {
                foreach (Vertex vertex in this.Vertices)
                    vertex.Draw(bitmap, e, brushes["black"]);
                foreach (Edge edge in this.Edges)
                    edge.Draw(bitmap, e, false, useBresenham);
            }

            e.Graphics.FillEllipse(brushes["red"], CenterOfMass.X - 5, CenterOfMass.Y - 5,
                10, 10);
        }
        public void DrawOffset(Bitmap bitmap, PaintEventArgs e)
        {
            if (Vertices.Count <= 3 || !Finished) return;
            foreach (Vertex vertex in this.InflatedVertices)
                vertex.Draw(bitmap, e, brushes["red"]);
            foreach (Edge edge in this.InflatedEdges)
                edge.Draw(bitmap, e, false);
        }
        public bool IsClockwiseOffset()
        {
            int sum = 0;
            for (int i = 0; i < this.Vertices.Count; i++)
            {
                Vertex current = Vertices[i];
                Vertex next = Vertices[(i + 1) % Vertices.Count];

                sum += (next.X - current.X) * (next.Y + current.Y);
            }
            return sum > 0;
        }
        public double length(Vertex p1, Vertex p2)
        {
            Vertex vertex = new Vertex(p2.X - p1.X, p2.Y - p1.Y);
            return Math.Sqrt(vertex.X * vertex.X + vertex.Y * vertex.Y);
        }
        public double CrossProduct(Vertex p1, Vertex p2, Vertex p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
        }

        public bool CheckForIntersection(Vertex p1, Vertex p2, Vertex p3, Vertex p4)
        {
            double cp1 = CrossProduct(p1, p2, p3);
            double cp2 = CrossProduct(p1, p2, p4);
            double cp3 = CrossProduct(p3, p4, p1);
            double cp4 = CrossProduct(p3, p4, p2);

            if (((cp1 > 0 && cp2 < 0) || (cp1 < 0 && cp2 > 0)) &&
                ((cp3 > 0 && cp4 < 0) || (cp3 < 0 && cp4 > 0)))
            {
                return true;
            }

            if (cp1 == 0 && cp2 == 0 && cp3 == 0 && cp4 == 0)
            {
                if (p1.X > p2.X) (p1, p2) = (p2, p1);
                if (p3.X > p4.X) (p3, p4) = (p4, p3);

                if (p1.X <= p4.X && p3.X <= p2.X &&
                    p1.Y <= p4.Y && p3.Y <= p2.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public Vertex FindIntersection(Vertex p1, Vertex p2, Vertex p3, Vertex p4)
        {
            double A1 = p2.Y - p1.Y;
            double B1 = p1.X - p2.X;
            double C1 = A1 * p1.X + B1 * p1.Y;

            double A2 = p4.Y - p3.Y;
            double B2 = p3.X - p4.X;
            double C2 = A2 * p3.X + B2 * p3.Y;

            double det = A1 * B2 - A2 * B1;

            double X = (C1 * B2 - C2 * B1) / det;
            double Y = (A1 * C2 - A2 * C1) / det;

            return new Vertex((int)X, (int)Y);
        }

        public void CalculateOffset()
        {
            List<Vertex> offsetPolygon = new List<Vertex>();
            for (int i = 0; i < Vertices.Count; i++)
            {
                int next = (i + 1) % Vertices.Count;
                int current = i;
                double vecLen = length(Vertices[current], Vertices[next]);
                int x = Vertices[current].X - Vertices[next].X;
                int y = Vertices[current].Y - Vertices[next].Y;
                offsetPolygon.Add(
                    new Vertex(
                    (int)(-y / vecLen * offsetDistance + Vertices[current].X),
                    (int)(x / vecLen * offsetDistance + Vertices[current].Y)));

                bool shouldAdd = true;
                if (offsetPolygon.Count > 1)
                    for (int j = 0; j < offsetPolygon.Count - 2; j++)
                    {
                        if (CheckForIntersection(offsetPolygon[j], offsetPolygon[j + 1],
                            offsetPolygon[offsetPolygon.Count - 2], offsetPolygon[offsetPolygon.Count - 1]))
                        {
                            Vertex intersectionPoint = FindIntersection(offsetPolygon[j], offsetPolygon[j + 1],
                                offsetPolygon[offsetPolygon.Count - 2], offsetPolygon[offsetPolygon.Count - 1]);
                            if (offsetPolygon.Count - j - 3 <= j + 2)
                            {
                                offsetPolygon.RemoveRange(j + 1, offsetPolygon.Count - 1 - (j + 1));
                                offsetPolygon.Insert(j + 1, intersectionPoint);
                            }
                            else
                            {
                                offsetPolygon.RemoveRange(0, j + 1);
                                offsetPolygon.RemoveRange(offsetPolygon.Count - 1, 1);
                                offsetPolygon.Add(intersectionPoint);
                                shouldAdd = false;
                            }
                            break;
                        }
                    }
                if (shouldAdd) offsetPolygon.Add(
                    new Vertex(
                    (int)(-y / vecLen * offsetDistance + Vertices[next].X),
                     (int)(x / vecLen * offsetDistance + Vertices[next].Y))
                    );


                for (int j = 0; j < offsetPolygon.Count - 2; j++)
                {
                    if (CheckForIntersection(offsetPolygon[j], offsetPolygon[j + 1],
                        offsetPolygon[offsetPolygon.Count - 2], offsetPolygon[offsetPolygon.Count - 1]))
                    {
                        Vertex intersectionPoint = FindIntersection(offsetPolygon[j], offsetPolygon[j + 1],
                            offsetPolygon[offsetPolygon.Count - 2], offsetPolygon[offsetPolygon.Count - 1]);

                        if (offsetPolygon.Count - j - 3 <= j + 2)
                        {
                            offsetPolygon.RemoveRange(j + 1, offsetPolygon.Count - 1 - (j + 1));
                            offsetPolygon.Insert(j + 1, intersectionPoint);
                        }
                        else
                        {
                            offsetPolygon.RemoveRange(0, j + 1);
                            offsetPolygon.RemoveRange(offsetPolygon.Count - 1, 1);
                            offsetPolygon.Add(intersectionPoint);
                        }
                        break;
                    }
                }
            }
            InflatedVertices = offsetPolygon;
            InflatedEdges.Clear();
            for(int i = 0; i < InflatedVertices.Count; ++i)
            {
                InflatedEdges.Add(new Edge(InflatedVertices[i], InflatedVertices[(i + 1) % InflatedVertices.Count]));
            }

        } 
    }
}
