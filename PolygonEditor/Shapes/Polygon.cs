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
                RotateVertices();
                CalculateOffset2();
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
        private void RotateVertices()
        {
            if (!IsClockwiseOffset()) return;
            List<Vertex> copy = new List<Vertex>();
            foreach(Vertex v in Vertices) copy.Add(v);
            Vertices.Clear();
            Edges.Clear();
            Vertices.Add(copy.Last());
            for(int i = copy.Count - 2; i >= 0; --i)
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
            if(Vertices.Count <= 2)
            {
                Vertices.RemoveAt(index);
                Edges.Clear();
                if(Vertices.Count != 0) CalculateCenterOfMass();
                Finished = false;
                InflatedVertices.Clear();
                InflatedEdges.Clear();
                return;
            }
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
            CalculateOffset2();
        }
        public void Move(int dX, int dY)
        {
            foreach (Vertex vertex in this.Vertices)
            {
                vertex.X += dX;
                vertex.Y += dY;
            }
            CalculateCenterOfMass();
            CalculateOffset2();
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
            CalculateOffset2();
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
        public void CalculateOffset2()
        {
            InflatedVertices.Clear();
            InflatedEdges.Clear();

            Vertex prev = Vertices.Last();
            Point[] vectors = new Point[Vertices.Count];
            (Point start, Point end)[] edges = new (Point start, Point end)[Vertices.Count];
            List<Point> temp = new List<Point>();
            double v1, v2, a, b, frac;
            int counter = 0, square = offsetDistance * offsetDistance;
            foreach (Vertex vertex in Vertices)
            {
                v1 = vertex.X - prev.X;
                v2 = vertex.Y - prev.Y;
                if (v1 == 0)
                {
                    a = offsetDistance;
                    b = 0;
                }
                else if (v2 == 0)
                {
                    a = 0;
                    b = offsetDistance;
                }
                else
                {
                    frac = v2 / v1;
                    b = Math.Sqrt(square / (frac * frac + 1));
                    a = Math.Abs(frac * b);
                }
                if (v1 > 0)
                {
                    if (v2 < 0) a *= -1;
                    b *= -1;
                }
                else if (v2 < 0) a *= -1;
                vectors[counter] = new Point((int)a, (int)b);
                ++counter;
                prev = vertex;
            }
            prev = Vertices.Last();
            counter = 0;
            foreach (Vertex vertex in Vertices)
            {
                edges[counter] = (new Point(prev.X + vectors[counter].X, prev.Y + vectors[counter].Y), new Point(vertex.X + vectors[counter].X, vertex.Y + vectors[counter].Y));
                prev = vertex;
                ++counter;
            }
            (Point start, Point end) prevEdge = edges[counter - 1];
            foreach ((Point start, Point end) edge in edges)
            {
                if ((prevEdge.end.X != edge.start.X) || (prevEdge.end.Y != edge.start.Y)) 
                    temp.Add(IntersectionPoint(prevEdge, edge));
                prevEdge = edge;
            }
            foreach (Point vertex in temp)
                InflatedVertices.Add(new Vertex(vertex.X, vertex.Y));
            ResolveSelfIntersections(); // delete if it is bugged
            for(int i = 0; i < InflatedVertices.Count; ++i)
                InflatedEdges.Add(new Edge(InflatedVertices[i], InflatedVertices[(i + 1) % InflatedVertices.Count]));

            

            //if (offsetDistance > 3)
            //{
            //    Vertex prev = Vertices.Last();
            //    Point[] vectors = new Point[Vertices.Count];
            //    (Point start, Point end)[] edges = new (Point start, Point end)[Vertices.Count];
            //    Point[] temp = new Point[Vertices.Count];
            //    double v1, v2, a, b, frac;
            //    int counter = 0, square = (int)((offsetDistance - 1.5) * (offsetDistance - 1.5));
            //    foreach (Vertex vertex in Vertices)
            //    {
            //        v1 = vertex.X - prev.X;
            //        v2 = vertex.Y - prev.Y;
            //        if (v1 == 0)
            //        {
            //            a = (offsetDistance - 1.5);
            //            b = 0;
            //        }
            //        else if (v2 == 0)
            //        {
            //            a = 0;
            //            b = (offsetDistance - 1.5);
            //        }
            //        else
            //        {
            //            frac = v2 / v1;
            //            b = Math.Sqrt(square / (frac * frac + 1));
            //            a = Math.Abs(frac * b);
            //        }
            //        if (v1 > 0)
            //        {
            //            if (v2 < 0) a *= -1;
            //            b *= -1;
            //        }
            //        else if (v2 < 0) a *= -1;
            //        vectors[counter] = new Point((int)a, (int)b);
            //        ++counter;
            //        prev = vertex;
            //    }
            //    prev = Vertices.Last();
            //    counter = 0;
            //    foreach (Vertex vertex in Vertices)
            //    {
            //        edges[counter] = (new Point(prev.X + vectors[counter].X, prev.Y + vectors[counter].Y), new Point(vertex.X + vectors[counter].X, vertex.Y + vectors[counter].Y));
            //        prev = vertex;
            //        ++counter;
            //    }
            //    (Point start, Point end) prevEdge = edges[counter - 1];
            //    counter = 0;
            //    foreach ((Point start, Point end) line in edges)
            //    {
            //        if ((prevEdge.end.X != line.start.X) || (prevEdge.end.Y != line.start.Y)) temp[counter] = (IntersectionPoint(prevEdge, line));
            //        else temp[counter] = new Point(0, 0);
            //        prevEdge = line;
            //        ++counter;
            //    }
            //    List<(Point, Point)> points = new List<(Point, Point)>();
            //    for (int i = 0; i < temp.Length; ++i) 
            //        if (temp[i].X != 0 || temp[i].Y != 0) 
            //            points.Add((temp[i], Vertices.ElementAt(i).Point));
            //    Console.WriteLine();
            //}
        }
        private Point IntersectionPoint((Point, Point) prevLine, (Point, Point) line)
        {
            int x12 = prevLine.Item1.X - prevLine.Item2.X;
            int x34 = line.Item1.X - line.Item2.X;
            int y12 = prevLine.Item1.Y - prevLine.Item2.Y;
            int y34 = line.Item1.Y - line.Item2.Y;
            int xy12 = prevLine.Item1.X * prevLine.Item2.Y - prevLine.Item1.Y * prevLine.Item2.X;
            int xy34 = line.Item1.X * line.Item2.Y - line.Item1.Y * line.Item2.X;
            int frac = x12 * y34 - y12 * x34;
            if (frac == 0) return new Point(0, 0);
            return new Point((xy12 * x34 - x12 * xy34) / frac, (xy12 * y34 - xy34 * y12) / frac);
        }
    }
}
