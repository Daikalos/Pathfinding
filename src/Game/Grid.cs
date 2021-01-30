using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pathfinding
{
    class Grid
    {
        private readonly List<Tile> tiles
            = new List<Tile>();
        private readonly Graph graph;

        private readonly int
            tileWidth,
            tileHeight,
            tileGapWidth,
            tileGapHeight;

        private bool
            eightDirectional,
            fourDirectional;

        public int TileWidth => tileWidth;
        public int TileHeight => tileHeight;
        public int TileGapWidth => tileGapWidth;
        public int TileGapHeight => tileGapHeight;

        public bool EightDirectional => eightDirectional;
        public bool FourDirectional => fourDirectional;

        public Grid(Graph graph, int tileWidth, int tileHeight, int tileGapWidth, int tileGapHeight)
        {
            this.graph = graph;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileGapWidth = tileGapWidth;
            this.tileGapHeight = tileGapHeight;

            eightDirectional = true;
            fourDirectional = false;
        }

        public void Generate()
        {
            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    tiles.Add(new Tile(
                        new Vector2(
                            x * (tileWidth + tileGapWidth),
                            y * (tileHeight + tileGapHeight)),
                        new Point(tileWidth, tileHeight), this, graph, graph.AtPos(x, y)));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            tiles.ForEach(t => t.Draw(spriteBatch));
        }

        public void MakeEightDirectional()
        {
            if (eightDirectional)
                return;

            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    Tile tile = AtPos(x, y);

                    if (tile.IsWall)
                        continue;

                    tile.MakeEightDirectional();
                    tile.Update();
                }
            }

            fourDirectional = false;
            eightDirectional = true;
        }
        public void MakeFourDirectional()
        {
            if (fourDirectional)
                return;

            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    Tile tile = AtPos(x, y);

                    if (tile.IsWall)
                        continue;

                    tile.MakeFourDirectional();
                    tile.Update();
                }
            }

            fourDirectional = true;
            eightDirectional = false;
        }

        public Tile AtPos(int x, int y)
        {
            return tiles[x + y * graph.Width];
        }
        public Tile AtMousePos(Point pos)
        {
            int x = pos.X / (tileWidth + tileGapWidth);
            int y = pos.Y / (tileHeight + tileGapHeight);

            if (!graph.WithinBoard(x, y))
                return null;

            return tiles[x + y * graph.Width];
        }

        public void LoadContent()
        {
            tiles.ForEach(t => t.LoadContent());
        }
    }
}
