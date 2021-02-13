using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Utilities;

namespace Pathfinding
{
    /// <summary>
    /// randomized depth first search
    /// </summary>
    class RDFS : IMazeGenerator
    {
        public void Generate(Graph graph, Grid grid, Vertex start)
        {
            Stack<Vertex> open = new Stack<Vertex>();

            graph.InitializeVertices();

            Vertex current = start;
            current.IsVisited = true;

            open.Push(current);

            while (open.Count > 0)
            {
                current = open.Pop();

                // Visualization
                {
                    Thread.Sleep(1);

                    Tile tile = grid.AtPos(current.Position);

                    tile.Color = Color.AliceBlue;
                }

                List<Tuple<Vertex, Edge>> unvisitedNeighbours = new List<Tuple<Vertex, Edge>>();

                foreach (Edge edge in current.Edges)
                {
                    Vertex neighbour = edge.To;

                    if (neighbour.IsVisited)
                        continue;

                    unvisitedNeighbours.Add(new Tuple<Vertex, Edge>(neighbour, edge));
                }

                if (unvisitedNeighbours.Count > 0)
                {
                    open.Push(current);

                    Tuple<Vertex, Edge> data = unvisitedNeighbours[StaticRandom.RandomNumber(0, unvisitedNeighbours.Count)];

                    Vertex randVertex = data.Item1;
                    current.RemoveEdge(data.Item2);

                    randVertex.IsVisited = true;
                    open.Push(randVertex);
                }
            }
        }
    }
}
