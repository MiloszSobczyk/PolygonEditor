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
        private static readonly SolidBrush blackBrush = new SolidBrush(Color.Black);
        private static readonly SolidBrush blueBrush = new SolidBrush(Color.Blue);
        public Vertex(Point point)
        {
            Selected = false;
            Edges = new List<Edge>();
            X = point.X;
            Y = point.Y;
        }
        public List<Edge> Edges { get; private set; }
        public bool Selected { get; set; }
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
            if(!Selected)
                e.Graphics.FillEllipse(blackBrush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
            else
                e.Graphics.FillEllipse(blueBrush, new Rectangle(X - radius / 2, Y - radius / 2, radius, radius));
        }
    }
}
