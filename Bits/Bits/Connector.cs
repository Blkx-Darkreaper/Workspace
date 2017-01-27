using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Connector : Entity
    {
        public Point InputPoint { get; protected set; }
        public OutputConnection Output { get; protected set; }
        //public bool IsReceiving { get; protected set; }
        protected bool hasData { get; set; }
        protected LinkedList<Data> allData { get; set; }
        protected LinkedList<Data> allDataToBeRemoved { get; set; }
        //public enum PathType { Straight = 0, BitNode };

        public Connector(Point input, Point output) : base() {
            this.InputPoint = input;
            this.Output = new OutputConnection(output);
            //this.IsReceiving = true;
            this.hasData = false;
            this.allData = new LinkedList<Data>();
            this.allDataToBeRemoved = new LinkedList<Data>();
        }

        public Connector(int inputX, int inputY, int outputX, int outputY) : this(new Point(inputX, inputY), new Point(outputX, outputY)) { }

        //public virtual void ConnectInputTo(Connector other) {
        //    if (other == null)
        //    {
        //        return;
        //    }

        //    OutputConnection otherOutput = other.Output;
        //    Point contactPoint = otherOutput.Center;

        //    StartPoint.AddConnection(contactPoint, other);
        //    //otherOutput.AddConnection(StartPoint.Branches);
        //    otherOutput.AddConnection(this);
        //}

        //public virtual void DisconnectInput()
        //{
        //    if (StartPoint == null)
        //    {
        //        return;
        //    }

        //    Connector other = StartPoint.Branches;
        //    if (other != null)
        //    {
        //        other.Output.RemoveConnection();
        //    }

        //    StartPoint.RemoveConnection();
        //}

        public virtual void ConnectTo(Connector other)
        {
            if (other == null)
            {
                return;
            }

            //OutputConnection otherInput = other.StartPoint;
            Point contactPoint = Output.Center;

            Output.AddConnection(other);
            //otherInput.AddConnection(contactPoint, this);
        }

        public virtual void DisconnectOutput()
        {
            if (Output == null)
            {
                return;
            }

            //Connector other = Output.Branches;
            //if (other != null)
            //{
            //    other.StartPoint.RemoveConnection();
            //}

            Output.RemoveConnection(this);
        }

        public virtual bool IsReceiving()
        {
            return true;
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            DrawData(graphics, colour, display);
            DrawConnected(graphics, colour, display);
        }

        protected virtual void DrawData(Graphics graphics, Color colour, Size display)
        {
            foreach (Data data in allData)
            {
                data.Draw(graphics, colour, display);
            }
        }

        protected virtual void DrawConnected(Graphics graphics, Color colour, Size display)
        {
            foreach (Connector nextConnector in Output.Branches)
            {
                if (nextConnector == null)
                {
                    return;
                }

                nextConnector.Draw(graphics, colour, display);
            }
        }

        public virtual string Debug()
        {
            string debug = string.Empty;

            return this.Debug(debug);
        }

        public virtual string Debug(string debug)
        {
            debug = AddDebug(debug);

            foreach (Connector nextConnector in Output.Branches)
            {
                debug = DebugNext(debug, nextConnector);
            }

            return debug;
        }

        protected virtual string AddDebug(string debug)
        {
            debug += string.Format("\nId{0}: {1} Input({2},{3}), Output({4},{5})", Id, this.GetType().Name, 
                this.InputPoint.X, this.InputPoint.Y, this.Output.Center.X, this.Output.Center.Y);
            return debug;
        }

        protected virtual string DebugNext(string debug, Connector nextConnector)
        {
            if (nextConnector == null)
            {
                return debug;
            }

            return nextConnector.Debug(debug);
        }

        public override void Update(float timeElapsed)
        {
            UpdateData(timeElapsed);
            UpdateConnected(timeElapsed);
        }

        protected virtual void UpdateData(float timeElapsed)
        {
            foreach (Data data in allData)
            {
                data.Update(timeElapsed);
                bool reachedWayPoint = data.WayPointReached(data.Center);
                if (reachedWayPoint == false)
                {
                    continue;
                }

                TransferData(data);
            }

            foreach (Data data in allDataToBeRemoved)
            {
                allData.Remove(data);
            }

            allDataToBeRemoved.Clear();
        }

        protected virtual void UpdateConnected(float timeElapsed)
        {
            foreach (Connector nextConnector in Output.Branches)
            {
                if (nextConnector == null)
                {
                    return;
                }

                nextConnector.Update(timeElapsed);
            }
        }

        protected virtual void TransferData(Data data)
        {
            allDataToBeRemoved.AddLast(data);

            SetHasData();

            Output.SendData(data);
        }

        //protected virtual void SendData(Content data) {
        //    Connector nextConnector = Output.Branches;
        //    if (nextConnector == null)
        //    {
        //        data.Stop();
        //        return;
        //    }

        //    nextConnector.ReceiveData(data);
        //}

        protected virtual void SetHasData()
        {
            if (allData.Count > 0)
            {
                hasData = true;
            }

            if (allDataToBeRemoved.Count < allData.Count)
            {
                return;
            }

            hasData = false;
        }

        public virtual void ReceiveData(Data data)
        {
            allData.AddLast(data);
            data.AddWayPoint(this.Output.Center);
            data.Cruise();

            SetHasData();
        }

        protected virtual int GetLargestDataValue(int totalBits)
        {
            int largestDataValue = 64;
            while (totalBits % largestDataValue != 0)
            {
                largestDataValue /= 8;
            }

            return largestDataValue;
        }

        //public virtual int GetPathType()
        //{
        //    return (int)PathType.Straight;
        //}

        //public virtual Point GetNextWayPoint(Point currentWayPoint)
        //{
        //    return Output.Center;
        //}
    }

    public class OutputConnection : Entity
    {
        public Point Center { get; protected set; }
        public LinkedList<Connector> Branches { get; protected set; }

        public OutputConnection(Point center)
        {
            this.Center = center;
            this.Branches = new LinkedList<Connector>();
        }

        public void AddConnection(Connector connector)
        {
            if(connector == null) {
                return;
            }

            this.Center = connector.InputPoint;
            this.Branches.AddLast(connector);
        }

        //public void AddConnection(Connector inputLine)
        //{
        //    this.Branches = inputLine;
        //}

        //public void AddConnection(Point point, Connector inputLine)
        //{
        //    this.Center = point;
        //    AddConnection(inputLine);
        //}

        public void RemoveConnection(Connector connector)
        {
            this.Branches.Remove(connector);
        }

        public Connector GetNextConnector()
        {
            if (Branches.Count == 0)
            {
                return null;
            }

            Connector nextConnector = Branches.ElementAt(0);
            return nextConnector;
        }

        public void SendData(Data data)
        {
            foreach (Connector nextConnector in Branches)
            {
                if (nextConnector == null)
                {
                    data.Stop();
                    return;
                }

                bool isReceiving = nextConnector.IsReceiving();
                if (isReceiving == false)
                {
                    continue;
                }

                nextConnector.ReceiveData(data);
                return;
            }
        }
    }
}