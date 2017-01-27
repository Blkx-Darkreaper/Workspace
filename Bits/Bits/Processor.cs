using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Processor : Component
    {
        public string Name { get; protected set; }
        public int WordWidth { get; protected set; } // bits/word
        public int Bits { get; protected set; }
        protected Register programCounter { get; set; }
        protected Register instructionRegister { get; set; }    // not addressable
        protected Register memoryAddressRegister { get; set; }
        protected Register memoryDataRegister { get; set; }
        protected Register stackPointer { get; set; }
        protected Register framePointer { get; set; }
        protected List<Register> allRegisters { get; set; }
        protected ArithmeticLogicUnit alu { get; set; }

        public Processor(Point center, string name, int bits) : base(center) {
            this.Name = name;
            this.WordWidth = bits;
            this.Bits = bits;
            this.allRegisters = new List<Register>();

            Point pcCenter = new Point(center.X, center.Y);
            this.programCounter = new Register("Program Counter", pcCenter, bits);
            allRegisters.Add(programCounter);

            Size registerSize = programCounter.GetSize();
            int height = registerSize.Height + 30;

            Point irCenter = new Point(center.X, pcCenter.Y + height);
            this.instructionRegister = new Register("Instruction Register", irCenter, bits);
            allRegisters.Add(instructionRegister);

            Point marCenter = new Point(center.X, irCenter.Y + height);
            this.memoryAddressRegister = new Register("Memory Address Register", marCenter, bits);
            allRegisters.Add(memoryAddressRegister);

            Point mdrCenter = new Point(center.X, marCenter.Y + height);
            this.memoryDataRegister = new Register("Memory Data Register", mdrCenter, bits);
            allRegisters.Add(memoryDataRegister);

            Point spCenter = new Point(center.X, mdrCenter.Y + height);
            this.stackPointer = new Register("Stack Pointer", spCenter, bits);
            allRegisters.Add(stackPointer);

            Point fpCenter = new Point(center.X, spCenter.Y + height);
            this.framePointer = new Register("Frame Pointer", fpCenter, bits);
            allRegisters.Add(framePointer);

            // Re-adjust center to compensate for added registers
            int centerY = pcCenter.Y + (fpCenter.Y - pcCenter.Y) / 2;

            Point aluCenter = new Point(center.X, fpCenter.Y + height);
            this.alu = new ArithmeticLogicUnit(aluCenter);

            // Re-adjust center to compensate for ALU
            centerY += (alu.GetSize().Height + 30) / 2;

            this.Center = new Point(center.X, centerY);
        }

        public Processor(int x, int y, string name, int bits) : this(new Point(x, y), name, bits) { }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            foreach (Register register in allRegisters)
            {
                register.Draw(graphics, colour, display);
            }

            alu.Draw(graphics, colour, display);

            Rectangle bounds = GetBounds();
            Program.DrawTextJustified(graphics, colour, Name, bounds, (int)Program.Text.Justified.Center);

            Size size = bounds.Size;
            int dashLength = 1;
            int spaceLength = 2;
            Program.DrawDottedRectangle(graphics, colour, Center, size, dashLength, spaceLength);
        }

        public override Size GetSize()
        {
            Size size = new Size(40, 40);

            size.Height += alu.GetSize().Height;
            size.Height += 30;

            int totalRegisters = allRegisters.Count;
            if (totalRegisters == 0)
            {
                return size;
            }

            Register first = allRegisters[0];
            Rectangle firstBounds = first.GetBounds();

            //int registerHeight = firstBounds.Height + 30;
            //size.Height += registerHeight;

            int topLeftCornerX = firstBounds.X;
            int topLeftCornerY = firstBounds.Y;

            Register last = allRegisters[allRegisters.Count - 1];
            Rectangle lastBounds = last.GetBounds();

            int bottomRightCornerX = lastBounds.X + lastBounds.Width;
            int bottomRightCornerY = lastBounds.Y + lastBounds.Height;

            int width = bottomRightCornerX - topLeftCornerX + 50;
            int height = bottomRightCornerY - topLeftCornerY;

            size.Width += width;
            size.Height += height;
            return size;
        }
    }
}