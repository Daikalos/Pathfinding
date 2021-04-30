using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinding
{
    class Vertex
    {
        public List<Vertex> Neighbours { get; private set; }
        public List<Edge> Edges { get; private set; }

        public Vertex Parent { get; set; }
        public Point Position { get; private set; }

        public int X => Position.X;
        public int Y => Position.Y;

        public bool IsVisited { get; set; }

        public float F => G + H;     // Total Cost
        public float G { get; set; } // Distance from current to start
        public float H { get; set; } // Distance from current to end

        public int EdgeCount => Edges.Count;

        public Vertex(int x, int y)
        {
            Position = new Point(x, y);

            Neighbours = new List<Vertex>();
            Edges = new List<Edge>();

            IsVisited = false;

            G = float.PositiveInfinity;
            H = float.PositiveInfinity;
        }
        public Vertex(Point point) : this(point.X, point.Y)
        {
        }

        public void ClearNeighbours()
        {
            Neighbours.Clear();
        }
        public void AddNeighbour(Vertex vertex)
        {
            Neighbours.Add(vertex);
        }
        public void RemoveNeighbour(Vertex vertex)
        {
            Neighbours.Remove(vertex);
        }

        public void ClearEdges()
        {
            Edges.Clear();
        }
        public void AddEdge(Edge edge)
        {
            if (edge == null)
                return;

            Edges.Add(edge);
        }
        public void RemoveEdge(Edge edge)
        {
            if (edge == null)
                return;

            Edges.Remove(edge);
        }
    }
}
