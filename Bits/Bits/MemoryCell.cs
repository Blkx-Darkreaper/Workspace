using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class MemoryCell : DataStructure
    {
        public int CellNumber { get; protected set; }
        public string Value { get; protected set; }
        public string VariableName { get; protected set; }
        protected int labelWidth { get; set; }

        public MemoryCell(Point center, int cellNumber, int maxCells) : base(center) {
            this.CellNumber = cellNumber;
            this.Value = "0";
            this.VariableName = string.Empty;
            int digits = maxCells.ToString().Length;
            this.labelWidth = (int)Math.Floor(10.873f + 6.873f * digits);
        }

        public override Size GetSize()
        {
            Size size = new Size(80 + labelWidth, 20);
            return size;
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            base.Draw(graphics, colour, display);

            // Draw cell number
            Rectangle bounds = GetBounds();
            Program.DrawTextAligned(graphics, colour, CellNumber.ToString(), bounds, (int)Program.Text.Alignment.Middle);

            Size size = bounds.Size;
            Program.DrawRectangle(graphics, colour, Center, size);

            // Draw greyscaleValue
            string text;

            const int binaryFormat = (int)Program.DataFormats.Binary;
            const int decimalFormat = (int)Program.DataFormats.Decimal;
            const int hexFormat = (int)Program.DataFormats.Hexadecimal;
            const int stringFormat = (int)Program.DataFormats.String;

            switch (Program.DataFormat)
            {
                case binaryFormat:
                    text = Program.ConvertToBinary(this.Value);
                    break;

                case decimalFormat:
                    text = Program.ConvertToDecimal(this.Value).ToString();
                    break;

                case hexFormat:
                    text = Program.ConvertToHexadecimal(this.Value);
                    break;

                case stringFormat:
                default:
                    text = string.Empty + this.Value;
                    break;
            }

            Rectangle valueBounds = bounds;
            valueBounds.X += labelWidth;
            valueBounds.Width -= labelWidth;
            Program.DrawRectangle(graphics, colour, valueBounds);

            Program.DrawText(graphics, colour, text, valueBounds, (int)Program.Text.Justified.Center, (int)Program.Text.Alignment.Middle);
        }

        public void StoreData(string value)
        {
            StoreData(value, this.VariableName);
        }

        public void StoreData(string value, string variableName)
        {
            this.Value = value;
            this.VariableName = variableName;
        }

        public void Null()
        {
            this.Value = "0";
        }

        public void Deallocate()
        {
            Null();
            this.VariableName = string.Empty;
        }
    }
}
