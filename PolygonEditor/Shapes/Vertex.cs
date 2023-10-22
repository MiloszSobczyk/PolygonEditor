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
        private int x;
        private int y;
        public List<Edge> Edges { get; private set; } = new List<Edge>();
        public Vertex(Point point)
        {
            this.x = point.X;
            this.y = point.Y;
            this.Edges = new List<Edge>();
        }
        public Vertex(int X, int Y) : this(new Point(X, Y)) { }
        public int X
        {
            get => x;
            set => x = value;
        }
        public int Y
        {
            get => x;
            set => y = value;
        }
        public override void Move(int dX, int dY)
        {
            x += dX;
            y += dY;
        }
        public void AddEdge(Edge edge) => Edges.Add(edge);
        public void RemoveEdge(Edge edge) => Edges.Remove(edge);
        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(new SolidBrush(Color.Black), 
                new Rectangle(x - radius, y - radius, radius, radius));
        }
    }
}
