using Microsoft.VisualBasic.Devices;
using PolygonEditor.Shapes;
using System.Diagnostics;

namespace PolygonEditor
{
    public partial class PolygonEditor : Form
    {
        private readonly List<Polygon> polygons;
        private bool creatingPolygon = false;
        private bool editingPolygon = false;
        private Polygon? selectedPolygon = null;
        private Vertex? selectedVertex = null;
        private Edge? selectedEdge = null;
        private Point mousePosition;
        private readonly Bitmap bitmap;
        private bool mouseDown = false;
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
            p1.AddVertex(300, 100);
            p1.AddVertex(300, 300);
            p1.AddVertex(100, 300);
            p1.AddVertex(100, 100, true);
            polygons.Add(p1);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            this.polygons.ForEach(polygon => { if (polygon != selectedPolygon) polygon.Draw(bitmap, e); });
            if (selectedPolygon != null) selectedPolygon.Draw(bitmap, e);
            if (creatingPolygon)
            {
                Point lastPoint = selectedPolygon!.Vertices.Last().Point;
                e.Graphics.DrawLine(Polygon.pens["blue"], lastPoint, mousePosition);
                e.Graphics.FillEllipse(Polygon.brushes["blue"], lastPoint.X - 5, lastPoint.Y - 5, 10, 10);
            }
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedVertex != null && editingPolygon)
            {
                selectedVertex.Move(e.Location.X - selectedVertex.X, e.Location.Y - selectedVertex.Y);
                selectedPolygon!.CalculateCenterOfMass();
                this.canvas.Invalidate();
                return;
            }
            if (selectedEdge != null && editingPolygon)
            {
                selectedEdge.Move(e.Location.X - selectedEdge.ClickPoint.X, e.Location.Y - selectedEdge.ClickPoint.Y);
                selectedPolygon!.CalculateCenterOfMass();
                selectedEdge.ClickPoint = e.Location;
                this.canvas.Invalidate();
                return;
            }
            if(selectedPolygon != null && editingPolygon && mouseDown)
            {
                selectedPolygon.Move(e.Location.X - selectedPolygon.CenterOfMass.X, e.Location.Y - selectedPolygon.CenterOfMass.Y);
                selectedPolygon.CalculateCenterOfMass();
                this.canvas.Invalidate();
                return;
            }
            mousePosition = e.Location;
            if (!creatingPolygon && !editingPolygon)
            {
                ChangeSelectedPolygon(null);
                foreach (Polygon polygon in polygons)
                {
                    if (Functions.CalculateDistance(mousePosition, polygon.CenterOfMass) < Vertex.radius)
                    {
                        ChangeSelectedPolygon(polygon);
                        break;
                    }
                }
                this.canvas.Invalidate();
                return;
            }
            if (selectedPolygon == null) return;
            if (selectedPolygon.Vertices.Count >= 0 &&
                Functions.CalculateDistance(mousePosition, selectedPolygon.Vertices[0].Point) < Vertex.radius)
                selectedPolygon.Vertices[0].Selected = true;
            else selectedPolygon.Vertices[0].Selected = false;
            this.canvas.Invalidate();
        }
        private void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon) return;
            if (editingPolygon)
            {
                if(selectedEdge != null)
                {
                    selectedPolygon!.AddBetween(selectedEdge);
                    selectedEdge = null;
                    this.canvas.Invalidate();
                    return;
                }
            }

            foreach (Polygon polygon in polygons)
                foreach (Vertex vertex in polygon.Vertices)
                    vertex.Selected = false;
            creatingPolygon = true;
            Polygon newPolygon = new Polygon(new Vertex(e.X, e.Y));
            ChangeSelectedPolygon(newPolygon);
            polygons.Add(newPolygon);
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
                        creatingPolygon = false;
                        editingPolygon = true;
                    }
                    else selectedPolygon!.AddVertex(e.X, e.Y);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    selectedPolygon!.RemoveVertex(selectedPolygon!.Vertices.Last());
                    if (selectedPolygon.Vertices.Count == 0)
                    {
                        polygons.Remove(selectedPolygon);
                        creatingPolygon = false;
                        selectedPolygon = null;
                    }
                }
                this.canvas.Invalidate();
                return;
            }
            else if (editingPolygon)
            {
                if (e.Button == MouseButtons.Left)
                {

                }
                else if (e.Button == MouseButtons.Right)
                {
                    foreach (Vertex vertex in selectedPolygon!.Vertices)
                        if (Functions.CalculateDistance(vertex.Point, e.Location) < 2.0f)
                        {
                            selectedPolygon!.RemoveVertex(vertex);
                            if (selectedPolygon!.Vertices.Count == 1)
                            {
                                editingPolygon = false;
                                creatingPolygon = true;
                            }
                            return;
                        }
                    //ChangeSelectedPolygon(null);
                    //editingPolygon = false;
                    //creatingPolygon = false;
                }
            }

        }
        private void ChangeSelectedPolygon(Polygon? newSelectedPolygon)
        {
            foreach (Polygon polygon in polygons) polygon.Selected = false;
            if (newSelectedPolygon != null)
            {
                newSelectedPolygon.Selected = true;
                selectedPolygon = newSelectedPolygon;
            }
            this.canvas.Invalidate();
        }
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = (e.Button == MouseButtons.Left);
            if (selectedPolygon != null &&
                Functions.CalculateDistance(mousePosition, selectedPolygon.CenterOfMass) < 10.0f)
            {
                editingPolygon = true;
                return;
            }
            if (selectedPolygon == null || selectedVertex != null || selectedVertex != null) return;
            selectedVertex = selectedPolygon!.Vertices
                .FirstOrDefault(vertex => Functions.CalculateDistance(e.Location, vertex.Point) < Vertex.radius);
            if(selectedVertex != null)
            {
                selectedEdge = null;
                return;
            }
            selectedEdge = selectedPolygon!.Edges
                .FirstOrDefault(edge => edge.CalculateDistanceFromEdge(e.Location) < 10.0f);
            if (selectedEdge != null)
                selectedEdge.ClickPoint = e.Location;
        }
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            selectedVertex = null;
            selectedEdge = null;
        }
        private void canvas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.E)
            {
                creatingPolygon = false;
                editingPolygon = false;
                ChangeSelectedPolygon(null);
                selectedVertex = null;
            }
            this.canvas.Invalidate();
        }
    }
}