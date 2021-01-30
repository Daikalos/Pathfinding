using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pathfinding
{
    class Map
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

        public Map(Graph graph, int tileWidth, int tileHeight, int tileGapWidth, int tileGapHeight)
        {
            this.graph = graph;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileGapWidth = tileGapWidth;
            this.tileGapHeight = tileGapHeight;

            eightDirectional = true;
            fourDirectional = false;
        }

        public void RemoveWall(Tile tile)
        {
            if (eightDirectional)
                MakeEightDirectional(tile);
            else if (fourDirectional)
                MakeFourDirectional(tile);
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

                    MakeEightDirectional(tile);
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

                    MakeFourDirectional(tile);
                }
            }

            fourDirectional = true;
            eightDirectional = false;
        }

        public void MakeEightDirectional(Tile tile)
        {
            tile.Vertex.ClearEdges();

            for (int y = -1; y <= 1; ++y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Vertex vertex = tile.Vertex;

                    if (!graph.WithinBoard(vertex.X + x, vertex.Y + y))
                        continue;

                    new Edge(vertex, graph.AtPos(vertex.X + x, vertex.Y + y));
                }
            }
        }
        public void MakeFourDirectional(Tile tile)
        {
            tile.Vertex.ClearEdges();

            for (int y = -1; y <= 1; y += 2)
            {
                for (int x = -1; x <= 1; x += 2)
                {
                    Vertex vertex = tile.Vertex;

                    if (!graph.WithinBoard(vertex.X + x, vertex.Y + y))
                        continue;

                    new Edge(vertex, graph.AtPos(vertex.X + x, vertex.Y + y));
                }
            }
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
                        new Point(tileWidth, tileHeight), graph.AtPos(x, y)));
                }
            }
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
    }
}
