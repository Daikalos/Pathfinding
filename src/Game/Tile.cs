using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinding
{
    class Tile : GameObject
    {
        private readonly Grid grid;
        private readonly Graph graph;
        private readonly Vertex vertex;

        private Color color;
        private bool isWall;

        public Vertex Vertex => vertex;
        public Color Color { get => color; set => color = value; }
        public bool IsWall => isWall;

        public int X => vertex.X;
        public int Y => vertex.Y;

        public Tile(Vector2 position, Point size, Grid grid, Graph graph, Vertex vertex) : base(position, size)
        {
            this.grid = grid;
            this.graph = graph;
            this.vertex = vertex;

            Update();
        }

        public override void Update()
        {
            isWall = !vertex.Neighbours.All(
                n => n.Edges.Any(e1 => e1.To == vertex));
            color = isWall ? Color.DimGray : Color.LightGray;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DestRect, null, color);
        }

        public void AddWall()
        {
            if (isWall)
                return;

            vertex.Neighbours.ForEach(n =>
            {
                List<Edge> edgeToRemove = n.Edges.Where(e => e.To == vertex).ToList();

                if (edgeToRemove.Count > 0)
                    edgeToRemove.ForEach(e => n.RemoveEdge(e));
            });

            Update();
        }
        public void RemoveWall()
        {
            if (!isWall)
                return;

            foreach (Vertex n0 in vertex.Neighbours)
            {
                if (n0.Edges.All(e => e.To != vertex))
                    new Edge(n0, vertex);
            }

            Update();
        }

        public void MakeEightDirectional()
        {
            vertex.ClearEdges();

            for (int y = -1; y <= 1; ++y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    if (x == 0 && y == 0)
                        continue;

                    if (!graph.WithinBoard(vertex.X + x, vertex.Y + y))
                        continue;

                    new Edge(vertex, graph.AtPos(vertex.X + x, vertex.Y + y), (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
                }
            }
        }
        public void MakeFourDirectional()
        {
            vertex.ClearEdges();

            for (int y = -1; y <= 1; y += 2)
            {
                if (!graph.WithinBoard(vertex.X, vertex.Y + y))
                    continue;

                new Edge(vertex, graph.AtPos(vertex.X, vertex.Y + y));
            }
            for (int x = -1; x <= 1; x += 2)
            {
                if (!graph.WithinBoard(vertex.X + x, vertex.Y))
                    continue;

                new Edge(vertex, graph.AtPos(vertex.X + x, vertex.Y));
            }
        }

        public override void LoadContent()
        {
            AssignTexture("Node");
        }
    }
}
