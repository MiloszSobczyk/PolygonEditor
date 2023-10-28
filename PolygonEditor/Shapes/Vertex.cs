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
        }
        public bool Selected { get; set; }
        public Vertex(int X, int Y) : this(new Point(X, Y)) { }
        public int X { get; set; }
        public int Y { get; set; }
        public Edge? Edge1 { get; set; }
        public Edge? Edge2 { get; set; }
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
            e.Graphics.FillEllipse(brush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
        }
    }
}
