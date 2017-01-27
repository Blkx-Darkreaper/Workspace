using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bits
{
    public class Entity
    {
        public int Id { get; protected set; }
        protected static int nextId = 1;
        public bool isHidden { get; protected set; }

        public Entity() {
            this.Id = nextId;
            nextId++;
        }

        public virtual void Draw(Graphics graphics, Color colour, Size display) {
            if (isHidden == true)
            {
                return;
            }
        }

        public virtual void Update(float timeElapsed) { }

        public virtual void SetIsHidden(bool hidden)
        {
            isHidden = hidden;
        }
    }
}
