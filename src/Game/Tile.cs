using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace Pathfinding
{
    class Tile : GameObject
    {
        private readonly Grid Grid;
        private readonly Graph graph;
        private readonly Vertex vertex;

        private Color color;
        private bool isWall;

        public Vertex Vertex => vertex;
        public bool IsWall => isWall;

        public int X => vertex.X;
        public int Y => vertex.Y;

        public Tile(Vector2 position, Point size, Grid Grid, Graph graph, Vertex vertex) : base(position, size)
        {
            this.Grid = Grid;
            this.graph = graph;
            this.vertex = vertex;

            Update();
        }

        public override void Update()
        {
            isWall = (vertex.EdgeCount == 0);
            color = isWall ? Color.DimGray : Color.LightGray;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DestRect, null, color);
        }

        public void AddWall()
        {
            isWall = true;
            vertex.ClearEdges();

            Update();
        }
        public void RemoveWall()
        {
            if (!isWall)
                return;

            if (Grid.EightDirectional)
                MakeEightDirectional();
            else if (Grid.FourDirectional)
                MakeFourDirectional();

            Update();
        }

        public void MakeEightDirectional()
        {
            Vertex.ClearEdges();

            for (int y = -1; y <= 1; ++y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    if (x == 0 && y == 0)
                        continue;

                    if (!graph.WithinBoard(vertex.X + x, vertex.Y + y))
                        continue;

                    new Edge(vertex, graph.AtPos(vertex.X + x, vertex.Y + y));
                }
            }
        }
        public void MakeFourDirectional()
        {
            Vertex.ClearEdges();

            for (int y = -1; y <= 1; y += 2)
            {
                for (int x = -1; x <= 1; x += 2)
                {
                    if (!graph.WithinBoard(vertex.X + x, vertex.Y + y))
                        continue;

                    new Edge(vertex, graph.AtPos(vertex.X + x, vertex.Y + y));
                }
            }
        }

        public override void LoadContent()
        {
            AssignTexture("Node");
        }
    }
}
