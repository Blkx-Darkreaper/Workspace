using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionMaker
{
    public class Condition : IEquatable<Condition>
    {
        public int Id { get; set; }
        protected static int nextId = 0;
        public string Description { get; set; }

        public Condition() : this(string.Empty) { }

        public Condition(string desc)
        {
            this.Id = nextId;
            nextId++;
            this.Description = desc;
        }

        public bool Equals(Condition other)
        {
            return Id == other.Id;
        }

        public virtual bool ConditionMet()
        {
            throw new NotImplementedException();
        }
    }
}
