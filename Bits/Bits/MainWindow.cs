using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/*
    Notes:
    Big Endian - later portion of data is stored last
    Little Endian - later portion of data is stored first ie The number 0x5ADA is stored in memory location 0x0010. This means that memory location 0x0010 has the number 0xDA and the memory location 0x0011 has the number 0x5A
 */

namespace Bits
{
    public partial class MainWindow : Form
    {
        protected Connector root { get; set; }
        protected LinkedList<Connector> allControlLines { get; set; }
        //protected LinkedList<Content> allData { get; set; }
        public static Timer timer { get; set; }
        protected static int totalTime { get; set; }
        protected static bool debugMode { get; set; }
        protected Processor processor { get; set; }
        protected Memory memory { get; set; }
        protected Drive drive { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Start();
        }

        public void Start()
        {
            Display.Width = 40;
            Display.Height = 40;

            Program.IsDisplayingVarNames = true;
            Program.DataFormat = (int)Program.DataFormats.String;

            timer = new Timer();
            timer.Tick += new EventHandler(Refresh);
            timer.Interval = 100;
            timer.Start();

            totalTime = 0;

            Assembler.BuildInstructionSet();

            //debugMode = true; //debug

            //Display.Paint -= Update;    // testing
            //Display.Paint += Whiteboard;    //testing

            BuildProcessor();
            BuildDrive();
            BuildMemory();
        }

        private void BuildDrive()
        {
            int totalBytes = 5*1024;  // Kb
            int bytesPerBlock = 256;
            int entriesPerBlock = 16;
            drive = new Drive(new Point(80, 3400), "A", totalBytes, bytesPerBlock, entriesPerBlock);

            Size size = drive.GetSize();
            int width = size.Width;
            Display.Width += width;

            int height = size.Height;
            Display.Height += height;
        }

        private void BuildMemory()
        {
            int totalBytes = 128;
            int bytesPerCell = 1;
            memory = new Memory(new Point(220, 1310), totalBytes, bytesPerCell);

            Size size = memory.GetSize();
            int width = size.Width + (220-80);
            Display.Width += width;
        }

        private void BuildProcessor()
        {
            string name = "Central Processor";
            int bits = 5;
            processor = new Processor(new Point(600, 100), name, bits);

            Size size = processor.GetSize();
            int width = size.Width + (600 - 220);
            Display.Width += width;
        }

        //private void BuildProcessor()
        //{
        //    allControlLines = new LinkedList<Connector>();

        //    int height;

        //    //Register registerA = new Register("A", 100, 100, 0);
        //    //headHeight = registerA.GetBounds().Height;
        //    //Display.Height += headHeight;

        //    //Register registerB = new Register("B", 100, 150, 1);
        //    //headHeight = registerB.GetBounds().Height;
        //    //Display.Height += headHeight;

        //    //Register registerC = new Register("C", 125, 200, 2);
        //    //headHeight = registerC.GetBounds().Height;
        //    //Display.Height += headHeight;

        //    //Register pc = new Register("D", 150, 250, 3);
        //    //headHeight = pc.GetBounds().Height;
        //    //Display.Height += headHeight;

        //    Register pc = new Register("Program Counter", 150, 100, 4);
        //    height = pc.GetSize().Height;
        //    Display.Height += height;
        //    pc.SetInputOpen(false);

        //    int inputX = pc.InputPoint.X;
        //    int inputY = pc.InputPoint.Y;

        //    root = new Dataline(new Point(inputX, 0), pc);

        //    Register mar = new Register("Memory Address Register", 150, 250, 4);
        //    height = pc.GetSize().Height;
        //    Display.Height += height;

        //    Register mdr = new Register("Memory Data Register", 150, 400, 4);
        //    height = pc.GetSize().Height;
        //    Display.Height += height;

        //    //Dataline toNextRegisterLine = new Dataline(root, new Point(inputX, inputY + 50));
        //    Dataline toNextRegisterLine = new Dataline(root, mar);

        //    int outputX = pc.Output.Center.X;
        //    int outputY = pc.Output.Center.Y;
        //    //Connector outputLine = new Dataline(pc, new Point(outputX, outputY));
        //    Connector outputLine = new Dataline(pc, new Point(outputX, outputY + 150));

        //    Connector toALULine = new Dataline(outputLine, new Point(outputX, outputY + 150 + 50));
        //    mar.ConnectTo(toALULine);

        //    //BitCell cell1 = new BitCell(100, 50, 2);
        //    //root.ConnectTo(cell1);
        //    //allControlLines.AddLast(cell1);

        //    //Dataline line1 = new Dataline(cell1, new Point(150, 70));
        //    //allControlLines.AddLast(line1);

        //    //BitCell cell2 = new BitCell(150, 50, 1);
        //    //line1.ConnectTo(cell2);
        //    //allControlLines.AddLast(cell2);

        //    //Dataline line2 = new Dataline(cell2, new Point(200, 70));
        //    //allControlLines.AddLast(line2);

        //    //BitCell cell3 = new BitCell(200, 50, 0);
        //    //line2.ConnectTo(cell3);
        //    //allControlLines.AddLast(cell3);

        //    //Dataline lineOut = new Dataline(cell3, new Point(250, 70));

        //    float maxVelocity = Program.MAX_VELOCITY;
        //    float cruiseVelocity = Program.CRUISE_VELOCITY;

        //    //Content bit = new DataBit(root.InputPoint, cruiseVelocity, maxVelocity);
        //    //root.ReceiveData(bit);
        //    //bit.Cruise();

        //    //Content dataByte = new DataByte(root.InputPoint, cruiseVelocity, maxVelocity);
        //    //root.ReceiveData(dataByte);
        //    //dataByte.Cruise();

        //    //Content hex = new DataHex(root.InputPoint, cruiseVelocity, maxVelocity);
        //    //root.ReceiveData(hex);
        //    //hex.Cruise();

        //    //allData = new LinkedList<Content>();

        //    //Bit bit = new Bit(cell1, 5);
        //    //allData.AddLast(bit);

        //    //BitContainer empty = new BitContainer(100, 100);
        //    //allControlLines.AddLast(empty);

        //    //BitContainer full = new BitContainer(140, 100);
        //    //full.IsFull = true;
        //    //allControlLines.AddLast(full);

        //    //ConnectBitContainers(full, empty);
        //}

        private void Whiteboard(object sender, PaintEventArgs e)
        {
            Display.Width = 300;
            Display.Height = 200;

            Size display = Display.Size;
            Color colour = Color.Blue;
            Graphics graphics = e.Graphics;

            // Advanced
            // Data lines
            Program.DrawLine(graphics, colour, new Point(20, 0), new Point(20, display.Height));
            Program.DrawLine(graphics, colour, new Point(25, 0), new Point(25, display.Height));

            Program.DrawLine(graphics, colour, new Point(45, 0), new Point(45, 85));
            Program.DrawLine(graphics, colour, new Point(50, 0), new Point(50, 85));
            Program.DrawLine(graphics, colour, new Point(45, 85), new Point(50, 85));

            // Gate
            Program.DrawRectangle(graphics, colour, new Point(55, 92), new Size(10, 25));
            Program.DrawLine(graphics, colour, new Point(55, 80), new Point(55, 0));

            // Content lines
            Program.DrawLine(graphics, colour, new Point(60, 80), new Point(110, 80));
            Program.DrawLine(graphics, colour, new Point(60, 85), new Point(115, 85));
            Program.DrawLine(graphics, colour, new Point(60, 80), new Point(60, 85));

            Program.DrawLine(graphics, colour, new Point(45, 105), new Point(200, 105));
            Program.DrawLine(graphics, colour, new Point(45, 105), new Point(45, display.Height));
            Program.DrawLine(graphics, colour, new Point(50, 110), new Point(200, 110));
            Program.DrawLine(graphics, colour, new Point(50, 110), new Point(50, display.Height));

            // Bit Container
            Program.DrawLine(graphics, colour, new Point(110, 80), new Point(110, 71));
            Program.DrawArc(graphics, colour, new Point(125, 45), 30, 210, 300);
            Program.DrawLine(graphics, colour, new Point(140, 80), new Point(140, 71));
            Program.DrawCircle(graphics, colour, new Point(125, 45), 5);
            Program.DrawLine(graphics, colour, new Point(115, 85), new Point(115, 68));
            Program.DrawArc(graphics, colour, new Point(125, 45), 25, 205, 310);
            Program.DrawLine(graphics, colour, new Point(135, 85), new Point(135, 68));

            // Content lines
            Program.DrawLine(graphics, colour, new Point(140, 80), new Point(200, 80));
            Program.DrawLine(graphics, colour, new Point(135, 85), new Point(200, 85));

            // Bits, Bytes, and Hexes
            //Program.DrawCircle(e.Graphics, colour, new Point(125, 32), 4);
            //Program.DrawCircle(e.Graphics, colour, new Point(138, 45), 4);
            //Program.DrawCircle(e.Graphics, colour, new Point(125, 58), 4);
            //Program.DrawCircle(e.Graphics, colour, new Point(112, 45), 4);
            Program.DrawHexagon(e.Graphics, colour, new Point(125, 30), 0, 10);
            Program.DrawHexagon(e.Graphics, colour, new Point(140, 45), 0, 10);
            Program.DrawHexagon(e.Graphics, colour, new Point(125, 60), 0, 10);
            Program.DrawHexagon(e.Graphics, colour, new Point(110, 45), 0, 10);

            Program.DrawHexagon(e.Graphics, colour, new Point(35, 40), 0, 10);
            Program.DrawSquare(e.Graphics, colour, new Point(35, 60), 45, 8);
            Program.DrawCircle(e.Graphics, colour, new Point(35, 80), 3);

            Program.DrawHexagon(e.Graphics, colour, new Point(80, 150), 0, 30);
        }

        protected void Refresh(object sender, EventArgs e)
        {
            Display.Refresh();
            //Display.Invalidate();
        }

        protected void Update(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Size display = Display.Size;
            Color colour = Color.Blue;

            int updateInterval = timer.Interval;
            float timeElapsed = updateInterval / 100f;

            totalTime += (int)timeElapsed;

            //root.Update(timeElapsed);
            //foreach (Connector controlLine in allControlLines)
            //{
            //    controlLine.Update(timeElapsed);
            //}

            //if (debugMode == true)
            //{
            //    string debug = root.Debug();
            //    timer.Stop();
            //    Console.Write(debug);
            //    return;
            //}

            //root.Draw(graphics, colour, display);
            //foreach (Connector controlLine in allControlLines)
            //{
            //    controlLine.Draw(graphics, colour, display);
            //}

            processor.Draw(graphics, colour, display);

            memory.Draw(graphics, colour, display);

            drive.Draw(graphics, colour, display);

            //int CRUISE_VELOCITY = 2;
            //int MAX_VELOCITY = 5;
            //if (totalTime < 120)
            //{
            //    if (totalTime % 16 == 0)
            //    {
            //        Content bit = new DataBit(root.InputPoint, CRUISE_VELOCITY, MAX_VELOCITY);
            //        root.ReceiveData(bit);
            //        bit.Cruise();
            //    }
            //}

            //if (totalTime % 16 == 0)
            //{
            //    BitCell cell1 = (BitCell)allControlLines.ElementAt(1);
            //    if (cell1.IsFull() == true)
            //    {
            //        cell1.DischargeStoredData();
            //    }
            //}

            //if (totalTime > 100)
            //{
            //    if (totalTime < 115)
            //    {
            //        BitCell cell1 = (BitCell)allControlLines.ElementAt(1);
            //        if (cell1.IsFull() == true)
            //        {
            //            cell1.DischargeStoredData();
            //        }
            //    }
            //}

            //foreach (Branches inputLine in allControlLines)
            //{
            //    inputLine.Update(timeElapsed);
            //    inputLine.Draw(graphics, colour, display);
            //}

            //foreach (Content bufferData in allData)
            //{
            //    bufferData.Update(timeElapsed);
            //    bufferData.Draw(graphics, colour, display);
            //}

            // Reference totalPoints
            //Pen pen = new Pen(new SolidBrush(Color.Red));
            //Rectangle rect;

            //rect = new Rectangle(100-1, 30-1, 2, 2);
            //graphics.DrawRectangle(pen, rect);

            //rect = new Rectangle(100-1, 50-1, 2, 2);
            //graphics.DrawRectangle(pen, rect);

            //rect = new Rectangle(100-1, 70-1, 2, 2);
            //graphics.DrawRectangle(pen, rect);
        }

        //public void ConnectBitContainers(BitContainer container1, BitContainer container2)
        //{
        //    Point start = container1.OutputCenter;
        //    Point end = container2.InputCenter;

        //    LinkedList<Point> allVertices = new LinkedList<Point>();

        //    int nextX = start.X;
        //    int nextY = start.Y + 20;
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
        //        allControlLines.AddLast(line);

        //        lastConnector = line;
        //    }

        //    lastConnector.ConnectTo(container2);
        //}

        protected void Execute_Click(object sender, EventArgs e)
        {
            string input = Input.Text;
            Input.Text = string.Empty;

            string message = Assembler.Parse(input);
            StringReader reader = new StringReader(message);

            string nextLine = reader.ReadLine();
            while (nextLine != null)
            {
                InputDisplay.AppendText(nextLine);
                InputDisplay.AppendText(Environment.NewLine);

                nextLine = reader.ReadLine();
            }
        }
    }
}