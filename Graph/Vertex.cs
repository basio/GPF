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
        public T value;
        public int superstep;
        public bool isActive { set; get; }

        public IGraphNode parent { set; get; }

        public List<Vertex<T>> outgoing_edges=new List<Vertex<T>>();

        public List<Vertex<T>> incoming_edges =new List<Vertex<T>>();

        public Vertex()
        {
            ID = id__++;
        }
        public Vertex(T value)
        {
            ID = id__++;
            this.value = value;
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


        }

    }
}
