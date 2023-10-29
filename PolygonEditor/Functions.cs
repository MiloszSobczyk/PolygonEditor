using PolygonEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void BresenhamDrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            int w = x2 - x1;
            int h = y2 - y1;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                
                //putpixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x1 += dx1;
                    y1 += dy1;
                }
                else
                {
                    x1 += dx2;
                    y1 += dy2;
                }
            }
        }
    }
}
