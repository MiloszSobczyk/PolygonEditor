using Microsoft.VisualBasic.Devices;
using PolygonEditor.Shapes;
using System.Diagnostics;

namespace PolygonEditor
{
    public partial class PolygonEditor : Form
    {
        private readonly List<Polygon> polygons;
        private bool useBresenham = false;
        private bool creatingPolygon = false;
        private bool editingPolygon = false;
        private bool mouseDown = false;
        private Polygon? hoveredPolygon = null;
        private Polygon? selectedPolygon = null;
        private Vertex? hoveredVertex = null;
        private Vertex? selectedVertex = null;
        private Edge? hoveredEdge = null;
        private Edge? selectedEdge = null;

        private Point mousePosition;
        private readonly Bitmap bitmap;
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
            p1.Edges[0].SetConstraint(Constraint.Horizontal);
            p1.Edges[1].SetConstraint(Constraint.Vertical);
            polygons.Add(p1);

            Polygon p2 = new Polygon(new Vertex(650, 400));
            p2.AddVertex(300, 400);
            p2.AddVertex(400, 200);
            p2.AddVertex(400, 100);
            p2.AddVertex(550, 150);
            p2.AddVertex(550, 200);
            p2.AddVertex(650, 600, true);
            p2.Edges[0].SetConstraint(Constraint.Horizontal);
            p2.Edges[2].SetConstraint(Constraint.Vertical);
            p2.Edges[4].SetConstraint(Constraint.Vertical);
            polygons.Add(p2);
        }
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            this.polygons.ForEach(polygon => polygon.Draw(bitmap, e, useBresenham));
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
            mousePosition = e.Location;
            if (selectedPolygon == null)
            {
                ChangeHoveredPolygon(null);
                foreach (Polygon polygon in polygons)
                    if (Functions.CalculateDistance(mousePosition, polygon.CenterOfMass) < 10.0f)
                        ChangeHoveredPolygon(polygon);
                return;
            }
            if (selectedPolygon != null && !mouseDown) // one condition unnecessary
            {
                ChangeHoveredVertex(selectedPolygon.Vertices
                    .FirstOrDefault(vertex => Functions.CalculateDistance(mousePosition, vertex.Point) < Vertex.radius));
                if (hoveredVertex != null)
                {
                    ChangeHoveredEdge(null);
                    return;
                }
                ChangeHoveredEdge(selectedPolygon.Edges
                    .FirstOrDefault(edge => edge.CalculateDistanceFromEdge(mousePosition) < 10.0f));
            }

            if (selectedVertex != null && mouseDown)
            {
                selectedVertex.Move(mousePosition.X - selectedVertex.X, mousePosition.Y - selectedVertex.Y);
            }
            else if (selectedEdge != null && mouseDown)
            {
                selectedEdge.Move(mousePosition.X - selectedEdge.ClickPoint.X,
                    mousePosition.Y - selectedEdge.ClickPoint.Y);
                selectedEdge.ClickPoint = mousePosition;
            }
            else if (selectedPolygon != null && mouseDown
                && selectedEdge == null && selectedVertex == null)
            {
                selectedPolygon.Move(mousePosition.X - selectedPolygon.CenterOfMass.X,
                    mousePosition.Y - selectedPolygon.CenterOfMass.Y);
            }
            else
            {
                this.canvas.Invalidate();
                return;
            }
            selectedPolygon!.CalculateOffset(25.0f);
            selectedPolygon!.CalculateCenterOfMass();
            this.canvas.Invalidate();
        }
        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (hoveredPolygon != null)
            {
                ChangeSelectedPolygon(hoveredPolygon);
                ChangeHoveredPolygon(null);
                editingPolygon = selectedPolygon!.Finished;
                creatingPolygon = !selectedPolygon.Finished;
                return;
            }

            if (creatingPolygon)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (selectedPolygon!.Vertices[0] == hoveredVertex)
                    {
                        selectedPolygon.AddVertex(0, 0, true);
                        creatingPolygon = false;
                        editingPolygon = true;
                    }
                    else selectedPolygon!.AddVertex(e.X, e.Y);
                }
                else if (e.Button == MouseButtons.Right)
                    selectedPolygon!.RemoveVertex(selectedPolygon!.Vertices.Last());
            }
            else if (editingPolygon)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ChangeSelectedVertex(hoveredVertex);
                    ChangeSelectedEdge(hoveredEdge);
                }
                if (e.Button == MouseButtons.Right && hoveredVertex != null)
                {
                    selectedPolygon!.RemoveVertex(hoveredVertex);
                    if (!selectedPolygon.Finished)
                    {
                        creatingPolygon = true;
                        editingPolygon = false;
                    }
                }
            }
            if (selectedPolygon != null && selectedPolygon.Vertices.Count == 0)
            {
                polygons.Remove(selectedPolygon);
                creatingPolygon = false;
                ChangeHoveredPolygon(null);
                ChangeSelectedPolygon(null);
            }
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
                        selectedPolygon!.AddInBetween(hoveredEdge);
                        ChangeHoveredEdge(null);
                        ChangeSelectedEdge(null);
                        this.canvas.Invalidate();
                        return;
                    }
                }
                creatingPolygon = true;
                Polygon newPolygon = new Polygon(new Vertex(e.X, e.Y));
                polygons.Add(newPolygon);
                ChangeSelectedPolygon(newPolygon);
            }
            this.canvas.Invalidate();
        }
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = !creatingPolygon;
            ChangeSelectedVertex(hoveredVertex);
            if (selectedVertex != null)
            {
                ChangeSelectedEdge(null);
                return;
            }
            ChangeSelectedEdge(hoveredEdge);
            if (selectedEdge != null)
                selectedEdge.ClickPoint = e.Location;
        }
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private void PolygonEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (selectedEdge == null && selectedVertex == null)
                    ChangeSelectedPolygon(null);
                ChangeSelectedEdge(null);
                ChangeSelectedVertex(null);
                creatingPolygon = false;
                editingPolygon = false;
            }
            else if (e.KeyCode == Keys.N && polygons.Count > 0)
            {
                int index = selectedPolygon == null ? 0 : polygons.IndexOf(selectedPolygon);
                ChangeHoveredPolygon(polygons[index == polygons.Count - 1 ? 0 : index + 1]);
            }
            else if (e.KeyCode == Keys.R)
            {
                polygons.Clear();
                editingPolygon = false;
                creatingPolygon = false;
                ChangeHoveredEdge(null);
                ChangeSelectedEdge(null);
                ChangeHoveredPolygon(null);
                ChangeSelectedPolygon(null);
                ChangeSelectedVertex(null);
                ChangeHoveredVertex(null);
            }
            else if (e.KeyCode == Keys.H && selectedEdge != null)
                selectedEdge.SetConstraint(Constraint.Horizontal);
            else if (e.KeyCode == Keys.V && selectedEdge != null)
                selectedEdge.SetConstraint(Constraint.Vertical);
            else if (e.KeyCode == Keys.X && selectedEdge != null)
                selectedEdge.SetConstraint(Constraint.None);
            if (e.KeyCode == Keys.H || e.KeyCode == Keys.V || e.KeyCode == Keys.X)
            {
                this.noneRadioButton.Checked = false;
                this.horizontalRadioButton.Checked = false;
                this.verticalRadioButton.Checked = false;

                this.horizontalRadioButton.Enabled =
                    selectedEdge.Vertex1.Neighbors[0].constraint != Constraint.Horizontal
                    && selectedEdge.Vertex2.Neighbors[1].constraint != Constraint.Horizontal;
                this.verticalRadioButton.Enabled =
                    selectedEdge.Vertex1.Neighbors[0].constraint != Constraint.Vertical
                    && selectedEdge.Vertex2.Neighbors[1].constraint != Constraint.Vertical;

                if (selectedEdge.Constraint == Constraint.None)
                    this.noneRadioButton.Checked = true;
                else if (selectedEdge.Constraint == Constraint.Horizontal)
                    this.horizontalRadioButton.Checked = true;
                else if (selectedEdge.Constraint == Constraint.Vertical)
                    this.verticalRadioButton.Checked = true;
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
            this.edgeConstraintLabel.Visible = false;
            this.noneRadioButton.Visible = false;
            this.horizontalRadioButton.Visible = false;
            this.verticalRadioButton.Visible = false;
            if (selectedEdge != null)
                selectedEdge.Selected = false;
            selectedEdge = newSelectedEdge;
            if (selectedEdge != null)
            {
                selectedEdge.Selected = true;

                this.edgeConstraintLabel.Visible = true;
                this.noneRadioButton.Visible = true;
                this.horizontalRadioButton.Visible = true;
                this.verticalRadioButton.Visible = true;

                this.noneRadioButton.Checked = false;
                this.horizontalRadioButton.Checked = false;
                this.verticalRadioButton.Checked = false;

                this.horizontalRadioButton.Enabled =
                    selectedEdge.Vertex1.Neighbors[0].constraint != Constraint.Horizontal
                    && selectedEdge.Vertex2.Neighbors[1].constraint != Constraint.Horizontal;
                this.verticalRadioButton.Enabled =
                    selectedEdge.Vertex1.Neighbors[0].constraint != Constraint.Vertical
                    && selectedEdge.Vertex2.Neighbors[1].constraint != Constraint.Vertical;

                if (selectedEdge.Constraint == Constraint.None)
                    this.noneRadioButton.Checked = true;
                else if (selectedEdge.Constraint == Constraint.Horizontal)
                    this.horizontalRadioButton.Checked = true;
                else if (selectedEdge.Constraint == Constraint.Vertical)
                    this.verticalRadioButton.Checked = true;
            }
        }
        private void ChangeHoveredPolygon(Polygon? newHoveredPolygon)
        {
            foreach (Polygon polygon in polygons) polygon.Hovered = false;
            hoveredPolygon = newHoveredPolygon;
            if (hoveredPolygon != null)
            {
                hoveredPolygon.Hovered = true;
                creatingPolygon = !hoveredPolygon.Finished;
                editingPolygon = !creatingPolygon;
            }
            this.canvas.Invalidate();
        }
        private void ChangeSelectedPolygon(Polygon? newSelectedPolygon)
        {
            foreach (Polygon polygon in polygons) polygon.Selected = false;
            selectedPolygon = newSelectedPolygon;
            if (selectedPolygon != null)
            {
                selectedPolygon.Selected = true;
                creatingPolygon = !selectedPolygon.Finished;
                editingPolygon = selectedPolygon.Finished;
            }
            ChangeSelectedEdge(null);
            ChangeSelectedVertex(null);
            this.canvas.Invalidate();
        }
        private void ChangeHoveredVertex(Vertex? newHoveredVertex)
        {
            if (selectedPolygon != null)
                foreach (Vertex vertex in selectedPolygon.Vertices)
                    vertex.Hovered = false;
            hoveredVertex = newHoveredVertex;
            if (hoveredVertex != null)
                hoveredVertex.Hovered = true;
            this.canvas.Invalidate();
        }
        private void ChangeSelectedVertex(Vertex? newSelectedVertex)
        {
            if (selectedPolygon != null)
                foreach (Vertex vertex in selectedPolygon.Vertices)
                    vertex.Selected = false;
            selectedVertex = newSelectedVertex;
            if (selectedVertex != null)
                selectedVertex.Selected = true;
            this.canvas.Invalidate();
        }
        private void noneRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (noneRadioButton.Checked)
                selectedEdge.SetConstraint(Constraint.None);
        }
        private void horizontalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (horizontalRadioButton.Checked)
                selectedEdge.SetConstraint(Constraint.Horizontal);
        }
        private void verticalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (verticalRadioButton.Checked)
                selectedEdge.SetConstraint(Constraint.Vertical);

        }
        private void bresenhamCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            useBresenham = bresenhamCheckbox.Checked;
            this.canvas.Invalidate();
        }
    }
}