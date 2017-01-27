using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class File : Entry
    {
        public File(Point center, string name)
            : base(center, name)
        {
            int bit = (int)FileAttributes.volume;
            this.Attribute += (byte)Math.Pow(2, bit);
        }

        public File(Point center) : this(center, string.Empty) { }
    }
}