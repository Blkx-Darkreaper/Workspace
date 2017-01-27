using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Drive : Component
    {
        public string Name { get; protected set; }
        public int TotalBytes { get; protected set; } // Bytes
        public int BytesPerBlock { get; protected set; }    // Bytes
        public List<Block> DataBlocks { get; protected set; }
        public int BootSectorStartBlock { get; protected set; }
        public int FATStartBlock { get; protected set; }
        public int DuplicateFATStartBlock { get; protected set; }
        public int RootFolderStartBlock { get; protected set; }
        public int OtherFilesStartBlock { get; protected set; }
        public int BlockToSeek { get; set; }
        public int CurrentBlock { get; protected set; }
        protected Size headSize { get; set; }
        protected float readTimeRemaining { get; set; }
        protected float readDelay { get; set; }
        protected int padding { get; set; }
        protected int labelHeight { get; set; }

        public Drive(Point center, string name, int totalBytes, int bytesPerBlock, int entriesPerBlock) : base(center) {
            this.Name = name;
            this.TotalBytes = totalBytes;
            this.BytesPerBlock = bytesPerBlock;

            int totalBlocks = totalBytes / bytesPerBlock;
            this.DataBlocks = new List<Block>(totalBlocks);
            this.BlockToSeek = 0;
            this.CurrentBlock = 0;
            this.headSize = new Size(20, 10);
            this.readTimeRemaining = 0;
            this.readDelay = 1;
            this.padding = 4;
            this.labelHeight = 9 + 3;   // 9 is actual height of text

            InitBlocks(totalBlocks, entriesPerBlock);
        }

        protected virtual void InitBlocks(int totalBlocks, int entriesPerBlock)
        {
            Size blockSize = new Block(new Point(0, 0), 1, entriesPerBlock).GetSize();
            int blockHeight = blockSize.Height;

            int blockCenterX = this.Center.X;
            int initBlockCenterY = this.Center.Y - (blockHeight / 2) * (totalBlocks - 1) + labelHeight - 3 * padding / 2;

            for (int blockNumber = 0; blockNumber < totalBlocks; blockNumber++)
            {
                int offsetY = blockNumber * blockHeight;
                int blockCenterY = initBlockCenterY + offsetY;

                Point blockCenter = new Point(blockCenterX, blockCenterY);
                Block blockToAdd = new Block(blockCenter, blockNumber, entriesPerBlock);
                DataBlocks.Add(blockToAdd);
            }

            SetBlockSelection(this.CurrentBlock, true);
        }

        protected virtual void Seek()
        {
            if (this.BlockToSeek == this.CurrentBlock)
            {
                SetBlockSelection(this.CurrentBlock, true);
                return;
            }

            SetBlockSelection(this.CurrentBlock, false);
            this.CurrentBlock++;
            SetBlockSelection(this.CurrentBlock, true);
        }

        protected virtual void SetBlockSelection(int blockNumber, bool isSelected)
        {
            Block selectedBlock = this.DataBlocks[blockNumber];
            selectedBlock.IsSelected = isSelected;
        }

        public override void Update(float timeElapsed)
        {
            base.Update(timeElapsed);

            if (readTimeRemaining < 0)
            {
                readTimeRemaining -= timeElapsed;
            }

            if (readTimeRemaining > 0)
            {
                return;
            }

            Seek();

            // reset timer
            readTimeRemaining = readDelay;
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            foreach (Block block in DataBlocks)
            {
                block.Draw(graphics, colour, display);
            }

            Rectangle bounds = GetBounds();
            string label = string.Format("{0}:\\", Name);
            Program.DrawText(graphics, colour, label, bounds);

            Size size = bounds.Size;
            int dashLength = 1;
            int spaceLength = 2;
            Program.DrawDottedRectangle(graphics, colour, Center, size, dashLength, spaceLength);

            DrawHead(graphics, colour, display);
        }

        protected virtual Point GetHeadCenter()
        {
            Rectangle driveBounds = GetBounds();

            int driveCornerX = driveBounds.X;

            int headWidth = headSize.Width;
            int headHeight = headSize.Height;

            int blockCenterY = DataBlocks[CurrentBlock].Center.Y;

            int headCenterX = driveCornerX - headWidth / 2 - 5;
            int headCenterY = blockCenterY;

            Point headCenter = new Point(headCenterX, headCenterY);
            return headCenter;
        }

        protected virtual void DrawHead(Graphics graphics, Color colour, Size display)
        {
            LinkedList<Point> allVertices = new LinkedList<Point>();

            int width = headSize.Width;
            int height = headSize.Height;

            Point headCenter = GetHeadCenter();

            // Top left topRightCorner
            int cornerX = headCenter.X - width / 2;
            int cornerY = headCenter.Y - height / 2;
            allVertices.AddLast(new Point(cornerX, cornerY));

            // Top middle topRightCorner
            int vertexX = headCenter.X;
            int vertexY = cornerY;
            allVertices.AddLast(new Point(vertexX, vertexY));

            // Right middle point
            vertexX = headCenter.X + width / 2;
            vertexY = headCenter.Y;
            allVertices.AddLast(new Point(vertexX, vertexY));

            // Bottom middle topRightCorner
            vertexX = headCenter.X;
            vertexY = headCenter.Y + height / 2;
            allVertices.AddLast(new Point(vertexX, vertexY));

            // Bottom left topRightCorner
            vertexX = headCenter.X - width / 2;
            allVertices.AddLast(new Point(vertexX, vertexY));

            // return to top left topRightCorner
            allVertices.AddLast(new Point(cornerX, cornerY));

            Program.DrawPolygon(graphics, colour, allVertices.ToArray());
        }

        public override Size GetSize()
        {
            Size size = new Size(40, 20);

            int totalBlocks = DataBlocks.Count;
            if (totalBlocks == 0)
            {
                return size;
            }

            Size blockSize = DataBlocks.First().GetSize();

            int width = blockSize.Width + 2 * padding;
            int height = totalBlocks * blockSize.Height + 2 * padding + labelHeight;

            size.Width = width;
            size.Height = height;

            return size;
        }
    }
}
