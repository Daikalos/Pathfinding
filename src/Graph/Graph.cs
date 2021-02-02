using System;
using Microsoft.Xna.Framework;

namespace Graph
{
    /// <summary>
    /// Weighted Graph
    /// </summary>
    class WGraph
    {
        private readonly Vertex[] Vertices;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public WGraph(int width, int height)
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
                    Vertices[x + y * Width] = new Vertex(new Point(x, y));
                }
            }
        }
        private void AddEdges()
        {
            for (int y = 0; y < Height; ++y) // Add all edges
            {
                for (int x = 0; x < Width; ++x)
                {
                    for (int i = -1; i <= 1; ++i) // Left and Right
                    {
                        for (int j = -1; j <= 1; ++j) //Top and Bottom
                        {
                            if (j == 0 && i == 0)
                                continue;

                            if (WithinBoard(x + j, y + i))
                            {
                                Vertex vertex = AtPos(x, y);
                                Vertex neighbour = AtPos(x + j, y + i);

                                new Edge(vertex, neighbour);
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
            return Vertices[pos.X + pos.Y * Width];
        }

        public bool WithinBoard(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= Width || y >= Height);
        }
        public bool WithinBoard(Point pos)
        {
            return WithinBoard(pos.X, pos.Y);
        }

        public static float ManhattanDistance(Vertex from, Vertex to)
        {
            return Math.Abs(from.Position.X - to.Position.X) +
                   Math.Abs(from.Position.Y - to.Position.Y);
        }
        public static float DiagonalDistance(Vertex from, Vertex to)
        {
            return (float)Math.Sqrt(Math.Pow(from.Position.X - to.Position.X, 2) +
                                    Math.Pow(from.Position.Y - to.Position.Y, 2));
        }
    }
}
