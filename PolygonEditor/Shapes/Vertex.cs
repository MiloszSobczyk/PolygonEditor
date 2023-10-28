using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PolygonEditor.Shapes
{

    public class Vertex : Shape
    {
        public static readonly int radius = 10;
        public Vertex(Point point)
        {
            Selected = false;
            X = point.X;
            Y = point.Y;
            Neighbors = new (Vertex vertex, Constraint constraint)[2];
        }
        public bool Selected { get; set; }
        public Vertex(int X, int Y) : this(new Point(X, Y)) { }
        public int X { get; set; }
        public int Y { get; set; }
        public (Vertex vertex, Constraint constraint)[] Neighbors { get; set; }
        public Point Point
        {
            get { return new Point(X, Y); }
        }
        public override void Move(int dX, int dY)
        {
            if (Neighbors[0].constraint == Constraint.None && Neighbors[1].constraint == Constraint.None)
            {
                X += dX;
                Y += dY;
                return;
            }
            if (Neighbors[0].constraint != Constraint.None)
            {
                int constraint = (int)Neighbors[0].constraint;
                //Neighbors[0].vertex.X += (1 - constraint) * dX;
                //Neighbors[0].vertex.Y += constraint * dY;
                X += (1 - constraint) * dX;
                Y += constraint * dY;
            }
            if (Neighbors[1].constraint != Constraint.None)
            {
                int constraint = (int)Neighbors[1].constraint;
                //Neighbors[1].vertex.X += (1 - constraint) * dX;
                //Neighbors[1].vertex.Y += constraint * dY;
                X += (1 - constraint) * dX;
                Y += constraint * dY;
            }
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Brush brush)
        {
            e.Graphics.FillEllipse(brush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
        }
    }
}
