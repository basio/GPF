using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace GPF
{
    class Graph<T>:IGraphNode
    {
        //graph consists of partitions, which contains of vertex, each vertex store its edges
        public Dictionary<int, Worker<T>> workers=new Dictionary<int,Worker<T>>();

      public  Dictionary<int, int> vertex_to_worker = new Dictionary<int, int>();
        
        int n_worker;
        int n_vertex;
        int n_edges;

        public void createRandomGraph(int n, int m, int nworkers=10)
        {
            n_edges=m;
            n_vertex=n;
            n_worker=nworkers;
            List<Vertex<T>> vertices = new List<Vertex<T>>();
            for (int i = 0; i < n; i++) {
                vertices.Add(new Vertex<T>());                
            }
            Random r = new Random();

            for (int i = 0; i < m; i++) {
                int s = r.Next(n);
                int e = r.Next(n);
                vertices[s].AddEdge(vertices[e]);
            }
            //
            for (int i = 0; i < n_worker; i++)
            {
                Worker<T> w=new Worker<T>(i,1);
                w.parent = this;
                workers.Add(i,w );
            }
            //assign worker
            for (int i = 0; i < n; i++)  {
                int p = i % n_worker;
                workers[p].AddtoPart(vertices[i]);
                vertex_to_worker.Add(vertices[i].ID, p);
                
            }
        }
        public IGraphNode parent { set; get; }
        public void run()
        {
            Thread [] ts = new Thread[n_worker];
            for (int i = 0; i < n_worker; i++)
            {
                ts[i] = new Thread(workers[i].run);
            }
            for (int i = 0; i < n_worker; i++)
            {
                ts[i].Start();
            }
        }
    }
}
