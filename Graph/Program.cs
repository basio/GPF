using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPF
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<float> g = new Graph<float>();
            g.createRandomGraph(1000 * 1000 *10, 10*1000 * 1000 );
            g.run();
        }
    }
}
