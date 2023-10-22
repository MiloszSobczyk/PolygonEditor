using PolygonEditor.Shapes;
using System.Diagnostics;

namespace PolygonEditor
{
    public partial class PolygonEditor : Form
    {
        private readonly List<Polygon> polygons;
        public PolygonEditor()
        {
            InitializeComponent();
            polygons = new List<Polygon>();
            IntializePolygons();
        }
        private void IntializePolygons()
        {
            Polygon p1 = new Polygon(new Vertex(100, 100));
            p1.AddVertex(200, 200);
            p1.AddVertex(300, 200);
            p1.AddVertex(200, 400);
            p1.AddVertex(451, 153);
            polygons.Add(p1);
            this.canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.canvas.Width, this.canvas.Height);
            this.polygons.ForEach(polygon => polygon.Draw(bitmap, e));
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
    }
}