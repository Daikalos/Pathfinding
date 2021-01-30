using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utilities;
using LilyPath;
using Window;
using UI;

namespace Pathfinding
{
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private DrawBatch drawBatch;

        private List<Button> buttons;
        private Camera camera;

        private SpriteFont font;

        private Graph graph;
        private Map map;

        private List<Vertex> path;
        private Tile current, start, goal;

        private IPathfinder pathfinder;
        private string currPath;
        private string dirPath;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 144.0f);

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            camera = new Camera(Window, new Vector2(0.25f, 4.0f), 600.0f, 0.0f, 0.08f);

            buttons = new List<Button>()
            {
                new Button(new Vector2(16, 16), new Point(128, 48),
                    ButtonType.Small, Select_A_Star, "A*", 1.0f, 1.0f, 1.05f),
                new Button(new Vector2(16, 80), new Point(128, 48),
                    ButtonType.Small, Select_Dijkstra, "Dijkstra", 0.40f, 1.0f, 1.05f),
                new Button(new Vector2(16, 144), new Point(128, 48),
                    ButtonType.Small, Select_BFS, "BFS", 1.0f, 1.0f, 1.05f),
                new Button(new Vector2(16, 208), new Point(128, 48),
                    ButtonType.Small, Select_DFS, "DFS", 1.0f, 1.0f, 1.05f),

                new Button(new Vector2(16, Window.ClientBounds.Height - 128 - 32), new Point(452, 64),
                    ButtonType.Wide, FindPath, "Find Path", 1.0f, 1.0f, 1.02f),
                new Button(new Vector2(16, Window.ClientBounds.Height - 64 - 16), new Point(452, 64),
                    ButtonType.Wide, DelPath, "Delete Path", 1.0f, 1.0f, 1.02f),

                new Button(new Vector2(Window.ClientBounds.Width - 128 - 16, 16), new Point(128, 48),
                    ButtonType.Small, EightDir, "8-Dir", 0.6f, 1.0f, 1.05f),
                new Button(new Vector2(Window.ClientBounds.Width - 128 - 16, 80), new Point(128, 48),
                    ButtonType.Small, FourDir, "4-Dir", 0.6f, 1.0f, 1.05f),

                new Button(new Vector2(Window.ClientBounds.Width - 128 - 16, 144), new Point(128, 48),
                    ButtonType.Small, ClearGrid, "Clear", 0.6f, 1.0f, 1.05f)
            };

            graph = new Graph(64, 64);
            graph.Generate();

            map = new Map(graph, 32, 32, 4, 4);
            map.Generate();

            path = new List<Vertex>();

            pathfinder = null;
            currPath = string.Empty;
            dirPath = "8-Dir";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawBatch = new DrawBatch(GraphicsDevice);

            ResourceManager.AddTexture("Node", this.Content.Load<Texture2D>("Sprites/node"));
            ResourceManager.AddTexture("Button_Small", this.Content.Load<Texture2D>("Sprites/button_small"));
            ResourceManager.AddTexture("Button_Wide", this.Content.Load<Texture2D>("Sprites/button_wide"));
            ResourceManager.AddTexture("Null", this.Content.Load<Texture2D>("Sprites/null"));

            ResourceManager.AddFont("8bit", this.Content.Load<SpriteFont>("Fonts/8bit"));

            buttons.ForEach(b => b.LoadContent());

            font = ResourceManager.RequestFont("8bit");
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            camera.Update(gameTime);

            buttons.ForEach(b => b.Update(Window));

            Point mousePos = camera.ViewToWorld(KeyMouseReader.MousePos);
            current = map.AtMousePos(mousePos);

            if (KeyMouseReader.KeyPressed(Keys.Q))
            {
                if (!current.IsWall)
                    start = current;
            }
            if (KeyMouseReader.KeyPressed(Keys.E))
            {
                if (!current.IsWall)
                    goal = current;
            }

            if (KeyMouseReader.LeftHold())
                RemoveWall(current);
            if (KeyMouseReader.RightHold())
                AddWall(current);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DodgerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicWrap, null, null, null, camera.WorldMatrix);

            // DRAW LEVEL

            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x) // Draw Grid
                {
                    bool isWall = (graph.AtPos(new Point(x, y)).EdgeCount == 0);
                    Color color = (!isWall) ? Color.LightGray : Color.DimGray;

                    spriteBatch.Draw(node, new Rectangle(
                        x * (graph.VertW + graph.GapW), 
                        y * (graph.VertH + graph.GapH), graph.VertW, graph.VertH), color);
                }
            }

            if (start != null)
            {
                StringManager.DrawStringMid(spriteBatch, font, "S", new Vector2(
                    start.Position.X * (graph.VertW + graph.GapW) + (graph.VertW / 2),
                    start.Position.Y * (graph.VertH + graph.GapH) + (graph.VertH / 2)), Color.Firebrick, 1.0f);
            }

            if (goal != null)
            {
                StringManager.DrawStringMid(spriteBatch, font, "G", new Vector2(
                    goal.Position.X * (graph.VertW + graph.GapW) + (graph.VertW / 2),
                    goal.Position.Y * (graph.VertH + graph.GapH) + (graph.VertH / 2)), Color.Firebrick, 1.0f);
            }

            drawBatch.Begin(DrawSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicWrap, null, null, null, camera.WorldMatrix);

            for (int i = 0; i < path.Count - 1; i++) // Draw Path
            {
                Vector2 pos0 = new Vector2(
                    path[i].Position.X * (graph.VertW + graph.GapW) + (graph.VertW / 2), 
                    path[i].Position.Y * (graph.VertH + graph.GapH) + (graph.VertH / 2));
                Vector2 pos1 = new Vector2(
                    path[i + 1].Position.X * (graph.VertW + graph.GapW) + (graph.VertW / 2), 
                    path[i + 1].Position.Y * (graph.VertH + graph.GapH) + (graph.VertH / 2));

                drawBatch.DrawLine(new Pen(Color.IndianRed, 8), pos0, pos1);
            }

            drawBatch.End();

            // -- USER INTERFACE --

            buttons.ForEach(b => b.Draw(spriteBatch, camera));

            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, "Path: " + currPath, new Vector2(160, 40), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, "Curr:", new Vector2(16, 300), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, "Start:", new Vector2(16, 340), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, "Goal: ", new Vector2(16, 380), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, "Cnt: ", new Vector2(16, 440), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, "Dir: ", new Vector2(16, 480), new Color(59, 76, 93), 0.9f);

            if (current != null)
                StringManager.CameraDrawStringLeft(spriteBatch, camera, font, current.Position.X + "," + current.Position.Y, new Vector2(200, 300), new Color(59, 76, 93), 0.9f);
            if (start != null)
                StringManager.CameraDrawStringLeft(spriteBatch, camera, font, start.Position.X + "," + start.Position.Y, new Vector2(200, 340), new Color(59, 76, 93), 0.9f);
            if (goal != null)
                StringManager.CameraDrawStringLeft(spriteBatch, camera, font, goal.Position.X + "," + goal.Position.Y, new Vector2(200, 380), new Color(59, 76, 93), 0.9f);

            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, path.Count.ToString(), new Vector2(200, 440), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, camera, font, dirPath, new Vector2(200, 480), new Color(59, 76, 93), 0.9f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Select_A_Star(GameWindow window)
        {
            pathfinder = new A_Star();
            currPath = "A*";
        }
        private void Select_Dijkstra(GameWindow window)
        {
            pathfinder = new Dijkstra();
            currPath = "Dijkstra";
        }
        private void Select_BFS(GameWindow window)
        {
            pathfinder = new BFS();
            currPath = "BFS";
        }
        private void Select_DFS(GameWindow window)
        {
            pathfinder = new DFS();
            currPath = "DFS";
        }

        private void FindPath(GameWindow window)
        {
            if (pathfinder == null || start == null || goal == null)
                return;

            path = pathfinder.PathTo(graph, start.Vertex, goal.Vertex);
        }
        private void DelPath(GameWindow window)
        {
            start = null;
            goal = null;
            path.Clear();
            pathfinder = null;
            currPath = string.Empty;
        }
        private void ClearGrid(GameWindow window)
        {
            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    RemoveWall(map.AtPos(x, y));
                }
            }
        }
        private void EightDir(GameWindow window)
        {
            dirPath = "8-Dir";
            map.MakeEightDirectional();
        }
        private void FourDir(GameWindow window)
        {
            dirPath = "4-Dir";
            map.MakeFourDirectional();
        }

        private void AddWall(Tile tile)
        {
            if (tile == null || tile.IsWall || tile == start || tile == goal)
                return;

            tile.Vertex.Edges.Clear();
        }
        private void RemoveWall(Tile tile)
        {
            if (tile == null || !tile.IsWall)
                return;

            for (int y = -1; y <= 1; ++y) // Left and Right
            {
                for (int x = -1; x <= 1; ++x) //Top and Bottom
                {
                    if (y == 0 && x == 0)
                        continue;

                    if (!graph.WithinBoard(vertex.Position.X + x, vertex.Position.Y + y))
                        continue;

                    new Edge(vertex, graph.AtPos(vertex.Position.X + x, vertex.Position.Y + y));
                }
            }
        }
    }
}
