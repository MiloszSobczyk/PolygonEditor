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
    public class Edge
    {
        private static readonly double accuracy = 3.0;
        private static readonly Pen hoveredPen = new Pen(Color.Blue, 3);
        private static readonly Pen selectedPen = new Pen(Color.Green, 3);
        private static readonly Pen constraintPen = new Pen(Color.Red, 2);
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
            UpdateConstraints(Constraint.None);
        }
        public bool UpdateConstraints(Constraint constraint)
        {
            return Functions.UpdateConstraints(Vertex1, Vertex2, constraint);
        }
        public void SetConstraint(Constraint constraint)
        {
            if(constraint == Constraint.None)
            {
                Constraint = constraint;
                return;
            }
            if (constraint == Vertex1.Neighbors[1].constraint 
                || constraint == Vertex2.Neighbors[0].constraint) return;

            Constraint = constraint;
            UpdateConstraints(constraint);
            if(constraint == Constraint.Horizontal)
                this.Vertex1.Y = this.Vertex2.Y;
            else
                this.Vertex1.X = this.Vertex2.X;
        }
        public void Move(int dX, int dY)
        {
            this.Vertex1!.MoveAtEdge(dX, dY, 0);
            this.Vertex2!.MoveAtEdge(dX, dY, 1);
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Pen pen)
        {
            if(Selected)
                e.Graphics.DrawLine(selectedPen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);
            else if(Hovered)
                e.Graphics.DrawLine(hoveredPen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);
            else
                e.Graphics.DrawLine(pen, Vertex1.X, Vertex1.Y, Vertex2.X, Vertex2.Y);

            Point middle = new Point((Vertex1.X + Vertex2.X) / 2, (Vertex1.Y + Vertex2.Y) / 2);
            if (Constraint == Constraint.Horizontal)
                e.Graphics.DrawLine(constraintPen, middle.X - 10, middle.Y + 4, middle.X + 10, middle.Y + 4);
            else if(Constraint == Constraint.Vertical) 
                e.Graphics.DrawLine(constraintPen, middle.X - 4, middle.Y + 22, middle.X - 4, middle.Y + 2);
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

    }
}
