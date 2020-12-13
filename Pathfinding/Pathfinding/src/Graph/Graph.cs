using System;
using Microsoft.Xna.Framework;

public class Graph
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

    public Vertex AtPos(int x, int y)
    {
        return Vertices[x + y * Width];
    }
    public Vertex AtPos(Point pos)
    {
        return Vertices[pos.X + pos.Y * Width];
    }
    public Vertex AtMousePos(Point pos)
    {
        return Vertices[(pos.X / Width) + ((pos.Y / Height) * Width)];
    }


    public bool WithinBoard(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= Width || y >= Height);
    }
    public bool WithinBoard(Point pos)
    {
        return !(pos.X < 0 || pos.Y < 0 || pos.X >= Width || pos.Y >= Height);
    }

    public static float Distance(Vertex from, Vertex to)
    {
        return Math.Abs(from.Position.X - to.Position.X) +
               Math.Abs(from.Position.Y - to.Position.Y); // Manhattan Distance
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

    public void GenerateGraph()
    {
        AddVertices();
        AddEdges();
    }
    private void AddVertices()
    {
        for (int y = 0; y < Height; y++) // Add all vertices
        {
            for (int x = 0; x < Width; x++)
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
                for (int i = -1; i <= 1; i += 2) // Left and Right
                {
                    if (WithinBoard((x + i), y))
                    {
                        Vertex vertex = AtPos(x, y);
                        Vertex neighbour = AtPos(x + i, y);

                        new Edge(vertex, neighbour);
                    }
                }
                for (int j = -1; j <= 1; j += 2) //Top and Bottom
                {
                    if (WithinBoard(x, (y + j)))
                    {
                        Vertex vertex = AtPos(x, y);
                        Vertex neighbour = AtPos(x, y + j);

                        new Edge(vertex, neighbour);
                    }
                }
            }
        }
    }
}
