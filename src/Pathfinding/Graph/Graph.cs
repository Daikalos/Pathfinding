﻿using System;
using Microsoft.Xna.Framework;

namespace Pathfinding
{
    /// <summary>
    /// Weighted Graph
    /// </summary>
    class Graph
    {
        private readonly Vertex[] Vertices;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Graph(int width, int height)
        {
            Width = width;
            Height = height;

            Vertices = new Vertex[Width * Height];
        }

        public void Generate()
        {
            AddVertices();
            AddEdges();
        }
        private void AddVertices()
        {
            for (int y = 0; y < Height; ++y) // Add all vertices
            {
                for (int x = 0; x < Width; ++x)
                {
                    Vertices[x + y * Width] = new Vertex(x, y);
                }
            }
        }
        private void AddEdges()
        {
            for (int y = 0; y < Height; ++y) // Add all edges
            {
                for (int x = 0; x < Width; ++x)
                {
                    for (int i = -1; i <= 1; ++i)
                    {
                        for (int j = -1; j <= 1; ++j)
                        {
                            if (j == 0 && i == 0)
                                continue;

                            if (WithinBoard(x + j, y + i))
                            {
                                Vertex vertex = AtPos(x, y);
                                Vertex neighbour = AtPos(x + j, y + i);

                                vertex.AddNeighbour(neighbour);

                                new Edge(vertex, neighbour, (float)Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2)));
                            }
                        }
                    }
                }
            }
        }

        public void InitializeVertices()
        {
            foreach (Vertex vertex in Vertices)
            {
                vertex.IsVisited = false;
                vertex.G = float.PositiveInfinity;
                vertex.H = float.PositiveInfinity;
            }
        }

        public Vertex AtPos(int x, int y)
        {
            return Vertices[x + y * Width];
        }
        public Vertex AtPos(Point pos)
        {
            return AtPos(pos.X, pos.Y);
        }

        public bool WithinBoard(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= Width || y >= Height);
        }
        public bool WithinBoard(Point pos)
        {
            return WithinBoard(pos.X, pos.Y);
        }
        public bool WithinBoard(Vertex vertex)
        {
            return WithinBoard(vertex.Position);
        }

        public static float ManhattanDistance(Vertex from, Vertex to)
        {
            return Math.Abs(to.Position.X - from.Position.X) +
                   Math.Abs(to.Position.Y - from.Position.Y);
        }
        public static float DiagonalDistance(Vertex from, Vertex to)
        {
            return (float)Math.Sqrt(Math.Pow(to.Position.X - from.Position.X, 2) +
                                     Math.Pow(to.Position.Y - from.Position.Y, 2));
        }
    }
}
