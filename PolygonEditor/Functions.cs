using PolygonEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PolygonEditor
{
    public static class Functions
    {
        public static double CalculateDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        public static bool UpdateConstraints(Vertex vertex1, Vertex vertex2, Constraint constraint)
        {
            if(constraint != Constraint.None)
            {
                if (vertex1.Neighbors[0].constraint == constraint) return false;
                if (vertex2.Neighbors[1].constraint == constraint) return false;
            }
            vertex1.Neighbors[1] = (vertex2, constraint);
            vertex2.Neighbors[0] = (vertex1, constraint);
            return true;
        }
        public static void BresenhamDrawLine(int x1, int y1, int x2, int y2, Color color, PaintEventArgs e)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            using (SolidBrush brush = new SolidBrush(color))
            {
                while (true)
                {
                    e.Graphics.FillRectangle(brush, x1, y1, 1, 1);
                    if (x1 == x2 && y1 == y2)
                        break;
                    int err2 = 2 * err;
                    if (err2 > -dy)
                    {
                        err -= dy;
                        x1 += sx;
                    }
                    if (err2 < dx)
                    {
                        err += dx;
                        y1 += sy;
                    }
                }
            }
        }
    }
}
