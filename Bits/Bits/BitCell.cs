using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class BitCell : Component
    {
        protected BitNode node { get; set; }
        protected DataCapacitor buffer { get; set; }
        protected Dataline fromInputLine { get; set; }
        protected Dataline toOutputLine { get; set; }
        public int SlotValue { get; protected set; }

        public BitCell(Point center, int bitNumber)
            : base(center)
        {
            int bitNodeRadius = 20;

            if (bitNumber < 0)
            {
                bitNumber = 0;
            }

            int bitCapacity = (int)Math.Pow(2, bitNumber);

            int slotValue = GetLargestDataValue(bitCapacity);
            this.SlotValue = slotValue;
            int dataSlots = bitCapacity / slotValue;
            
            int nodeCenterY = center.Y - bitNodeRadius;
            this.node = new BitNode(center.X, nodeCenterY, dataSlots);

            int ioY = center.Y + bitNodeRadius * 2;
            this.InputPoint = new Point(center.X, ioY);

            //int outputX = centrum.X + bitNodeRadius;
            //Point outputPoint = new Point(outputX, centrum.Y);
            Point outputPoint = new Point(center.X, ioY);
            this.Output = new OutputConnection(outputPoint);

            Connector sink = this.Output.GetNextConnector();
            this.buffer = new DataCapacitor(this.InputPoint, slotValue, bitCapacity, sink, Program.OUTPUT_DELAY);
            this.fromInputLine = new Dataline(this.buffer, this.node);
            this.toOutputLine = new Dataline(this.node, this.Output.Center);
        }

        public BitCell(int centerX, int centerY, int bitNumber) :
            this(new Point(centerX, centerY), bitNumber) { }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            buffer.Draw(graphics, colour, display);
            //fromInputLine.Draw(graphics, colour, display);
            //node.Draw(graphics, colour, display);
            //toOutputLine.Draw(graphics, colour, display);
        }

        public override void Update(float timeElapsed)
        {
            buffer.Update(timeElapsed);
            //fromInputLine.Update(timeElapsed);
            //node.Update(timeElapsed);
            //toOutputLine.Update(timeElapsed);
        }

        public override string Debug(string debug)
        {
            debug = AddDebug(debug);

            debug += " {";

            Connector nextConnector = buffer;
            debug = DebugNext(debug, nextConnector);

            //debug += "}";
            return debug;
        }

        public virtual int GetValue()
        {
            if (node.IsFull == false)
            {
                return 0;
            }

            int value = this.SlotValue;
            int slots = node.DataSlots;

            int bits = value * slots;
            return bits;
        }

        //public override void ConnectInputTo(Connector other)
        //{
        //    if (other == null)
        //    {
        //        return;
        //    }

        //    OutputConnection otherOutput = other.Output;
        //    Point contactPoint = otherOutput.Center;

        //    OutputConnection input = fromInputLine.StartPoint;
        //    input.AddConnection(contactPoint, other);
        //    otherOutput.AddConnection(input.Branches);
        //}

        //public override void DisconnectInput()
        //{
        //    OutputConnection input = fromInputLine.StartPoint;

        //    Connector other = input.Branches;
        //    if (other == null)
        //    {
        //        return;
        //    }

        //    other.Output.RemoveConnection();
        //    input.RemoveConnection();
        //}

        public override void ConnectTo(Connector other)
        {
            if (other == null)
            {
                return;
            }

            //OutputConnection otherInput = other.StartPoint;
            //Point contactPoint = Output.Center;

            OutputConnection output = toOutputLine.Output;
            output.AddConnection(other);
            //otherInput.AddConnection(contactPoint, toOutputLine);

            buffer.Sink = other;
        }

        public override void DisconnectOutput()
        {
            OutputConnection output = toOutputLine.Output;

            Connector other = output.GetNextConnector();
            if (other == null)
            {
                return;
            }

            //other.StartPoint.RemoveConnection();
            output.RemoveConnection(this);
        }

        public override Rectangle GetBounds()
        {
            Size size = GetSize();

            int width = size.Width;
            int height = size.Height;
            return base.GetBounds(width, height);
        }

        public override Size GetSize()
        {
            Size nodeSize = node.GetSize();

            int width = nodeSize.Width;
            int height = nodeSize.Height;

            height *= 2;

            Size size = new Size(width, height);
            return size;
        }

        public override void ReceiveData(Data data)
        {
            if (node.IsFull == true)
            {
                Output.SendData(data);
                return;
            }

            //if (padding.IsFull == true)
            //{
            //    SendData(data);
            //    return;
            //}

            //int slotsFilled = node.GetSlotsFilled();
            //int slotsRemaining = node.DataSlots - slotsFilled;
            //int largestDataValue = node.SlotValue;

            //int totalEntries = bufferData.Value;
            //if (totalEntries != largestDataValue)
            //{
            //data.Stop();
            buffer.ReceiveData(data);
            //    return;
            //}

            //fromInputLine.ReceiveData(bufferData);
        }

        //protected virtual void ReceiveDataFromBuffer()
        //{
        //    if (padding.Count == 0)
        //    {
        //        return;
        //    }

        //    int slotsFilled = node.GetSlotsFilled();
        //    int slotsRemaining = node.DataSlots - slotsFilled;
        //    int largestDataValue = node.SlotValue;

        //    if (totalBufferValue < largestDataValue)
        //    {
        //        return;
        //    }

        //    Content dataToReceive;
        //    switch (largestDataValue)
        //    {
        //        case 64:

        //            dataToReceive = new DataHex(this.StartPoint.Center, 2, 5);
        //            break;

        //        case 8:
        //            dataToReceive = new DataByte(this.StartPoint.Center, 2, 5);
        //            break;

        //        case 1:
        //        default:
        //            Content data = padding.Dequeue();
        //            DataBit[] allBitCells = BreakDataIntoBits(data);
        //            dataToReceive = allBitCells[0];
        //            for (int cellNumber = 1; cellNumber < allBitCells.Length; cellNumber++)
        //            {
        //                data = allBitCells[cellNumber];
        //                padding.Enqueue(data);
        //            }
        //            break;
        //    }

        //    dataToReceive.Cruise();
        //    fromInputLine.ReceiveData(dataToReceive);
        //}

        //protected virtual DataBit[] BreakDataIntoBits(Content data)
        //{
        //    int totalEntries = data.Value;

        //    LinkedList<DataBit> allBitCells = new LinkedList<DataBit>();
        //    if (totalEntries == 1)
        //    {
        //        allBitCells.AddLast((DataBit)data);
        //        return allBitCells.ToArray();
        //    }

        //    Point center = data.Center;
        //    float CRUISE_VELOCITY = data.CruiseVelocity;
        //    float MAX_VELOCITY = data.MAX_VELOCITY;
        //    float currentVelocity = data.Velocity;

        //    for (int cellNumber = 0; cellNumber < totalEntries; cellNumber++)
        //    {
        //        DataBit nextBitCell = new DataBit(center, CRUISE_VELOCITY, MAX_VELOCITY);
        //        nextBitCell.SetVelocity(currentVelocity);
        //        allBitCells.AddLast(nextBitCell);
        //    }

        //    return allBitCells.ToArray();
        //}

        //protected virtual DataByte MergeDataIntoByte(ref Content[] allData)
        //{
        //    LinkedList<int> allBitIndexes = new LinkedList<int>();

        //    int firstByteIndex = -1;
        //    for (int cellNumber = 0; cellNumber < allData.Length; cellNumber++)
        //    {
        //        Content data = allData[cellNumber];
        //        int totalEntries = data.Value;

        //        if(totalEntries > 8) {
        //            continue;
        //        }

        //        if (totalEntries == 8)
        //        {
        //            firstByteIndex = cellNumber;
        //            break;
        //        }

        //        allBitIndexes.AddLast(cellNumber);
        //        if (allBitIndexes.Count < 8)
        //        {
        //            continue;
        //        }

        //        break;
        //    }

        //    DataByte dataByte;
        //    if (firstByteIndex != -1)
        //    {
        //        dataByte = (DataByte)allData[firstByteIndex];
        //        allData[firstByteIndex] = null;
        //        return dataByte;
        //    }

        //    if (allBitIndexes.Count < 8)
        //    {
        //        return null;
        //    }

        //    int firstBitIndex = allBitIndexes.ElementAt(0);
        //    DataBit dataBit = (DataBit)allData[firstBitIndex];

        //    Point center = dataBit.Center;
        //    float CRUISE_VELOCITY = dataBit.CruiseVelocity;
        //    float MAX_VELOCITY = dataBit.MAX_VELOCITY;
        //    float currentVelocity = dataBit.Velocity;

        //    foreach (int bitIndex in allBitIndexes)
        //    {
        //        allData[bitIndex] = null;
        //    }

        //    dataByte = new DataByte(center, currentVelocity, MAX_VELOCITY);
        //    dataByte.SetVelocity(currentVelocity);

        //    return dataByte;
        //}

        //protected virtual DataByte MergeDataIntoHex(ref Content[] allData)
        //{
        //    LinkedList<int> allBitIndexes = new LinkedList<int>();
        //    LinkedList<int> allByteIndexes = new LinkedList<int>();

        //    int firstHexIndex = -1;
        //    int totalBytes;

        //    for (int cellNumber = 0; cellNumber < allData.Length; cellNumber++)
        //    {
        //        Content data = allData[cellNumber];
        //        int totalEntries = data.Value;

        //        if (totalEntries > 64)
        //        {
        //            continue;
        //        }

        //        if (totalEntries == 64)
        //        {
        //            firstHexIndex = cellNumber;
        //            break;
        //        }

        //        if (totalEntries == 8)
        //        {
        //            allByteIndexes.AddLast(cellNumber);
        //        }

        //        if (totalEntries == 1)
        //        {
        //            allBitIndexes.AddLast(cellNumber);
        //        }

        //        totalBytes = allByteIndexes.Count + allBitIndexes.Count / 8;
        //        if (totalBytes < 8)
        //        {
        //            continue;
        //        }

        //        break;
        //    }

        //    DataHex hex;
        //    if (firstHexIndex != -1)
        //    {
        //        hex = (DataHex)allData[firstHexIndex];
        //        allData[firstHexIndex] = null;
        //        return hex;
        //    }

        //    totalBytes = allByteIndexes.Count + allBitIndexes.Count / 8;
        //    if (totalBytes < 8)
        //    {
        //        return null;
        //    }

        //    Point center;
        //    float CRUISE_VELOCITY;
        //    float MAX_VELOCITY;
        //    float currentVelocity;

        //    if (allBitIndexes.Count > 0)
        //    {
        //        int firstByteIndex = allByteIndexes.ElementAt(0);
        //        DataByte dataByte = (DataByte)allData[firstByteIndex];

        //        center = dataByte.Center;
        //        CRUISE_VELOCITY = dataByte.CruiseVelocity;
        //        MAX_VELOCITY = dataByte.MAX_VELOCITY;
        //        currentVelocity = dataByte.Velocity;
        //    }
        //    else
        //    {
        //        int firstBitIndex = allBitIndexes.ElementAt(0);
        //        DataBit dataBit = (DataBit)allData[firstBitIndex];

        //        center = dataBit.Center;
        //        CRUISE_VELOCITY = dataBit.CruiseVelocity;
        //        MAX_VELOCITY = dataBit.MAX_VELOCITY;
        //        currentVelocity = dataBit.Velocity;
        //    }

        //    foreach (int byteIndex in allByteIndexes)
        //    {
        //        allData[byteIndex] = null;
        //    }

        //    int bytesNeeded = 8 - allBitIndexes.Count;
        //    if (bytesNeeded != 0)
        //    {
        //        int bitsNeeded = bytesNeeded * 8;
        //        for (int cellNumber = 0; cellNumber < bitsNeeded; cellNumber++)
        //        {
        //            int bitIndex = allBitIndexes.ElementAt(cellNumber);
        //            allData[bitIndex] = null;
        //        }
        //    }

        //    hex = new DataHex(center, currentVelocity, MAX_VELOCITY);
        //    hex.SetVelocity(currentVelocity);

        //    return hex;
        //}

        public virtual void DischargeStoredData()
        {
            node.DischargeStoredData();
        }

        public virtual bool IsFull()
        {
            return node.IsFull;
        }

        //public override Point GetNextWayPoint(Point currentWayPoint)
        //{
        //    int location;
        //    Point[] allLocations = new Point[] { fromInputLine.StartPoint.Center, fromInputLine.Output.Center, node.Output.Center, toOutputLine.Output.Center };
        //    for (location = 0; location < allLocations.Length; location++)
        //    {
        //        Point pointToCompare = allLocations[location];
        //        bool match = Program.ComparePoints(currentWayPoint, pointToCompare);
        //        if (match == false)
        //        {
        //            continue;
        //        }

        //        break;
        //    }

        //    Point nextWayPoint;

        //    if (location >= allLocations.Length)
        //    {
        //        if (toOutputLine.Output.Branches != null)
        //        {
        //            nextWayPoint = toOutputLine.Output.Branches.Output.Center;
        //            return nextWayPoint;
        //        }

        //        location = allLocations.Length - 1;
        //    }

        //    nextWayPoint = allLocations[location];
        //    return nextWayPoint;
        //}
    }

    public class BitNode : Component
    {
        public bool IsFull { get; protected set; }
        public int DataSlots { get; protected set; }
        //public int BitCapacity { get; protected set; }
        public static int nodeRadius { get; set; }
        protected static int indicatorRadius { get; set; }

        public BitNode(Point center, int dataSlots)
            : base(center)
        {
            this.IsFull = false;
            //this.BitCapacity = totalEntries;
            this.DataSlots = dataSlots;
            nodeRadius = 20;
            indicatorRadius = 10;

            //int connectorX = centrum.X + nodeRadius; //testing
            int connectorY = center.Y + nodeRadius;

            this.InputPoint = new Point(center.X, connectorY);
            Point outputPoint = new Point(center.X, connectorY);  //actual
            //Point outputPoint = new Point(connectorX, centrum.Y);  //testing

            this.Output = new OutputConnection(outputPoint);
        }

        public BitNode(int centerX, int centerY, int bitCapacity) :
            this(new Point(centerX, centerY), bitCapacity) { }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            Rectangle bodyBounds = GetBounds();
            Program.DrawCircle(graphics, colour, bodyBounds);

            int diameter = indicatorRadius * 2;
            Rectangle bitBounds = GetBounds(diameter, diameter);
            Program.DrawCircle(graphics, colour, bitBounds);
            //Program.DrawSquare(graphics, colour, Center, 45, 8);

            base.Draw(graphics, colour, display);

            if (IsFull == false)
            {
                return;
            }

            Program.FillCircle(graphics, colour, Center, indicatorRadius);
        }

        public override void Update(float timeElapsed)
        {
            SyncData();

            base.Update(timeElapsed);
        }

        public override Rectangle GetBounds()
        {
            return base.GetBounds(nodeRadius * 2, nodeRadius * 2);
        }

        public override Size GetSize()
        {
            return new Size(nodeRadius * 2, nodeRadius * 2);
        }

        protected override void TransferData(Data data)
        {
            base.TransferData(data);

            RemoveData(data);
        }

        public override void ReceiveData(Data data)
        {
            base.ReceiveData(data);

            if (IsFull == false)
            {
                SyncData();
                data.Orbit(Center, nodeRadius);
            }
            else
            {
                //data.Cruise();
            }

            AddData(data);
        }

        private void SyncData(double threshold = 2)
        {
            if (allData.Count < 2)
            {
                return;
            }

            int lastIndex = allData.Count - 1;
            int dataSynched = 0;

            for (int i = 0; i < lastIndex; i++)
            {
                int dataIndex = lastIndex - i;
                Data data = allData.ElementAt(dataIndex);

                int previousIndex = dataIndex - 1;
                Data previousData = allData.ElementAt(previousIndex);

                float clockwiseArcAngle = (float)Program.GetClockwiseArcAngle(data.CenterF, previousData.CenterF, this.Center);

                //float lastDataAngle = (float)Program.GetAbsAngle(this.Center, bufferData.CenterF);
                //float secondLastDataAngle = (float)Program.GetAbsAngle(this.Center, previousData.CenterF);

                //float clockwiseArcAngle = (secondLastDataAngle - lastDataAngle) % 360;
                //if (clockwiseArcAngle < 0)
                //{
                //    clockwiseArcAngle += 360;
                //}

                float desiredArcAngle = 360 / DataSlots;

                float angleDif = Math.Abs(clockwiseArcAngle - desiredArcAngle);
                if (angleDif < threshold)
                {
                    data.Cruise();
                    dataSynched++;
                    continue;
                }

                float currentVelocity = data.Velocity;
                float cruiseVelocity = data.CruiseVelocity;
                float halfCruise = cruiseVelocity / 2;

                if (clockwiseArcAngle > desiredArcAngle)
                {
                    if (currentVelocity > cruiseVelocity)
                    {
                        continue;
                    }

                    data.ChangeVelocity(halfCruise);
                }
                else
                {
                    if (currentVelocity < cruiseVelocity)
                    {
                        continue;
                    }

                    data.ChangeVelocity(-halfCruise);
                }
            }
        }

        public virtual void DischargeStoredData()
        {
            foreach (Data data in allData)
            {
                data.StopOrbiting();
            }
        }

        protected virtual void AddData(Data data)
        {
            UpdateBitFlag();
        }

        protected virtual void RemoveData(Data data)
        {
            UpdateBitFlag();
        }

        public virtual void UpdateBitFlag()
        {
            int slotsFilled = GetSlotsFilled();
            IsFull = slotsFilled >= DataSlots;
        }

        public virtual int GetSlotsFilled()
        {
            int slotsFilled = allData.Count - allDataToBeRemoved.Count;
            return slotsFilled;
        }

        //public override int GetPathType()
        //{
        //    return (int)PathType.BitNode;
        //}
    }
}