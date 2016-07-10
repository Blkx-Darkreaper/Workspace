using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionMaker
{
    public class Action
    {
        public int Id { get; protected set; }
        protected static int nextId = 0;
        public string Description { get; set; }
    }
}
