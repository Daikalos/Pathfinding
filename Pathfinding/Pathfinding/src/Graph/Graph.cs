using System;
using Microsoft.Xna.Framework;

public class Graph
{
    private readonly Vertex[] Vertices;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public int VertW { get; private set; }
    public int VertH { get; private set; }

    public int GapW { get; private set; }
    public int GapH { get; private set; }

    public Graph(int width, int height, int vertW, int vertH, int gapW, int gapH)
    {
        Width = width;
        Height = height;
        VertW = vertW;
        VertH = vertH;
        GapW = gapW;
        GapH = gapH;

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
        int x = (pos.X / (VertW + GapW));
        int y = (pos.Y / (VertH + GapH));

        if (!WithinBoard(x, y))
            return null;

        return Vertices[x + y * Width];
    }

    public bool WithinBoard(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= Width || y >= Height);
    }
    public bool WithinBoard(Point pos)
    {
        return !(pos.X < 0 || pos.Y < 0 || pos.X >= Width || pos.Y >= Height);
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
                for (int i = -1; i <= 1; ++i) // Left and Right
                {
                    for (int j = -1; j <= 1; ++j) //Top and Bottom
                    {
                        if (j == 0 && i == 0)
                            continue;

                        if (WithinBoard(x + i, y + j))
                        {
                            Vertex vertex = AtPos(x, y);
                            Vertex neighbour = AtPos(x + i, y + j);

                            new Edge(vertex, neighbour);
                        }
                    }
                }
            }
        }
    }
}
