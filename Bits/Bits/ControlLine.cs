using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class ControlLine : Connector
    {
        public Point StartPoint {get; protected set;}
        public Point EndPoint {get; protected set;}
        protected Point[] allPoints { get; set; }
        protected bool isActive { get; set; }
        protected Component receiver { get; set; }

        public ControlLine(Point[] allPoints, Component receiver)
            : base(allPoints.ElementAt(0), allPoints.ElementAt(allPoints.Count() - 1))
        {
            this.allPoints = allPoints;
            this.StartPoint = allPoints.ElementAt(0);
            int lastIndex = allPoints.Count() - 1;
            this.EndPoint = allPoints.ElementAt(lastIndex);
            this.isActive = false;
            this.receiver = receiver;
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            colour = Color.Gray;

            if (isActive == true)
            {
                colour = Color.Yellow;
            }
            
            Point previousPoint = StartPoint;

            int totalPoints = allPoints.Count();
            for (int i = 1; i < totalPoints; i++)
            {
                Point nextPoint = allPoints.ElementAt(i);
                Program.DrawLine(graphics, colour, previousPoint, nextPoint);

                previousPoint = nextPoint;
            }
        }

        public virtual void SendSignal()
        {
            isActive = true;
        }
    }
}