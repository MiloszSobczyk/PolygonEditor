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
        private Polygon? hoveredPolygon = null;
        private Polygon? selectedPolygon = null;
        private Vertex? hoveredVertex = null;
        private Vertex? selectedVertex = null;
        private Edge? hoveredEdge = null;
        private Edge? selectedEdge = null;

        private bool movingPolygon = false;
        private Point mousePosition;
        private readonly Bitmap bitmap;
        private bool mouseDown = false;
        public PolygonEditor()
        {
            this.KeyPreview = true;
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
            this.polygons.ForEach(polygon => polygon.Draw(bitmap, e));
            if (creatingPolygon)
            {
                Point lastPoint = hoveredPolygon!.Vertices.Last().Point;
                e.Graphics.DrawLine(Polygon.pens["blue"], lastPoint, mousePosition);
                e.Graphics.FillEllipse(Polygon.brushes["blue"], lastPoint.X - 5, lastPoint.Y - 5, 10, 10);
            }
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedVertex != null && editingPolygon)
            {
                ChangeHoveredPolygon(null);
                foreach (Polygon polygon in polygons)
                    if (Functions.CalculateDistance(mousePosition, polygon.CenterOfMass) < 10.0f)
                        ChangeHoveredPolygon(polygon);
                if(hoveredPolygon == null)
                {
                    return;
                }
                if (hoveredPolygon != null &&
                    Functions.CalculateDistance(mousePosition, hoveredPolygon.CenterOfMass) < 10.0f)
                {
                    editingPolygon = true;
                    movingPolygon = true;
                    return;
                }

                hoveredVertex = hoveredPolygon!.Vertices
                    .FirstOrDefault(vertex => Functions.CalculateDistance(e.Location, vertex.Point) < Vertex.radius);
                if (hoveredVertex != null)
                {
                    ChangeHoveredEdge(null);
                    return;
                }

                ChangeHoveredEdge(hoveredPolygon!.Edges
                    .FirstOrDefault(edge => edge.CalculateDistanceFromEdge(e.Location) < 10.0f));
                if (hoveredEdge != null)
                    hoveredEdge.ClickPoint = e.Location;
            }

            if (selectedVertex != null && mouseDown)
            {
                selectedVertex.Move(e.Location.X - selectedVertex.X, e.Location.Y - selectedVertex.Y);
                hoveredPolygon!.CalculateCenterOfMass();
                this.canvas.Invalidate();
                return;
            }
            if (selectedEdge != null && mouseDown)
            {
                selectedEdge.Move(e.Location.X - selectedEdge.ClickPoint.X, 
                    e.Location.Y - selectedEdge.ClickPoint.Y);
                hoveredPolygon!.CalculateCenterOfMass();
                selectedEdge.ClickPoint = e.Location;
                this.canvas.Invalidate();
                return;
            }
            if (selectedPolygon != null && mouseDown)
            {
                selectedPolygon.Move(e.Location.X - selectedPolygon.CenterOfMass.X, 
                    e.Location.Y - selectedPolygon.CenterOfMass.Y);
                selectedPolygon.CalculateCenterOfMass();
                this.canvas.Invalidate();
                return;
            }
            if (!creatingPolygon && !editingPolygon)
            {
                ChangeHoveredPolygon(null);
                foreach (Polygon polygon in polygons)
                {
                    if (Functions.CalculateDistance(mousePosition, polygon.CenterOfMass) < Vertex.radius)
                    {
                        ChangeHoveredPolygon(polygon);
                        break;
                    }
                }
                this.canvas.Invalidate();
                return;
            }
            if (hoveredPolygon == null) return;
            if (hoveredPolygon.Vertices.Count >= 0 &&
                Functions.CalculateDistance(mousePosition, hoveredPolygon.Vertices[0].Point) < Vertex.radius)
                hoveredPolygon.Vertices[0].Hovered = true;
            else hoveredPolygon.Vertices[0].Hovered = false;
            this.canvas.Invalidate();
        }
        private void canvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon) return;
            if (e.Button == MouseButtons.Left)
            {
                if (editingPolygon)
                {
                    if (hoveredEdge != null)
                    {
                        hoveredPolygon!.AddBetween(hoveredEdge);
                        ChangeHoveredEdge(null);
                        this.canvas.Invalidate();
                        return;
                    }
                }

                foreach (Polygon polygon in polygons)
                    foreach (Vertex vertex in polygon.Vertices)
                        vertex.Hovered = false;
                creatingPolygon = true;
                Polygon newPolygon = new Polygon(new Vertex(e.X, e.Y));
                ChangeHoveredPolygon(newPolygon);
                polygons.Add(newPolygon);
            }
            this.canvas.Invalidate();
        }
        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (creatingPolygon)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Vertex firstVertex = hoveredPolygon!.Vertices[0];
                    if (firstVertex.Hovered)
                    {
                        hoveredPolygon.AddVertex(0, 0, true);
                        creatingPolygon = false;
                        editingPolygon = true;
                    }
                    else hoveredPolygon!.AddVertex(e.X, e.Y);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    hoveredPolygon!.RemoveVertex(hoveredPolygon!.Vertices.Last());
                    if (hoveredPolygon.Vertices.Count == 0)
                    {
                        polygons.Remove(hoveredPolygon);
                        creatingPolygon = false;
                        hoveredPolygon = null;
                    }
                }
                this.canvas.Invalidate();
                return;
            }
            else if (editingPolygon)
            {
                if (e.Button == MouseButtons.Right)
                {
                    foreach (Vertex vertex in hoveredPolygon!.Vertices)
                        if (Functions.CalculateDistance(vertex.Point, e.Location) < 3.0f)
                        {
                            hoveredPolygon!.RemoveVertex(vertex);
                            if (hoveredPolygon!.Vertices.Count == 2)
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

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (hoveredPolygon == null || hoveredVertex != null || hoveredVertex != null) return;

            mouseDown = true;
            if (hoveredPolygon != null &&
                Functions.CalculateDistance(mousePosition, hoveredPolygon.CenterOfMass) < 10.0f)
            {
                editingPolygon = true;
                movingPolygon = true;
                return;
            }

            hoveredVertex = hoveredPolygon!.Vertices
                .FirstOrDefault(vertex => Functions.CalculateDistance(e.Location, vertex.Point) < Vertex.radius);
            if (hoveredVertex != null)
            {
                ChangeHoveredEdge(null);
                return;
            }

            ChangeHoveredEdge(hoveredPolygon!.Edges
                .FirstOrDefault(edge => edge.CalculateDistanceFromEdge(e.Location) < 10.0f));
            if (hoveredEdge != null)
                hoveredEdge.ClickPoint = e.Location;
        }
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            hoveredVertex = null;
            ChangeHoveredEdge(null);
            movingPolygon = false;
        }
        private void PolygonEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ChangeHoveredPolygon(null);
                creatingPolygon = false;
                editingPolygon = false;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                int index = hoveredPolygon == null ? 0 : polygons.IndexOf(hoveredPolygon);
                ChangeHoveredPolygon(polygons[index == polygons.Count - 1 ? 0 : index + 1]);
            }
            else if (e.KeyCode == Keys.R)
            {
                polygons.Clear();
                editingPolygon = false;
                creatingPolygon = false;
                hoveredPolygon = null;
                ChangeHoveredEdge(null);
                hoveredVertex = null;
                this.KeyPreview = true;
            }
            else if (e.KeyCode == Keys.H && hoveredEdge != null)
            {
                hoveredEdge.AddConstraint(Constraint.Horizontal);
            }
            else if (e.KeyCode == Keys.V && hoveredEdge != null)
            {
                hoveredEdge.AddConstraint(Constraint.Vertical);
            }
            this.canvas.Invalidate();
        }

        private void ChangeHoveredEdge(Edge? newHoveredEdge)
        {
            if (hoveredEdge != null)
                hoveredEdge.Hovered = false;
            hoveredEdge = newHoveredEdge;
            if (hoveredEdge != null)
                hoveredEdge.Hovered = true;
        }
        private void ChangeSelectedEdge(Edge? newSelectedEdge)
        {
            this.horizontalCheckbox.Checked = false;
            this.verticalCheckbox.Checked = false;
            if (selectedEdge != null)
                selectedEdge.Selected = false;
            selectedEdge = newSelectedEdge;
            if (selectedEdge != null)
            {
                selectedEdge.Selected = true;
                this.horizontalCheckbox.Checked = selectedEdge.Constraint == Constraint.Horizontal;
                this.verticalCheckbox.Checked = selectedEdge.Constraint == Constraint.Vertical;
            }
        }
        private void ChangeHoveredPolygon(Polygon? newHoveredPolygon)
        {
            foreach (Polygon polygon in polygons) polygon.Hovered = false;
            if (newHoveredPolygon != null)
            {
                newHoveredPolygon.Hovered = true;
                hoveredPolygon = newHoveredPolygon;
                creatingPolygon = !hoveredPolygon.Finished;
                editingPolygon = !creatingPolygon;
            }
            this.canvas.Invalidate();
        }
        private void ChangeSelectedPolygon(Polygon? newSelectedPolygon)
        {
            foreach (Polygon polygon in polygons) polygon.Selected = false;
            if (newSelectedPolygon != null)
            {
                newSelectedPolygon.Selected = true;
                selectedPolygon = newSelectedPolygon;
                creatingPolygon = !selectedPolygon.Finished;
                editingPolygon = !creatingPolygon;
            }
            this.canvas.Invalidate();
        }
        private void ChangeHoveredVertex(Vertex newHoveredVertex)
        {
            if(selectedPolygon != null)
                foreach (Vertex vertex in selectedPolygon.Vertices) 
                    vertex.Hovered = false;
            if (newHoveredVertex != null)
            {
                newHoveredVertex.Hovered = true;
                hoveredVertex = newHoveredVertex;
            }
            this.canvas.Invalidate();
        }
        private void ChangeSelectedVertex(Vertex newSelectedVertex)
        {
            if (selectedPolygon != null)
                foreach (Vertex vertex in selectedPolygon.Vertices)
                    vertex.Selected = false;
            if (newSelectedVertex != null)
            {
                newSelectedVertex.Selected = true;
                selectedVertex = newSelectedVertex;
            }
            this.canvas.Invalidate();
        }
    }
}