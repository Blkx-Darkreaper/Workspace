using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Block : DataStructure
    {
        public int BlockNumber { get; protected set; }
        public bool IsEmpty { get; protected set; }
        public bool IsBad { get; protected set; }
        public bool IsEndOfFile { get; protected set; }
        public int MaxEntries { get; protected set; }
        public bool IsSelected { get; set; }
        protected List<Entry> allEntries { get; set; }
        protected int padding { get; set; }
        protected int labelHeight { get; set; }

        public Block(Point center, int blockNumber, int maxEntries) : base(center) {
            this.BlockNumber = blockNumber;
            this.IsEmpty = true;
            this.IsBad = false;
            this.IsEndOfFile = false;
            this.allEntries = new List<Entry>(maxEntries);
            this.MaxEntries = maxEntries;
            this.IsSelected = false;
            this.padding = 2;
            this.labelHeight = 9 + 3;   // 9 is actual height of text

            InitEntries(maxEntries);
        }

        public virtual void InitEntries(int totalEntries)
        {
            Size entrySize = new Entry(new Point(0, 0)).GetSize();
            int entryHeight = entrySize.Height;

            int entryCenterX = this.Center.X;
            int initEntryCenterY = this.Center.Y - (entryHeight / 2) * (totalEntries - 1) + labelHeight - 3 * padding;

            for (int i = 0; i < totalEntries; i++)
            {
                int offsetY = i * entryHeight;
                int blockCenterY = initEntryCenterY + offsetY;

                Point entryCenter = new Point(entryCenterX, blockCenterY);

                if (i == 0)
                {
                    Folder rootFolder = new Folder(entryCenter);
                    allEntries.Add(rootFolder);
                    continue;
                }

                Entry entryToAdd = new Entry(entryCenter);
                allEntries.Add(entryToAdd);
            }
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            base.Draw(graphics, colour, display);

            if (IsSelected == true)
            {
                int alpha = colour.A;
                int red = colour.R;
                int green = colour.G;
                int blue = colour.B;

                red += 50;
                green += 180;

                colour = Color.FromArgb(alpha, red, green, blue);
            }

            foreach (Entry entry in allEntries)
            {
                entry.Draw(graphics, colour, display);
            }

            Rectangle bounds = GetBounds();
            string label = string.Format("Block {0}", BlockNumber);
            Program.DrawText(graphics, colour, label, bounds);

            Size size = bounds.Size;
            Program.DrawRectangle(graphics, colour, Center, size);
        }

        public override Size GetSize()
        {
            Size size = new Size(40, 10);

            int totalEntries = MaxEntries;
            if (totalEntries == 0)
            {
                return size;
            }

            Size entrySize = new Entry(new Point(0, 0)).GetSize();

            int width = entrySize.Width + 2 * padding;
            int height = totalEntries * entrySize.Height + 2 * padding + labelHeight;

            size.Width = width;
            size.Height = height;

            return size;
        }
    }
}