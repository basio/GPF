using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPF
{
    class Message
    {
        public int src;
        public int dst;
        public int value;
        public int superstep;

        public Message(int src, int dst, int value, int superstep)
        {
            this.src = src;
            this.dst = dst;
            this.value = value;
            this.superstep = superstep;
        }

        public override string ToString()
        {
            return "s:"+src + " d:" + dst + " v:" + value + " s:" + superstep+"\n";
        }
    }
}
