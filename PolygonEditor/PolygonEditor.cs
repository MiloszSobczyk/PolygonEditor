using PolygonEditor.Shapes;
using System.Diagnostics;

namespace PolygonEditor
{
    public partial class PolygonEditor : Form
    {
        private readonly Pen blackPen;
        private readonly SolidBrush blackBrush;
        private readonly List<Polygon> polygons;
        private bool creatingPolygon = false;
        private Polygon? editedPolygon = null;
        private Point mousePosition;
        private readonly Bitmap bitmap;
        public PolygonEditor()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.canvas.Width, this.canvas.Height);
            blackPen = new Pen(Color.Black);
            blackBrush = new SolidBrush(Color.Black);
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
            p1.AddVertex(100, 100, true);
            polygons.Add(p1);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            this.polygons.ForEach(polygon => polygon.Draw(bitmap, e));
            if (creatingPolygon)
            {
                Point lastPoint = editedPolygon!.RecentVertex.Point;
                e.Graphics.DrawLine(blackPen, lastPoint, mousePosition);
                e.Graphics.FillEllipse(blackBrush, lastPoint.X - 5, lastPoint.Y - 5, 10, 10);
            }
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!creatingPolygon) return;
            mousePosition = e.Location;
            this.canvas.Invalidate();
        }
        private void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon) return;
            creatingPolygon = true;
            editedPolygon = new Polygon(new Vertex(e.X, e.Y));
            polygons.Add(editedPolygon);
            this.canvas.Invalidate();
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon)
            {
                if (e.Button == MouseButtons.Left) editedPolygon!.AddVertex(e.X, e.Y);
                else if (e.Button == MouseButtons.Right)
                {
                    if (editedPolygon!.RemoveVertex())
                    {
                        polygons.Remove(editedPolygon);
                        creatingPolygon = false;
                        editedPolygon = null;
                        this.canvas.Invalidate();
                    }
                }
            }
        }
    }
}