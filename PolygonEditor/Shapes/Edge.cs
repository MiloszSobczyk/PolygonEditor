using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Edge : Shape
    {
        private static readonly double accuracy = 3.0;
        public Vertex Vertex1;
        public Vertex Vertex2;
        public Point ClickPoint { get; set; }
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
            Console.WriteLine();
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Pen pen)
        {
            e.Graphics.DrawLine(pen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);
        }
        public double CalculateDistanceFromEdge(Point point)
        {
            if (point.X < Math.Min(Vertex1.X, Vertex2.X) - accuracy 
                || point.X > Math.Max(Vertex1.X, Vertex2.X) + accuracy)
                return int.MaxValue;
            if (point.Y < Math.Min(Vertex1.Y, Vertex2.Y) - accuracy 
                || point.Y > Math.Max(Vertex1.Y, Vertex2.Y) + accuracy)
                return int.MaxValue;
            double A = point.X - Vertex1.X;
            double B = point.Y - Vertex1.Y;
            double C = Vertex2.X - Vertex1.X;
            double D = Vertex2.Y - Vertex1.Y;

            double dot = A * (-D) + B * C;
            double len_sq = D * D + C * C;

            return Math.Abs(dot) / Math.Sqrt(len_sq);
        }
        public double CalculateDistanceFromEdge2(Point point)
        {
            double a = (Vertex1.Y - Vertex2.Y) / (Vertex1.X - Vertex2.X);
            double b = Vertex1.Y - Vertex1.X * a;
            double aInverse = 1 / a; // check if a == 0 or a == inf
            return 0.0;
        }
    }
}
