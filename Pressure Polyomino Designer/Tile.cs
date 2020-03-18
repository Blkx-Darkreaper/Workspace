using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Polyominoes
{
    [DataContract]
    class Tile : PictureBox
    {
        public static int size = 50;
        [DataMember]
        public Point Position { get; protected set; }

        //[JsonConstructor]
        public Tile(int x, int y)
        {
            this.Position = new Point(x, y);
            this.Image = Program.tileImage;
            this.Bounds = new Rectangle(x * Image.Width, y * Image.Height,
                Image.Width, Image.Height);
        }

        public virtual void Draw(Graphics graphics)
        {
            graphics.DrawImage(Image, Bounds);
        }
    }
}