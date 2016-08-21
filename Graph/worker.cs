using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Priority_Queue;

namespace GPF
{
    class WorkerFactory<T>
    {
        static public Worker<T> createWorker(int a, int b,int type=1)
        {
            if (type == 1)
                return new Worker1<T>(a,b);
            else
                return new Worker2<T>(a,b);
        }
    }

    abstract class Worker<T> : IGraphNode
    {
        public int ID;
        public Dictionary<int, Parition<T>> parts = new Dictionary<int, Parition<T>>();
        public Dictionary<int, int> vertex_to_parition = new Dictionary<int, int>();

        public SimplePriorityQueue<int> workQueue = new SimplePriorityQueue<int>();

      protected  int n_parition;
        static int indx;

        public void AddtoPart(Vertex<T> v)
        {
            indx = (indx++) % n_parition;
            parts[indx].AddVertex(v);
            vertex_to_parition.Add(v.ID, indx);
        }
        public Worker(int id, int n_part)
        {
            this.ID = id;
            this.n_parition = n_part;
            for (int i = 0; i < n_part; i++)
            {
                Parition<T> p = new Parition<T>(i);
                p.parent = this;
                parts.Add(i, p);

            }

        }
        public IGraphNode parent { set; get; }


        public void Print()
        {
        //    foreach (var v in workQueue)
        //    {
        //        Console.Write(v + " ");
        //    }
        //    Console.WriteLine();
            Console.WriteLine(workQueue.Count);
        }
        abstract public void run();
    }

    class Worker1<T> : Worker<T>
    {
        public Worker1(int id, int n_part):
            base(id,n_part){
        }
        public override void run()
        {
            for (int i = 0; i < n_parition; i++)
            {
                Parition<T> p = parts[i];
                foreach (KeyValuePair<int, Vertex<T>> tempv in p.vertices)
                {
                    workQueue.Enqueue(tempv.Value.ID, -tempv.Value.outgoing_edges.Count);
                }
            }
            while (workQueue.Count > 0)
            {
                //    Print();
                int vid = workQueue.Dequeue();
                //get parition
                int pid = vertex_to_parition[vid];
                Parition<T> p = parts[pid];
                Vertex<T> v = p.vertices[vid];
                int r = 1;
                // if (v.superstep > 0)
                //      r = v.Ready();

                if (r >= 0 || v.superstep == 0)
                {
                    //     Console.WriteLine(v);
                    v.compute();
                    v.superstep++;
                    if (v.isActive)
                    {
                        workQueue.Enqueue(v.ID, v.incoming_edges.Count);
                    }
                }
                // else
                //{
                //  workQueue.Enqueue(v.ID, -r);
                //}


            }
            for (int i = 0; i < n_parition; i++)
            {
                Parition<T> p = parts[i];
                foreach (KeyValuePair<int, Vertex<T>> tempv in p.vertices)
                {
                    Console.WriteLine(tempv.Value);
                }
            }
        }
    }
    
    
    class Worker2<T> : Worker<T> //wait for ready
    {
         public Worker2(int id, int n_part):
            base(id,n_part){
        }
         public override void run()
         {
             for (int i = 0; i < n_parition; i++)
             {
                 Parition<T> p = parts[i];
                 foreach (KeyValuePair<int, Vertex<T>> tempv in p.vertices)
                 {
                     workQueue.Enqueue(tempv.Value.ID, -tempv.Value.outgoing_edges.Count);
                 }
             }
             while (workQueue.Count > 0)
             {
                 //    Print();
                 int vid = workQueue.Dequeue();
                 //get parition
                 int pid = vertex_to_parition[vid];
                 Parition<T> p = parts[pid];
                 Vertex<T> v = p.vertices[vid];
                 int r = 1;
                 // if (v.superstep > 0)
                 //      r = v.Ready();

                 if (r >= 0 || v.superstep == 0)
                 {
                     //     Console.WriteLine(v);
                     v.compute();
                     v.superstep++;
                     if (v.isActive)
                     {
                         workQueue.Enqueue(v.ID, v.incoming_edges.Count);
                     }
                 }
                 // else
                 //{
                 //  workQueue.Enqueue(v.ID, -r);
                 //}


             }
             for (int i = 0; i < n_parition; i++)
             {
                 Parition<T> p = parts[i];
                 foreach (KeyValuePair<int, Vertex<T>> tempv in p.vertices)
                 {
                     Console.WriteLine(tempv.Value);
                 }
             }

         }
    }
    }

