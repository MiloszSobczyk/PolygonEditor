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
        public static readonly SolidBrush blackBrush = new SolidBrush(Color.Black);
        public List<Edge> Edges { get; private set; } = new List<Edge>();
        public Vertex(Point point)
        {
            X = point.X;
            Y = point.Y;
            Edges = new List<Edge>();
        }
        public Vertex(int X, int Y) : this(new Point(X, Y)) { }
        public int X { get; set; }
        public int Y { get; set; }
        public Point Point
        {
            get { return new Point(X, Y); }
        }
        public override void Move(int dX, int dY)
        {
            X += dX;
            Y += dY;
        }
        public void AddEdge(Edge edge) => Edges.Add(edge);
        public void RemoveEdge(Edge edge) => Edges.Remove(edge);
        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(blackBrush, 
                new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
        }
    }
}
