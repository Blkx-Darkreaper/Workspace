using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Memory : Component
    {
        public int TotalBytes { get; protected set; } // Bytes
        public int CellWidth { get; protected set; }    // Bytes
        public List<MemoryCell> DataCells { get; protected set; }
        protected int padding { get; set; }

        public Memory(Point center, int totalBytes, int cellWidth)
            : base(center)
        {
            this.TotalBytes = totalBytes;
            this.CellWidth = cellWidth;

            int totalCells = totalBytes / cellWidth;
            this.DataCells = new List<MemoryCell>(totalCells);
            this.padding = 2;

            InitCells(totalCells);
        }

        protected virtual void InitCells(int totalCells)
        {
            Size cellSize = new MemoryCell(new Point(0, 0), 0, 0).GetSize();
            int cellHeight = cellSize.Height;

            int cellCenterX = this.Center.X;
            int initCellCenterY = this.Center.Y - (cellHeight / 2) * (totalCells - 1);

            for (int cellNumber = 0; cellNumber < totalCells; cellNumber++)
            {
                int offsetY = cellNumber * cellHeight;
                int cellCenterY = initCellCenterY + offsetY;

                Point cellCenter = new Point(cellCenterX, cellCenterY);
                MemoryCell cellToAdd = new MemoryCell(cellCenter, cellNumber, totalCells);
                DataCells.Add(cellToAdd);
            }
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            foreach (MemoryCell cell in DataCells)
            {
                cell.Draw(graphics, colour, display);
            }

            Size size = GetSize();
            int dashLength = 1;
            int spaceLength = 2;
            Program.DrawDottedRectangle(graphics, colour, Center, size, dashLength, spaceLength);
        }

        public override Size GetSize()
        {
            Size size = new Size(40, 20);

            int totalCells = DataCells.Count;
            if (totalCells == 0)
            {
                return size;
            }

            Size cellSize = DataCells.First().GetSize();

            int width = cellSize.Width + 2 * padding;
            int height = totalCells * cellSize.Height + 2 * padding;

            size.Width = width;
            size.Height = height;

            return size;
        }
    }
}
