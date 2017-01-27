using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class DataCapacitor : Component
    {
        public int StoredBits { get; protected set; }
        public int OutputValue { get; protected set; }
        public int MaxBits { get; protected set; }
        public bool IsFull { get; protected set; }
        public bool HasReachedCapacity { get; protected set; }
        public int SinkBits { get; protected set; }
        public Connector Sink { get; set; }
        protected float outputDelayRemaining { get; set; }
        public float OutputDelay { get; protected set; }
        protected float inputDelayRemaining { get; set; }
        public float InputDelay { get; protected set; }

        public DataCapacitor(Point center, int outputValue, int maxBits, Connector sink, float outputDelay = 16, float inputDelay = 24)
            : base(center)
        {
            this.StoredBits = 0;
            this.OutputValue = outputValue;
            this.MaxBits = maxBits;
            this.IsFull = false;
            this.HasReachedCapacity = false;
            this.SinkBits = 0;
            this.Sink = sink;
            this.outputDelayRemaining = 0;
            this.OutputDelay = outputDelay;
            this.inputDelayRemaining = 0;
            this.InputDelay = inputDelay;
        }

        public override Size GetSize()
        {
            return new Size(12, 12);
        }

        public override Rectangle GetBounds()
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;

            int cornerX = Center.X - width / 2;
            int cornerY = Center.Y - height / 2;

            return new Rectangle(cornerX, cornerY, width, height);
        }

        public override void Draw(Graphics graphics, Color colour, Size display)
        {
            base.Draw(graphics, colour, display);

            Size size = GetSize();
            Program.FillRectangle(graphics, Color.White, Center, size);

            DrawCapacity(graphics, ref colour, ref size);
            Program.DrawRectangle(graphics, colour, Center, size);
        }

        protected virtual void DrawCapacity(Graphics graphics, ref Color colour, ref Size size)
        {
            if (StoredBits == 0)
            {
                return;
            }

            Rectangle fill;
            if (IsFull == true)
            {
                fill = GetBounds();
                Program.FillRectangle(graphics, colour, fill);
                return;
            }

            int fillHeight = (int)(size.Height * StoredBits / (double)MaxBits);

            int fillX = Center.X - size.Width / 2;
            int fillY = Center.Y + size.Height / 2 - fillHeight;    // Invert Y
            fill = new Rectangle(fillX, fillY, size.Width, fillHeight);
            Program.FillRectangle(graphics, colour, fill);
        }

        public override void Update(float timeElapsed)
        {
            base.Update(timeElapsed);
        }

        protected override void UpdateData(float timeElapsed)
        {
            if (outputDelayRemaining > 0)
            {
                outputDelayRemaining -= timeElapsed;
            }
            if (inputDelayRemaining > 0)
            {
                inputDelayRemaining -= timeElapsed;
            }

            if (SinkBits == 0)
            {
                if (StoredBits == 0)
                {
                    if (allData.Count == 0)
                    {
                        return;
                    }

                    allData.Clear();
                    return;
                }
            }

            InsufficientData();

            if (outputDelayRemaining > 0)
            {
                return;
            }

            EmptyStorage();

            EmptySink();

            // Reset Output timer
            outputDelayRemaining = OutputDelay;
        }

        //protected override void UpdateData(float timeElapsed)
        //{
        //    if (outputDelayRemaining > 0)
        //    {
        //        outputDelayRemaining -= timeElapsed;
        //    }
        //    if (inputDelayRemaining > 0)
        //    {
        //        inputDelayRemaining -= timeElapsed;
        //    }

        //    InsufficientData();

        //    if (outputDelayRemaining > 0)
        //    {
        //        return;
        //    }

        //    EmptySink();

        //    if (SinkBits == 0)
        //    {
        //        if (StoredBits == 0)
        //        {
        //            allData.Clear();
        //            return;
        //        }
        //    }

        //    // Reset timer
        //    outputDelayRemaining = OutputDelay;

        //    // Buffer must be full
        //    //if (StoredBits < MaxBits)
        //    //{
        //    //    return;
        //    //}

        //    if (StoredBits < OutputValue)
        //    {
        //        return;
        //    }

        //    Content data = allData.ElementAt(0);
        //    float cruiseVelocity = data.CruiseVelocity;
        //    float maxVelocity = data.MaxVelocity;

        //    switch (OutputValue)
        //    {
        //        case 64:
        //            data = new DataHex(Center, cruiseVelocity, maxVelocity);
        //            break;

        //        case 8:
        //            data = new DataByte(Center, cruiseVelocity, maxVelocity);
        //            break;

        //        case 1:
        //        default:
        //            data = new DataBit(Center, cruiseVelocity, maxVelocity);
        //            break;
        //    }

        //    StoredBits -= OutputValue;

        //    TransferData(data);
        //}

        public override void ReceiveData(Data data)
        {
            data.Stop();

            int bitsToAdd = data.Value;

            if (HasReachedCapacity == true)
            {
                SinkBits += bitsToAdd;
                return;
            }

            inputDelayRemaining = InputDelay;

            int totalBits = StoredBits + bitsToAdd;
            if (totalBits > MaxBits)
            {
                int excessBits = totalBits - MaxBits;
                SinkBits += excessBits;

                bitsToAdd = bitsToAdd - excessBits;
                if (bitsToAdd == 0)
                {
                    return;
                }
            }

            StoredBits += bitsToAdd;

            if (StoredBits == MaxBits)
            {
                HasReachedCapacity = true;
            }

            IsFull = StoredBits == MaxBits;

            allData.Clear();
            allData.AddLast(data);
        }

        protected override void TransferData(Data data)
        {
            Output.SendData(data);
        }

        protected virtual void InsufficientData()
        {
            if (HasReachedCapacity == true)
            {
                return;
            }

            if (StoredBits == 0)
            {
                return;
            }

            if (inputDelayRemaining > 0)
            {
                return;
            }

            SinkBits += StoredBits;
            StoredBits = 0;
        }

        protected virtual void EmptyStorage()
        {
            // Must be at capacity
            if (HasReachedCapacity == false)
            {
                return;
            }

            if (outputDelayRemaining > 0)
            {
                return;
            }

            if (StoredBits < OutputValue)
            {
                return;
            }

            Data data = allData.ElementAt(0);
            float cruiseVelocity = data.CruiseVelocity;
            float maxVelocity = data.MaxVelocity;

            switch (OutputValue)
            {
                case 64:
                    data = new DataHex(Center, cruiseVelocity, maxVelocity);
                    break;

                case 8:
                    data = new DataByte(Center, cruiseVelocity, maxVelocity);
                    break;

                case 1:
                default:
                    data = new DataBit(Center, cruiseVelocity, maxVelocity);
                    break;
            }

            StoredBits -= OutputValue;

            TransferData(data);
        }

        protected virtual void EmptySink()
        {
            if (SinkBits == 0)
            {
                return;
            }

            if (outputDelayRemaining > 0)
            {
                return;
            }

            int dataValue = GetLargestDataValue(SinkBits);

            Data data = allData.ElementAt(0);
            float cruiseVelocity = data.CruiseVelocity;
            float maxVelocity = data.MaxVelocity;

            switch (dataValue)
            {
                case 64:
                    data = new DataHex(Center, cruiseVelocity, maxVelocity);
                    break;

                case 8:
                    data = new DataByte(Center, cruiseVelocity, maxVelocity);
                    break;

                case 1:
                default:
                    data = new DataBit(Center, cruiseVelocity, maxVelocity);
                    break;
            }

            SinkBits -= dataValue;

            if (Sink == null)
            {
                return;
            }

            //data.Cruise();
            Sink.ReceiveData(data);
        }
    }
}