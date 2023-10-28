using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolygonEditor.Shapes
{
    public enum Constraint
    {
        None = -1,
        Horizontal = 0,
        Vertical = 1,
    }
    public class Edge : Shape
    {
        private static readonly double accuracy = 3.0;
        private static readonly Pen hoveredPen = new Pen(Color.Blue, 3);
        private static readonly Pen selectedPen = new Pen(Color.Green, 3);
        public bool Hovered { get; set; } = false;
        public bool Selected { get; set; } = false;
        public Vertex? Vertex1 { get; set; }
        public Vertex? Vertex2 { get; set; }
        public Point ClickPoint { get; set; }
        public Vertex? FromVertex { get; set; }
        public Constraint Constraint { get; set; }
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Constraint = Constraint.None;
        }
        public override void Move(int dX, int dY)
        {
            this.Vertex1!.Move(dX, dY);
            this.Vertex2!.Move(dX, dY);
            Console.WriteLine();
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Pen pen)
        {
            if(this.Hovered)
                e.Graphics.DrawLine(hoveredPen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);
            else if(this.Selected)
                e.Graphics.DrawLine(selectedPen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);
            else
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
            double a = (Vertex1!.Y - Vertex2!.Y) / (Vertex1.X - Vertex2.X);
            double b = Vertex1.Y - Vertex1.X * a;
            double aInverse = 1 / a; // check if a == 0 or a == inf
            return 0.0;
        }

        public void AddConstraint(Constraint constraint)
        {
            if(Vertex1 == null || Vertex2 == null) return;
            Constraint = constraint;
            if(Vertex1 == Vertex2.Neighbors[0].vertex)
            {
                Vertex2.Neighbors[0] = (Vertex1, constraint);
                Vertex1.Neighbors[1] = (Vertex2, constraint);
            }
            else
            {
                Vertex2.Neighbors[1] = (Vertex1, constraint);
                Vertex1.Neighbors[0] = (Vertex2, constraint);
            }
        }
    }
}
