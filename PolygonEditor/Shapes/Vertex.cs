using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PolygonEditor.Shapes
{

    public class Vertex
    {
        public static readonly int radius = 10;
        private static readonly Brush activeBrush = new SolidBrush(Color.Green);
        public bool Hovered { get; set; } = false;
        public bool Selected { get; set; } = false;
        public int X { get; set; }
        public int Y { get; set; }
        public (Vertex vertex, Constraint constraint)[] Neighbors { get; set; }
        public Vertex(int X, int Y) : this(new Point(X, Y)) { }
        public Vertex(Point point)
        {
            X = point.X;
            Y = point.Y;
            Neighbors = new (Vertex vertex, Constraint constraint)[2];
        }
        public Point Point
        {
            get { return new Point(X, Y); }
            set { X = value.X; Y = value.Y; }
        }
        public void Move(int dX, int dY)
        {
            X += dX;
            Y += dY;
            if (Neighbors[0].constraint == Constraint.Vertical)
                Neighbors[0].vertex.X += dX;
            else if (Neighbors[0].constraint == Constraint.Horizontal)
                Neighbors[0].vertex.Y += dY;
            if (Neighbors[1].constraint == Constraint.Vertical)
                Neighbors[1].vertex.X += dX;
            else if (Neighbors[1].constraint == Constraint.Horizontal)
                Neighbors[1].vertex.Y += dY;
        }
        public void MoveAtEdge(int dX, int dY, int index)
        {
            X += dX;
            Y += dY;
            if (Neighbors[index].constraint == Constraint.Vertical)
                Neighbors[index].vertex.X += dX;
            else if (Neighbors[index].constraint == Constraint.Horizontal)
                Neighbors[index].vertex.Y += dY;
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Brush brush)
        {
            if (X == int.MaxValue || X == int.MinValue || Y == int.MaxValue || Y == int.MinValue) return;
            if (Hovered)
                e.Graphics.FillEllipse(activeBrush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
            else if (Selected)
                e.Graphics.FillEllipse(activeBrush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
            else
                e.Graphics.FillEllipse(brush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
        }
    }
}