using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Register : Component
    {
        protected LinkedList<BitCell> allBitCells = new LinkedList<BitCell>();
        protected Gate inputGate { get; set; }
        //protected Dataline lineFromInput { get; set; }
        protected Gate outputGate { get; set; }
        public string Name { get; protected set; }
        public int Value { get; set; }

        public Register(string name, Point center, int bits)
            : base(center)
        {
            this.Name = name;
            this.Value = 0;

            AddBitCells(bits);
            AddIOLines(center, bits);
        }

        public Register(string name, int x, int y, int bits) : this(name, new Point(x, y), bits) { }

        protected void AddBitCells(int bits)
        {
            if (bits == 0)
            {
                return;
            }

            int offsetX = (bits - 1) * 25;
            int centerY = Center.Y;
            BitCell previousBitCell = null;
            BitCell nextBitCell = null;

            for (int i = 0; i < bits; i++)
            {
                int bitNumber = bits - i - 1;
                int centerX = Center.X - offsetX + i * 50;

                nextBitCell = new BitCell(centerX, centerY, bitNumber);
                allBitCells.AddLast(nextBitCell);

                if (previousBitCell != null)
                {
                    Dataline connectorLine = new Dataline(previousBitCell, nextBitCell);
                }

                previousBitCell = nextBitCell;
            }
        }

        protected void AddIOLines(Point center, int bits)
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;

            int inputX = center.X - width / 2 - 15;
            int inputY = center.Y + 40;
            if (bits == 0)
            {
                inputY = center.Y + height / 4;
            }

            this.InputPoint = new Point(inputX, inputY);
            this.inputGate = new Gate(InputPoint);
            //this.inputGate.SetIsOpen(false);    // testing

            int outputX = inputX + width + 30;
            int outputY = inputY;
            Point outputPoint = new Point(outputX, outputY);
            this.Output = new OutputConnection(outputPoint);

            this.outputGate = new Gate(outputPoint);
            this.outputGate.SetIsOpen(false);

            //Connector nextConnector = this.Output.GetNextConnector();
            //this.outputGate.ConnectTo(nextConnector);

            Dataline lineFromInput;
            if (bits == 0)
            {
                lineFromInput = new Dataline(this.inputGate, this.outputGate);
                //lineFromInput.ConnectTo(this.Output.Branches);
                return;
            }

            BitCell firstBitCell = allBitCells.ElementAt(0);
            lineFromInput = new Dataline(this.inputGate, firstBitCell);

            BitCell lastBitCell = allBitCells.ElementAt(allBitCells.Count - 1);
            Dataline lineToOutput = new Dataline(lastBitCell, this.outputGate);
        }

        public override bool IsReceiving()
        {
            return inputGate.IsOpen;
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            Rectangle bounds = GetBounds();

            Size size = bounds.Size;
            int width = size.Width;
            int height = size.Height;

            int dashLength = 1;
            int spaceLength = 2;
            Program.DrawDottedRectangle(graphics, colour, Center, size, dashLength, spaceLength);

            int cornerX = Center.X - width / 2;
            int cornerY = Center.Y - height / 2;
            Point topLeftCorner = new Point(cornerX, cornerY);

            //string label = string.Format("{0}= {1}", Name, Value);
            //Program.DrawText(graphics, colour, label, topLeftCorner);

            Program.DrawText(graphics, colour, Name, bounds);

            cornerX += width;
            Point topRightCorner = new Point(cornerX, cornerY);

            Program.DrawTextJustified(graphics, colour, Value.ToString(), bounds, (int)Program.Text.Justified.Right);

            this.inputGate.Draw(graphics, colour, display);
            //lineFromInput.Draw(graphics, colour, display);

            //foreach (BitCell bit in allBitCells)
            //{
            //    bit.Draw(graphics, colour, display);
            //}
        }

        public override void Update(float timeElapsed)
        {
            this.inputGate.Update(timeElapsed);
            //lineFromInput.Update(timeElapsed);

            int totalBits = 0;
            foreach (BitCell cell in allBitCells)
            {
                int value = cell.GetValue();
                totalBits += value;
            }

            this.Value = totalBits;
        }

        public override string Debug(string debug)
        {
            debug = AddDebug(debug);

            debug += " {";

            Connector nextConnector = this.inputGate;
            debug = DebugNext(debug, nextConnector);

            //debug += "}";

            return debug;
        }

        public override void ReceiveData(Data data)
        {
            this.inputGate.ReceiveData(data);
            //lineFromInput.ReceiveData(data);
        }

        public virtual void SetInputOpen(bool isOpen)
        {
            inputGate.SetIsOpen(isOpen);
        }

        public override Rectangle GetBounds()
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;

            return GetBounds(width, height);
        }

        public override Size GetSize()
        {
            Size size = new Size(40, 40);

            int totalBits = allBitCells.Count;
            if (totalBits == 0)
            {
                return size;
            }

            Size bitCellSize = allBitCells.First().GetSize();

            int width = totalBits * bitCellSize.Width + 10 * (totalBits - 1);
            int height = bitCellSize.Height;

            size.Width += width;
            size.Height += height;

            return size;
        }

        //protected void AddBitCells()
        //{
        //    Point centrum = new Point(Center.X, Center.Y);

        //    int DataSlots = allByteIndexes.Count;

        //    BitContainer nextBitCell;
        //    if (DataSlots == 0)
        //    {
        //        nextBitCell = new BitContainer(centrum);
        //        allByteIndexes.AddLast(nextBitCell);
        //        return;
        //    }

        //    int offsetX = DataSlots * 40;

        //    centrum.X += offsetX;

        //    nextBitCell = new BitContainer(centrum);
        //    BitContainer previousBit = allByteIndexes.Last();

        //    ConnectBitContainers(previousBit, nextBitCell);

        //    allByteIndexes.AddLast(nextBitCell);

        //    AddIOLines(Center);
        //}

        //protected void ConnectBitContainers(BitContainer container1, BitContainer container2)
        //{
        //    Point start = container1.OutputCenter;
        //    Point end = container2.InputCenter;

        //    LinkedList<Point> allVertices = new LinkedList<Point>();

        //    int nextX, nextY;

        //    nextX = start.X;
        //    nextY = start.Y + 20;
        //    allVertices.AddLast(new Point(nextX, nextY));

        //    nextX += (end.X - start.X) / 2;
        //    allVertices.AddLast(new Point(nextX, nextY));

        //    nextY = end.Y - 20;
        //    allVertices.AddLast(new Point(nextX, nextY));

        //    nextX = end.X;
        //    allVertices.AddLast(new Point(nextX, nextY));

        //    allVertices.AddLast(end);

        //    Point previous = start;
        //    Branches lastConnector = container1;
        //    foreach (Point nextBitCell in allVertices)
        //    {
        //        Dataline line = new Dataline(previous, nextBitCell);
        //        previous = nextBitCell;

        //        lastConnector.ConnectTo(line);

        //        lastConnector = line;
        //    }

        //    lastConnector.ConnectTo(container2);
        //}
    }
}