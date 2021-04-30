using System.Linq;
using System.Collections.Generic;
using Utilities;

namespace Pathfinding
{
    /// <summary>
    /// randomized depth first search
    /// </summary>
    class RDFS : IMaze
    {
        public void Generate(Graph graph, Grid grid, Vertex start)
        {
            List<Vertex> open = new List<Vertex>();

            for (int y = 0; y < graph.Height; ++y) // Add all vertices
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    graph.AtPos(x, y).ClearEdges();
                }
            }

            grid.UpdateColor();

            Vertex current = start;
            current.IsVisited = true;

            grid.AtPos(current).RemoveWall();
            open.AddRange(Neighbours(graph, grid, current));

            while (open.Count > 0)
            {
                int r = StaticRandom.RandomNumber(0, open.Count);
                current = open[r];

                List<Vertex> neighbours = Neighbours(graph, grid, current);

                if (neighbours.Where(n => n.IsVisited).Count() < 2)
                {
                    grid.AtPos(current).RemoveWall();
                    open.AddRange(Neighbours(graph, grid, current).Where(n => !n.IsVisited));

                    current.IsVisited = true;
                }

                open.Remove(current);
            }
        }

        private List<Vertex> Neighbours(Graph graph, Grid grid, Vertex vertex)
        {
            List<Vertex> neighbours = new List<Vertex>();

            for (int y = -1; y <= 1; ++y) // Add all vertices
            {
                for (int x = -1; x <= 1; ++x)
                {
                    if (grid.FourDirectional && (x & y) != 0)
                        continue;

                    if (!graph.WithinBoard(vertex.X + x, vertex.Y + y))
                        continue;

                    Vertex neighbour = graph.AtPos(vertex.X + x, vertex.Y + y);

                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }
    }
}
