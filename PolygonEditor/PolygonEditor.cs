using PolygonEditor.Shapes;
using System.Diagnostics;

namespace PolygonEditor
{
    public partial class PolygonEditor : Form
    {
        private readonly List<Polygon> polygons;
        private bool creatingPolygon = false;
        private Polygon? selectedPolygon = null;
        private Point mousePosition;
        private readonly Bitmap bitmap;
        public PolygonEditor()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.canvas.Width, this.canvas.Height);
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
                Point lastPoint = selectedPolygon!.RecentVertex.Point;
                e.Graphics.DrawLine(Polygon.pens["blue"], lastPoint, mousePosition);
                e.Graphics.FillEllipse(Polygon.brushes["blue"], lastPoint.X - 5, lastPoint.Y - 5, 10, 10);
            }
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            if (!creatingPolygon)
            {
                ChangeSelectedPolygon(null);
                foreach (Polygon polygon in polygons)
                {
                    // should also set editedPolyggon to null in certain cases
                    // should see all that may be selected and check which one is closest
                    if (Functions.CalculateDistance(mousePosition, polygon.CenterOfMass) < Vertex.radius)
                    {
                        ChangeSelectedPolygon(polygon);
                        break;
                    }
                }
                this.canvas.Invalidate();
                return;
            }
            if (selectedPolygon!.VertexCount >= 3 && 
                Functions.CalculateDistance(mousePosition, selectedPolygon!.Vertices[0].Point) < Vertex.radius)
                selectedPolygon.Vertices[0].Selected = true;
            else selectedPolygon.Vertices[0].Selected = false;
            this.canvas.Invalidate();
        }
        private void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon) return;
            foreach (Polygon polygon in polygons)
                foreach (Vertex vertex in polygon.Vertices)
                    vertex.Selected = false;
            creatingPolygon = true;
            selectedPolygon = new Polygon(new Vertex(e.X, e.Y));
            selectedPolygon.Selected = true;
            polygons.Add(selectedPolygon);
            this.canvas.Invalidate();
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Vertex firstVertex = selectedPolygon!.Vertices[0];
                    if (firstVertex.Selected)
                    {
                        selectedPolygon.AddVertex(0, 0, true);
                        selectedPolygon = null;
                        creatingPolygon = false;
                    }
                    else selectedPolygon!.AddVertex(e.X, e.Y);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (selectedPolygon!.RemoveLastVertex())
                    {
                        polygons.Remove(selectedPolygon);
                        creatingPolygon = false;
                        selectedPolygon = null;
                    }
                }
                this.canvas.Invalidate();
            }
            else
            {
                foreach(Polygon polygon in polygons)
                    if(Functions.CalculateDistance(mousePosition, polygon.CenterOfMass) < 10.0f)
                        ChangeSelectedPolygon(polygon);
            
            }
        }
        private void ChangeSelectedPolygon(Polygon? newSelectedPolygon)
        {
            foreach (Polygon polygon in polygons) polygon.Selected = false;
            if (newSelectedPolygon == null) return;
            newSelectedPolygon.Selected = true;
            selectedPolygon = newSelectedPolygon;
        }
    }
}