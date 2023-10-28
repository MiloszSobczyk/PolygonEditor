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
        private static readonly Brush activeBrush = new SolidBrush(Color.Green);
        public Vertex(Point point)
        {
            X = point.X;
            Y = point.Y;
            Neighbors = new (Vertex vertex, Constraint constraint)[2];
        }
        public bool Hovered { get; set; } = false;
        public bool Selected { get; set; } = false;
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
            X += dX;
            Y += dY;
        }
        public void Draw(Bitmap bitmap, PaintEventArgs e, Brush brush)
        {
            if(Hovered)
                e.Graphics.FillEllipse(activeBrush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
            else if(Selected)
                e.Graphics.FillEllipse(activeBrush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
            else
                e.Graphics.FillEllipse(brush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
        }
    }
}
