using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Entry : DataStructure   // 32 Bytes
    {
        public string Name { get; protected set; }  // 88 Bytes 8.3
        public byte Attribute { get; protected set; }   // 8 bits
        public byte Reserved { get; protected set; }    // 8 bits
        public string TimeCreated { get; protected set; }   // 24 bits
        public string DateCreated { get; protected set; }   // 16 bits
        public string DateLastAccessed { get; protected set; }  // 16 bits
        public int OtherReserved { get; protected set; }    // 16 bits
        public string TimeLastModified { get; protected set; }  // 16 bits
        public string DateLastModified { get; protected set; }  // 16 bits
        public int StartOfEntryBlockNumber { get; protected set; }   // 16 bits
        public int FileSize { get; protected set; } // 32 bits
        protected enum FileAttributes
        {
            directory, 
            volume,

            // User modifiable bits
            archive,
            system,
            hidden,
            readOnly
        }

        public Entry(Point center, string name)
            : base(center)
        {
            this.Name = name;
            this.Attribute = (byte)0;
        }

        public Entry(Point center) : this(center, string.Empty) { }

        public override Size GetSize()
        {
            Size size = new Size(80, 20);
            return size;
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            base.Draw(graphics, colour, display);

            Rectangle bounds = GetBounds();
            Program.DrawRectangle(graphics, colour, bounds);

            Program.DrawText(graphics, colour, Name, bounds);
        }
    }
}