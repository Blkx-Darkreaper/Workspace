using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Pathfinder
{
    public class Grid
    {
        public Point center { get; set; }
        public int terrain { get; set; }
        public const int SIZE = 32;

        public Grid(Point center, int terrain)
        {
            this.center = center;
            this.terrain = terrain;
        }

        public Grid(int cornerX, int cornerY, int terrain) : this(new Point(cornerX + SIZE / 2, cornerY + SIZE / 2), terrain) { }

        public Grid(Point center, Random random, int min, int max)
        {
            this.center = center;

            this.terrain = random.Next(min, max);
        }

        public Grid(int cornerX, int cornerY, Random random, int min, int max) : this(new Point(cornerX + SIZE / 2, cornerY + SIZE / 2), random, min, max) { }

        public bool Equals(Grid otherGrid)
        {
            if (center.X != otherGrid.center.X)
            {
                return false;
            }

            if (center.Y != otherGrid.center.Y)
            {
                return false;
            }

            return true;
        }

        protected Point GetTopLeftCorner()
        {
            int cornerX = center.X - SIZE / 2;
            int cornerY = center.Y - SIZE / 2;

            Point corner = new Point(cornerX, cornerY);
            return corner;
        }

        public void Draw(Graphics graphics, Color lineColour, int lineThickness)
        {
            DrawBorder(graphics, lineColour, lineThickness);
            DrawLabel(graphics);
        }

        protected void DrawBorder(Graphics graphics, Color lineColour, int lineThickness)
        {
            Point corner = GetTopLeftCorner();

            Point[] allPoints = new Point[5];
            allPoints[0] = corner;
            allPoints[1] = new Point(corner.X + SIZE, corner.Y);
            allPoints[2] = new Point(corner.X + SIZE, corner.Y + SIZE);
            allPoints[3] = new Point(corner.X, corner.Y + SIZE);
            allPoints[4] = corner;

            Pen pen = new Pen(lineColour, lineThickness);
            graphics.DrawLines(pen, allPoints);
        }

        protected void DrawLabel(Graphics graphics)
        {
            string label = terrain.ToString();

            Point corner = GetTopLeftCorner();
            int cornerX = corner.X;
            int cornerY = corner.Y;
            Rectangle bounds = new Rectangle(cornerX, cornerY, SIZE, SIZE);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Font font = new Font("Calibri", 32, FontStyle.Regular);
            SizeF fontSize = graphics.MeasureString(label, font);
            Font resizedFont = ScaleFontToFit(font, fontSize, bounds);

            graphics.DrawString(label, resizedFont, Brushes.Black, bounds, format);
        }

        protected Font ScaleFontToFit(Font font, SizeF fontSize, Rectangle bounds)
        {
            float widthRatio = bounds.Width / fontSize.Width;
            float heightRatio = bounds.Height / fontSize.Height;

            float ratio = Math.Min(widthRatio, heightRatio);

            float resizedSize = font.Size * ratio;

            Font resizedFont = new Font(font.FontFamily, resizedSize);
            return resizedFont;
        }
    }
}
