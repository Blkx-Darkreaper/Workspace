using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pressure_Puzzle_Maker
{
    public enum Edge { Top, Bottom, Left, Right, None };

    class Tile : PictureBox
    {
        public Point position { get; protected set; }
        protected enum TileType { blank, blocked, invalid, start };
        protected TileType type;
        public bool isBlocked { get { return type.Equals(TileType.blocked); } }
        public bool isStart { get { return type.Equals(TileType.start); } }

        public Edge edge { get; protected set; }
        public bool isEdgeTile { get { return !edge.Equals(Edge.None); } }
        protected bool valid = true;
        public bool isValid { get { return valid; } }

        public Tile(int x, int y)
        {
            this.position = new Point(x, y);
            this.Image = Program.blankImage;
            this.Bounds = new Rectangle(x * Image.Width, y * Image.Height,
                Image.Width, Image.Height);

            SetInvalidAndEdge(x, y);

            this.Invalidate();
        }

        protected void SetInvalidAndEdge(int x, int y)
        {
            int maxX = Program.allTiles.GetLength(0) - 1;
            int maxY = Program.allTiles.GetLength(1) - 1;

            this.edge = Edge.None;

            // Top edge
            if (y == 0)
            {
                // Handle Corners
                if (x == 0 || x == maxX)
                {
                    SetInvalid();
                    return;
                }

                this.edge = Edge.Top;
            }

            // Botoom edge
            if (y == maxY)
            {
                // Handle Corners
                if (x == 0 || x == maxX)
                {
                    SetInvalid();
                    return;
                }

                this.edge = Edge.Bottom;
            }

            // Left edge
            if (x == 0)
            {
                // Handle Corners
                if(y == 0 || y == maxY)
                {
                    SetInvalid();
                    return;
                }

                this.edge = Edge.Left;
            }

            // Right edge
            if (x == maxX)
            {
                // Handle Corners
                if (y == 0 || y == maxY)
                {
                    SetInvalid();
                    return;
                }

                this.edge = Edge.Right;
            }
        }

        protected void SetInvalid()
        {
            this.valid = false;
            this.Image = Program.invalidImage;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            ToggleTile(e);
        }

        public void ToggleTile(MouseEventArgs e)
        {
            if (valid == false)
            {
                return;
            }

            MouseButtons mouseButton = e.Button;

            if (mouseButton.Equals(MouseButtons.Left) == true)
            {
                switch (type)
                {
                    case TileType.blank:
                    case TileType.start:
                        this.type = TileType.blocked;
                        this.Image = Program.blockedImage;
                        Program.allStartTiles.Remove(this);
                        break;

                    case TileType.blocked:
                        if(isEdgeTile == false)
                        {
                            break;
                        }

                        this.type = TileType.start;
                        this.Image = Program.startImage;
                        Program.allStartTiles.Add(this);
                        break;
                }
            }

            if (mouseButton.Equals(MouseButtons.Right) == true)
            {
                if (type.Equals(TileType.start) == true) {
                        Program.allStartTiles.Remove(this);
                }

                this.type = TileType.blank;
                this.Image = Program.blankImage;
            }

            //this.Invalidate();
        }

        public void Draw(Graphics graphics)
        {
            graphics.DrawImage(Image, Bounds);
        }
    }
}