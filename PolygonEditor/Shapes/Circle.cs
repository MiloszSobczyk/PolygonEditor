using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Shapes
{
    public class Circle
    {
        public Point Center { get; set; }
        public int Radius { get; set; }
        public Circle(int x, int y, int radius = 5)
        {
            Center = new Point(x, y);
            Radius = radius;
        }

        public void Draw(Bitmap bitmap, PaintEventArgs e)
        {
            int x = Radius;
            int y = 0;
            int radiusError = 1 - x;
            SolidBrush brush = new SolidBrush(Color.Black);

            while (x >= y)
            {
                e.Graphics.FillRectangle(Brushes.Black, Center.X + x, Center.Y + y, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X - x, Center.Y + y, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X - x, Center.Y - y, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X + x, Center.Y - y, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X + y, Center.Y + x, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X - y, Center.Y + x, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X - y, Center.Y - x, 1, 1);
                e.Graphics.FillRectangle(Brushes.Black, Center.X + y, Center.Y - x, 1, 1);

                y++;

                if (radiusError < 0) radiusError += 2 * y + 1;
                else
                {
                    x--;
                    radiusError += 2 * (y - x) + 1;
                }
            }

            for (x = -Radius; x <= Radius; x++)
            {
                double yTemp = Math.Sqrt(Radius * Radius - x * x);

                int x1 = Center.X + x;
                int x2 = Center.X + x;
                int y1 = (int)(Center.Y - yTemp);
                int y2 = (int)(Center.Y + yTemp);

                double alpha = 1.0 - (yTemp - (int)yTemp);

                if (alpha == 1.0)
                {
                    e.Graphics.FillRectangle(brush, x1, y1, 1, 1);
                    e.Graphics.FillRectangle(brush, x2, y2, 1, 1);
                }
                else
                {
                    int color1 = (int)(alpha * 255);
                    int color2 = (int)((1.0 - alpha) * 255);
                    brush.Color = Color.FromArgb(color1, Color.Black);
                    e.Graphics.FillRectangle(brush, x1, y1, 1, 1);
                    brush.Color = Color.FromArgb(color2, Color.Black);
                    e.Graphics.FillRectangle(brush, x2, y2, 1, 1);
                }
            }
            brush.Color = Color.Red;
            e.Graphics.FillEllipse(brush, new Rectangle(Center.X - 5, Center.Y - 5, 10, 10));
            brush.Dispose();

        }
    }
}
