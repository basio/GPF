using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace GPF
{
    class Program
    {
        static void Main(string[] args)
        {
            
         
            Graph<float> g = new Graph<float>();
            g.createRandomGraph(10, 100,1 );
            g.run();
        }
    }
}
