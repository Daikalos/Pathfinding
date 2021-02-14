﻿using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinding
{
    class Dijkstra : IPathfinder
    {
        public List<Vertex> PathTo(Grid grid, Graph graph, Vertex start, Vertex goal)
        {
            PriorityQueue<Vertex> open = new MinHeap<Vertex>();

            graph.InitializeVertices();

            Vertex current = start;
            open.Enqueue(current, current.F);

            current.G = 0;

            while (open.Count > 0)
            {
                current = open.Dequeue();

                if (!current.IsVisited)
                {
                    current.IsVisited = true;
                    
                    // Visualization
                    {
                        Tile tile = grid.AtPos(current.Position);

                        if (!tile.IsWall)
                            tile.Color = Color.AliceBlue;
                    }

                    if (current.Equals(goal))
                        return FindPath(start, goal);

                    foreach (Edge edge in current.Edges)
                    {
                        Vertex neighbour = edge.To;

                        float gScore = current.G + edge.Weight;
                        if (gScore < neighbour.G)
                        {
                            neighbour.G = gScore;
                            neighbour.H = 0.0f;

                            neighbour.Parent = current;

                            if (!open.Contains(neighbour))
                                open.Enqueue(neighbour, neighbour.F);
                        }
                    }
                }
            }

            return new List<Vertex>(); // Return empty path if none is found
        }

        private List<Vertex> FindPath(Vertex start, Vertex end) // Reconstruct path
        {
            List<Vertex> path = new List<Vertex>();
            Vertex current = end;

            while (current != start)
            {
                path.Add(current);
                current = current.Parent;
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}
