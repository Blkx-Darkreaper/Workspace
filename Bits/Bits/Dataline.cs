using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Dataline : Connector
    {
        public Dataline(Point input, Point output) : base(input, output) { }

        public Dataline(Connector input, Point output)
            : base(input.Output.Center, output)
        {
            input.ConnectTo(this);
        }

        public Dataline(Point input, Connector output)
            : base(input, output.InputPoint)
        {
            ConnectTo(output);
        }

        public Dataline(Connector input, Connector output)
            : base(input.Output.Center, output.InputPoint)
        {
            //ConnectInputTo(input);
            input.ConnectTo(this);
            ConnectTo(output);
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            Point input = InputPoint;
            Point output = Output.Center;

            int heading = (int)Program.GetAbsAngle(input, output);
            int pixelLength = (int)Program.GetDistance(input, output);

            Program.DrawLine(graphics, colour, input, heading, pixelLength);

            base.Draw(graphics, colour, display);
        }
    }
}