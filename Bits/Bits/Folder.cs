using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Folder : Entry
    {
        public Folder(Point center, string name)
            : base(center, name)
        {
            int bit = (int)FileAttributes.directory;
            this.Attribute += (byte)Math.Pow(2, bit);
        }

        public Folder(Point center) : this(center, string.Empty) { }
    }
}