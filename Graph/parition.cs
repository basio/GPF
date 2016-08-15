using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPF
{
    class Parition <T>:IGraphNode
    {
        public int ID;
        public Dictionary<int,Vertex<T>> vertices=new Dictionary<int,Vertex<T>>();
        public int count { get { return vertices.Count; } }
        public Parition(int i)
        {
            ID = i;
            vertices = new Dictionary<int, Vertex<T>>();
        }
        public void AddVertex(Vertex<T> v)
        {
            v.parent = this;
            vertices.Add(v.ID,v);
        }

        public IGraphNode parent { set; get; }
       

    }
}
