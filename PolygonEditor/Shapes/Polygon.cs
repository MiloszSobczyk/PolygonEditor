using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
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
            CalculateOffset();
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
                InflatedVertices.Clear();
                InflatedEdges.Clear();
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
        public void Draw(Bitmap bitmap, PaintEventArgs e, bool useBresenham = false)
        {
            if(Hovered || Selected)
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
            if (Vertices.Count <= 2) return;
            foreach (Vertex vertex in this.InflatedVertices)
                vertex.Draw(bitmap, e, brushes["red"]);
            foreach (Edge edge in this.InflatedEdges)
                edge.Draw(bitmap, e, false);
        }
        public bool IsClockwiseOffset()
        {
            int sum = 0;
            for(int i = 0; i < this.Vertices.Count; i++)
            {
                Vertex current = Vertices[i];
                Vertex next = Vertices[(i + 1) % Vertices.Count];

                sum += (next.X - current.X) * (next.Y + current.Y);
            }
            return sum > 0;
        }
        public bool IsOnConvexHull(Vertex vertex, List<Vertex> convexHull)
        {
            return convexHull.Any(c => c.Point == vertex.Point);
        }
        public Point CalculateIntersestionPoint(Point p1, Point p2, Point p3, Point p4)
        {
            float m1 = (float)(p2.Y - p1.Y) / (p2.X - p1.X);
            float m2 = (float)(p4.Y - p3.Y) / (p4.X - p3.X);

            float x = (m1 * p1.X - m2 * p3.X + p3.Y - p1.Y) / (m1 - m2);
            float y = m1 * (x - p1.X) + p1.Y;

            return new Point((int)x, (int)y);
        }
        public bool DoLineSegmentsIntersect(Point p1, Point p2, Point p3, Point p4)
        {
            int x1 = p1.X;
            int y1 = p1.Y;
            int x2 = p2.X;
            int y2 = p2.Y;
            int x3 = p3.X;
            int y3 = p3.Y;
            int x4 = p4.X;
            int y4 = p4.Y;

            int crossProduct1 = (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);
            int crossProduct2 = (x2 - x1) * (y4 - y1) - (y2 - y1) * (x4 - x1);

            if ((crossProduct1 > 0 && crossProduct2 < 0) || (crossProduct1 < 0 && crossProduct2 > 0))
            {
                int crossProduct3 = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
                int crossProduct4 = (x4 - x3) * (y2 - y3) - (y4 - y3) * (x2 - x3);

                if ((crossProduct3 > 0 && crossProduct4 < 0) || (crossProduct3 < 0 && crossProduct4 > 0))
                    return true;
            }
            return false;
        }
        public void ResolveSelfIntersections()
        {
            for (int i = 0; i < InflatedVertices.Count; i++)
            {
                Vertex vertexA = InflatedVertices[i];
                Vertex vertexB = InflatedVertices[(i + 1) % InflatedVertices.Count];

                for (int j = i + 2; j < InflatedVertices.Count; j++)
                {
                    Vertex vertexC = InflatedVertices[j];
                    Vertex vertexD = InflatedVertices[(j + 1) % InflatedVertices.Count];

                    if (DoLineSegmentsIntersect(vertexA.Point, vertexB.Point, vertexC.Point, vertexD.Point))
                    {
                        Point intersectionPoint = CalculateIntersestionPoint(vertexA.Point, vertexB.Point, vertexC.Point, vertexD.Point);

                        Vertex newVertex1 = new Vertex(intersectionPoint);
                        newVertex1.Neighbors[0].vertex = vertexA;
                        newVertex1.Neighbors[1].vertex = vertexC;
                        Vertex newVertex2 = new Vertex(intersectionPoint);
                        newVertex2.Neighbors[0].vertex = vertexB;
                        newVertex2.Neighbors[1].vertex = vertexD;

                        vertexA.Neighbors[1].vertex = newVertex1;
                        vertexC.Neighbors[0].vertex = newVertex1;
                        vertexB.Neighbors[1].vertex = newVertex2;
                        vertexD.Neighbors[0].vertex = newVertex2;

                        InflatedVertices.RemoveRange(i + 1, j - i);

                        InflatedVertices.Insert(i, newVertex1);
                        InflatedVertices.Insert(i + 1, newVertex2);

                        ResolveSelfIntersections();
                    }
                }
            }
        }
        public void CalculateOffset()
        {
            InflatedVertices.Clear();
            InflatedEdges.Clear();
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertex currentVertex = Vertices[i];
                Vertex prevVertex = Vertices[(i - 1 + Vertices.Count) % Vertices.Count];
                Vertex nextVertex = Vertices[(i + 1) % Vertices.Count];

                Vector2 edge = new Vector2(nextVertex.Point.X - currentVertex.Point.X, nextVertex.Point.Y - currentVertex.Point.Y);
                Vector2 prevEdge = new Vector2(currentVertex.Point.X - prevVertex.Point.X, currentVertex.Point.Y - prevVertex.Point.Y);

                Vector2 edgeNormal = new Vector2(-edge.Y, edge.X);
                Vector2 prevEdgeNormal = new Vector2(-prevEdge.Y, prevEdge.X);


                if (IsClockwiseOffset())
                {
                    edgeNormal = Vector2.Normalize(edgeNormal);
                    prevEdgeNormal = Vector2.Normalize(prevEdgeNormal);
                }
                else
                {
                    edgeNormal = Vector2.Normalize(-edgeNormal);
                    prevEdgeNormal = Vector2.Normalize(-prevEdgeNormal);
                }


                Vector2 offsetVector = edgeNormal + prevEdgeNormal;
                offsetVector = Vector2.Normalize(offsetVector);

                Point offsetPoint = new Point(
                    (int)(currentVertex.Point.X + offsetVector.X * offsetDistance),
                    (int)(currentVertex.Point.Y + offsetVector.Y * offsetDistance));

                bool isConvexHullVertex = IsOnConvexHull(currentVertex, Vertices);

                if (isConvexHullVertex) InflatedVertices.Add(new Vertex(offsetPoint));
                else InflatedVertices.Add(currentVertex);
            }

            for (int i = 0; i < InflatedVertices.Count; i++)
            {
                int prevIndex = (i - 1 + InflatedVertices.Count) % InflatedVertices.Count;
                int nextIndex = (i + 1) % InflatedVertices.Count;

                InflatedVertices[i].Neighbors[0].vertex = InflatedVertices[prevIndex];
                InflatedVertices[i].Neighbors[1].vertex = InflatedVertices[nextIndex];
            }

            ResolveSelfIntersections();
            for(int i = 0; i < InflatedVertices.Count; ++i)
                InflatedEdges.Add(new Edge(InflatedVertices[i], InflatedVertices[(i + 1) % InflatedVertices.Count]));
        }
    }
}
