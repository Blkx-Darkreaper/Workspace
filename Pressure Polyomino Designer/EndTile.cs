using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Polyominoes
{
    [DataContract]
    class EndTile : Tile
    {
        [DataMember]
        public bool IsEndTile { get; protected set; }

        public enum Elec { positive, negative };
        [DataMember]
        public Elec Electrical { get; protected set; }

        public enum Mech { jagged, rounded, square };
        [DataMember]
        public Mech Mechanical { get; protected set; }

        public enum Plm { open, closed, none };
        [DataMember]
        public Plm Plumbing { get; protected set; }

        //public EndTile(int x, int y) : base(x, y)
        //{
        //    this.IsEndTile = true;

        //    this.Electrical = Elec.positive;
        //    this.Mechanical = Mech.jagged;
        //    this.Plumbing = Plm.open;
        //}

        public EndTile(int x, int y) : base(x, y) {
            this.IsEndTile = false;
        }

        public EndTile(int x, int y, Elec elec = Elec.positive, Mech mech = Mech.jagged, Plm plm = Plm.open) : base(x, y)
        {
            this.IsEndTile = true;

            this.Electrical = elec;
            this.Mechanical = mech;
            this.Plumbing = plm;
        }

        [JsonConstructor]
        public EndTile(Point position, Elec elec, Mech mech, Plm plm, bool isEndTile) : base(position.X, position.Y)
        {
            this.IsEndTile = isEndTile;

            this.Electrical = elec;
            this.Mechanical = mech;
            this.Plumbing = plm;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (IsEndTile != true)
            {
                return;
            }

            base.OnMouseClick(e);

            ToggleTile(e);
        }

        public void ToggleTile(MouseEventArgs e)
        {
            MouseButtons mouseButton = e.Button;

            int direction = 0;
            if (mouseButton.Equals(MouseButtons.Left) == true)
            {
                direction = 1;
            }

            if (mouseButton.Equals(MouseButtons.Right) == true)
            {
                direction = -1;
            }

            int current;
            int next;
            Array allValues;

            switch (Program.puzzleType)
            {
                case Program.PuzzleType.Elec:
                    current = (int)Electrical;

                    allValues = Enum.GetValues(typeof(Elec));

                    next = current + direction;
                    if (next < 0) next += allValues.Length;
                    next %= allValues.Length;

                    Electrical = (Elec)allValues.GetValue(next);
                    break;

                case Program.PuzzleType.Mech:
                    current = (int)Mechanical;

                    allValues = Enum.GetValues(typeof(Mech));

                    next = current + direction;
                    if (next < 0) next += allValues.Length;
                    next %= allValues.Length;

                    Mechanical = (Mech)allValues.GetValue(next);
                    break;

                case Program.PuzzleType.Plm:
                    current = (int)Plumbing;

                    allValues = Enum.GetValues(typeof(Plm));

                    next = current + direction;
                    if (next < 0) next += allValues.Length;
                    next %= allValues.Length;

                    Plumbing = (Plm)allValues.GetValue(next);
                    break;
            }
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);

            if (IsEndTile != true)
            {
                return;
            }

            Point centerPoint = new Point(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);

            Pen black = Pens.Black;

            switch (Program.puzzleType)
            {
                case Program.PuzzleType.Elec:
                    graphics.DrawLine(black, centerPoint.X - 5, centerPoint.Y, centerPoint.X + 5, centerPoint.Y);

                    if (Electrical.Equals(Elec.positive) == true)
                    {
                        // +
                        graphics.DrawLine(black, centerPoint.X, centerPoint.Y - 5, centerPoint.X, centerPoint.Y + 5);
                    }
                    break;

                case Program.PuzzleType.Mech:
                    if (Mechanical.Equals(Mech.jagged) == true)
                    {
                        int totalSpikes = 4;
                        int spikeHeight = (Tile.size - 1) / totalSpikes;
                        int spikeWidth = spikeHeight / 2;

                        int y = Bounds.Y;

                        for (int i = 0; i < totalSpikes; i++)
                        {
                            int x = Bounds.X + 2 + spikeHeight * i;

                            graphics.DrawLine(black, x, y, x + spikeWidth, y + spikeHeight);
                            graphics.DrawLine(black, x + spikeWidth, y + spikeHeight, x + spikeWidth * 2, y);
                        }
                    }

                    if (Mechanical.Equals(Mech.rounded) == true)
                    {
                        int totalBumps = 4;
                        int bumpRadius = (Tile.size - 1) / (totalBumps * 2);
                        int bumpDiameter = 2 * bumpRadius;

                        int y = Bounds.Y - 2;

                        for (int i = 0; i < totalBumps; i++)
                        {
                            int x = Bounds.X + 2 + bumpDiameter * i + i;

                            Rectangle circle = new Rectangle(x, y, bumpDiameter, bumpDiameter);
                            graphics.DrawArc(black, circle, 0, 180);
                        }
                    }

                    if (Mechanical.Equals(Mech.square) == true)
                    {
                        int totalMerlons = 3;
                        int merlonWidth = (Tile.size - 1) / (totalMerlons * 2 + 1);
                        int merlonHeight = merlonWidth + 2;

                        for (int i = 0; i < totalMerlons; i++)
                        {
                            int x = Bounds.X + 2 + merlonWidth + merlonWidth * i * 2;
                            int y = Bounds.Y;
                            graphics.DrawLine(black, x, y, x, y + merlonHeight);
                            graphics.DrawLine(black, x, y + merlonHeight, x + merlonWidth, y + merlonHeight);
                            graphics.DrawLine(black, x + merlonWidth, y + merlonHeight, x + merlonWidth, y);
                        }
                    }
                    break;

                case Program.PuzzleType.Plm:
                    if (Plumbing.Equals(Plm.none) == true)
                    {
                        break;
                    }

                    int margin = Tile.size / 4;

                    // O
                    Rectangle rect = new Rectangle(Bounds.X + margin, Bounds.Y + margin, Bounds.Width - 2 * margin, Bounds.Height - 2 * margin);
                    graphics.DrawEllipse(black, rect);

                    if (Plumbing.Equals(Plm.closed) == true)
                    {
                        // Ø
                        graphics.DrawLine(black, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                    }
                    break;
            }
        }
    }
}