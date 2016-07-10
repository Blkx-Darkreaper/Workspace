using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionMaker
{
    public class Activity : IEquatable<Activity>, IComparable<Activity>
    {
        public int Id { get; protected set; }
        protected static int nextId = 0;
        public string Description { get; set; }
        protected List<Action> actions { get; set; }

        public Activity(string desc, Action[] actions)
            : this(actions)
        {
            this.Description = desc;
        }

        public Activity(Action[] actions)
        {
            this.Id = nextId;
            nextId++;
            this.Description = string.Empty;

            if (actions != null)
            {
                this.actions = new List<Action>(actions);
            }
            else
            {
                this.actions = new List<Action>();
            }
        }

        public bool Equals(Activity other)
        {
            return Id == other.Id;
        }
        
        public int CompareTo(Activity other)
        {
            return Id.CompareTo(other.Id);
        }

        public override string ToString()
        {
            if (actions.Count == 0)
            {
                return Description;
            }

            string output = string.Format("{0}: ", Description);
            for(int i = 0; i < actions.Count; i++)
            {
                Action action = actions[i];
                if (i == 0)
                {
                    output += action.Description;
                    continue;
                }

                output += " , " + action.Description;
            }

            return output;
        }
    }
}
