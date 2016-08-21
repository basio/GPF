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

        public List<Vertex<T>> outgoing_edges = new List<Vertex<T>>();

        public List<Vertex<T>> incoming_edges = new List<Vertex<T>>();

        public Vertex(int value)
        {
            ID = id__++;
            this.value = ID;
            isActive = true;
        }

        public void AddEdge(Vertex<T> v)
        {
            if (!outgoing_edges.Contains(v))
            {
                outgoing_edges.Add(v);
                v.incoming_edges.Add(this);
            }
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
        List<Message> Messages = new List<Message>();
        public void SendMessage(Message message)
        {
            Vertex<T> v = getVertex(message.dst);
            lock (v)
            {
                v.Messages.Add(message);
            }
            if (!v.isActive)
            {
                v.isActive = true;

            }
            Worker<T> w = (Worker<T>)v.parent.parent;
            if (w.workQueue.Contains(message.dst))
                w.workQueue.IncreasePriority(message.dst, -1);
            else
                w.workQueue.Enqueue(message.dst, -1);
        }
        public int Ready()
        {
            int c = 0;
            int m = 0;
            lock (Messages)
            {
                m = Messages.Count();
            }
            if (m >= incoming_edges.Count)
                return m - incoming_edges.Count;

            //foreach (Vertex<T> v in incoming_edges)
            //{
            //    if (v.isActive) c++;
            //}
            //return m - c;

            //here if the superstep beyond all incoming 
            int incomingactive = 0;
            int total = 0;
            foreach (Vertex<T> v in incoming_edges)
            {
                if (!v.isActive) continue;
                if (v.superstep > superstep) incomingactive++;
                total++;
            }
            return incomingactive - total;

        }

        public void compute()
        {

            List<Message> messages_current = new List<Message>();
            lock (Messages)
            {
                for (int i = 0; i < Messages.Count; i++)
                {
                    messages_current.Add(Messages[i]);
                }
                Messages.Clear();
            }
            if (superstep == 0)
            {
                for (int j = 0; j < outgoing_edges.Count; j++)
                {
                    int dst = outgoing_edges[j].ID;
                    Message m = new Message(ID, dst, value, superstep);
                    SendMessage(m);
                }
            }
            else
            {
                int val = 0;
                for (int i = 0; i < messages_current.Count; i++)
                {
                    val = Math.Max(messages_current[i].value, val);
                }

                if (val > value)
                {
                    value = val;

                    for (int j = 0; j < outgoing_edges.Count; j++)
                    {
                        int dst = outgoing_edges[j].ID;
                        Message m = new Message(ID, dst, value, superstep);
                        SendMessage(m);
                    }
                }
                else
                {
                    isActive = false;
                }
            }
            if (superstep > 10) isActive = false;
        }
        public override string ToString()
        {

            string r = ID + " " + value + " " + isActive + "  " + superstep + "\n";
            foreach (Message m in Messages)
            {
                r = r + m.ToString();
            }
            return r;
        }

    }
}
