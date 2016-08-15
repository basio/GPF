using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GPF
{
    class Vertex<T> : IGraphNode
    {
        public int ID;
        static int id__ = 0;
        public int value;
        public int superstep;
        public bool isActive { set; get; }

        public IGraphNode parent { set; get; }

        public List<Vertex<T>> outgoing_edges=new List<Vertex<T>>();

        public List<Vertex<T>> incoming_edges =new List<Vertex<T>>();

        public Vertex()
        {
            ID = id__++;
            isActive = true;
        }
        public Vertex(int value)
        {
            ID = id__++;
            this.value = value;
            isActive = true;
        }

        public void AddEdge(Vertex<T> v){
            outgoing_edges.Add(v);
            v.incoming_edges.Add(this);
        }
        Vertex<T> getVertex(int id)
        {
            //get vertex
            Graph<T> g = (Graph<T>)parent.parent.parent;
            int w_id = g.vertex_to_worker[id];
            Worker<T> w = g.workers[w_id];
            int p_id = w.vertex_to_parition[id];
            Parition<T> part = w.parts[p_id];

            return part.vertices[id];
        }
        List<int> Messages = new List<int>();
        public void SendMessage(int dst, int message)
        {
            Vertex<T> v = getVertex(dst);
            lock(v){
            v.Messages.Add(message);
            }
            if (!v.isActive)
            {
                v.isActive = true;
                
            }

        }
        public int Ready()  {
            int c = 0;
            int m = 0;
            lock (Messages)
            {
                m = Messages.Count() ;
            }
            if (m > incoming_edges.Count)
                return m - incoming_edges.Count;

            foreach (Vertex<T> v in incoming_edges)
            {
                if (v.isActive) c++;
            }            
            return m-c;
        }
        
        public void compute()
        {
            List<int> messages_current = new List<int>();
            lock (Messages)
            {
                for (int i = 0; i < Messages.Count; i++)
                {
                    messages_current.Add(Messages[i]);
                }
                Messages.Clear();
            }

            int val = 0;
            for (int i = 0; i < messages_current.Count; i++)
            {
                val = Math.Max(messages_current[i], val);
            }
            Console.WriteLine(ID + "    " + val);
            if (val > value)
            {
                value = val;
                for (int j = 0; j < outgoing_edges.Count; j++)
                {
                    int dst = outgoing_edges[j].ID;
                    SendMessage(dst, value);
                }

            }
            else
            {
                isActive = false;
            }

            if (superstep > 10) isActive = false;
        }

    }
}
