using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utilities;
using LilyPath;
using Pathfinding;
using Window;
using UI;

namespace Pathfinding
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private DrawBatch drawBatch;

        private Thread findPathThread;
        private Thread generateMazeThread;

        private List<Button> buttons;
        private Camera camera;

        private SpriteFont font;

        private Graph graph;
        private Grid grid;

        private List<Tile> path;
        private Tile current, start, goal;

        private IPathfinder pathfinder;
        private IMaze maze;

        private string currPath;
        private string dirPath;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef,
                SynchronizeWithVerticalRetrace = false
            };

            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            IsFixedTimeStep = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            camera = new Camera(Window, new Vector2(0.025f, 4.0f), 600.0f, 0.0f, 0.05f);

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
                    ButtonType.Small, ClearGrid, "Clear", 0.6f, 1.0f, 1.05f),

                new Button(new Vector2(Window.ClientBounds.Width - 452 - 16, Window.ClientBounds.Height - 64 - 16), new Point(452, 64),
                    ButtonType.Wide, GenerateMaze, "Generate Maze", 0.9f, 1.0f, 1.02f)
            };

            graph = new Graph(128, 128);
            graph.Generate();

            grid = new Grid(graph, 32, 32, 8, 8);
            grid.Generate();

            maze = new RDFS();

            path = new List<Tile>();

            pathfinder = null;
            currPath = string.Empty;
            dirPath = "8-Dir";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawBatch = new DrawBatch(GraphicsDevice);

            ResourceManager.AddTexture("Node", Content.Load<Texture2D>("Sprites/node"));
            ResourceManager.AddTexture("Button_Small", Content.Load<Texture2D>("Sprites/button_small"));
            ResourceManager.AddTexture("Button_Wide", Content.Load<Texture2D>("Sprites/button_wide"));
            ResourceManager.AddTexture("Null", Content.Load<Texture2D>("Sprites/null"));

            ResourceManager.AddFont("8bit", Content.Load<SpriteFont>("Fonts/8bit"));

            grid.LoadContent();
            buttons.ForEach(b => b.LoadContent());

            font = ResourceManager.RequestFont("8bit");
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyMouseReader.Update();
            camera.Update(deltaTime);

            buttons.ForEach(b => b.Update());

            Point mousePos = camera.ViewToWorld(KeyMouseReader.MousePos);
            current = grid.AtMousePos(mousePos);

            if (current != null)
            {
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
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);

            // DRAW LEVEL

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicWrap, null, null, null, camera.WorldMatrix);

            drawBatch.Begin(DrawSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicWrap, null, null, null, camera.WorldMatrix);
            
            /*
            for (int y = 0; y < graph.Width; ++y)
            {
                for (int x = 0; x < graph.Height; ++x)
                {
                    Tile tile = grid.AtPos(x, y);
                    List<Edge> edges = new List<Edge>(tile.Vertex.Edges);

                    foreach (Edge edge in edges)
                    {
                        drawBatch.DrawLine(new Pen(new Color(0, 0, 100, 200), 5), tile.Middle, grid.AtPos(edge.To).Middle);
                    }
                }
            }*/

            drawBatch.End();

            grid.Draw(spriteBatch);

            spriteBatch.End();

            drawBatch.Begin(DrawSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicWrap, null, null, null, camera.WorldMatrix);

            for (int i = 0; i < path.Count - 1; i++) // Draw Path
                drawBatch.DrawLine(new Pen(new Color(0, 255, 0, 180), 8), path[i].Middle, path[i + 1].Middle);

            drawBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicWrap, null, null, null, camera.WorldMatrix);

            if (start != null)
                StringUtilities.DrawM(spriteBatch, font, "S", start.Middle, Color.Red, 1.0f);

            if (goal != null)
                StringUtilities.DrawM(spriteBatch, font, "G", goal.Middle, Color.Red, 1.0f);

            // -- USER INTERFACE --

            buttons.ForEach(b => b.Draw(spriteBatch, camera));

            StringUtilities.DrawLC(spriteBatch, camera, font, "Path: " + currPath, new Vector2(160, 40), new Color(59, 76, 93), 0.9f);
            StringUtilities.DrawLC(spriteBatch, camera, font, "Curr:", new Vector2(16, 300), new Color(59, 76, 93), 0.9f);
            StringUtilities.DrawLC(spriteBatch, camera, font, "Start:", new Vector2(16, 340), new Color(59, 76, 93), 0.9f);
            StringUtilities.DrawLC(spriteBatch, camera, font, "Goal: ", new Vector2(16, 380), new Color(59, 76, 93), 0.9f);
            StringUtilities.DrawLC(spriteBatch, camera, font, "Cnt: ", new Vector2(16, 440), new Color(59, 76, 93), 0.9f);
            StringUtilities.DrawLC(spriteBatch, camera, font, "Dir: ", new Vector2(16, 480), new Color(59, 76, 93), 0.9f);

            if (current != null)
                StringUtilities.DrawLC(spriteBatch, camera, font, current.X + ";" + current.Y, new Vector2(200, 300), new Color(59, 76, 93), 0.9f);
            if (start != null)
                StringUtilities.DrawLC(spriteBatch, camera, font, start.X + ";" + start.Y, new Vector2(200, 340), new Color(59, 76, 93), 0.9f);
            if (goal != null)
                StringUtilities.DrawLC(spriteBatch, camera, font, goal.X + ";" + goal.Y, new Vector2(200, 380), new Color(59, 76, 93), 0.9f);

            StringUtilities.DrawLC(spriteBatch, camera, font, path.Count.ToString(), new Vector2(200, 440), new Color(59, 76, 93), 0.9f);
            StringUtilities.DrawLC(spriteBatch, camera, font, dirPath, new Vector2(200, 480), new Color(59, 76, 93), 0.9f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Select_A_Star()
        {
            pathfinder = new A_Star();
            currPath = "A*";
        }
        private void Select_Dijkstra()
        {
            pathfinder = new Dijkstra();
            currPath = "Dijkstra";
        }
        private void Select_BFS()
        {
            pathfinder = new BFS();
            currPath = "BFS";
        }
        private void Select_DFS()
        {
            pathfinder = new DFS();
            currPath = "DFS";
        }

        private void FindPath()
        {
            if (pathfinder == null || start == null || goal == null)
                return;

            if (findPathThread != null && findPathThread.IsAlive)
                findPathThread.Abort();

            findPathThread = new Thread(new ThreadStart(() => 
            {
                grid.UpdateColor();
                path = grid.GetPath(pathfinder.PathTo(grid, graph, start.Vertex, goal.Vertex));
            })) 
            { 
                IsBackground = true
            };
            findPathThread.Start();
        }
        private void DelPath()
        {
            currPath = string.Empty;

            start = null;
            goal = null;
            pathfinder = null;

            path.Clear();
            grid.UpdateColor();
        }

        private void ClearGrid()
        {
            if (generateMazeThread != null && generateMazeThread.IsAlive)
                generateMazeThread.Abort();

            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    RemoveWall(grid.AtPos(x, y));
                }
            }
        }
        private void EightDir()
        {
            dirPath = "8-Dir";
            grid.MakeEightDirectional();
        }
        private void FourDir()
        {
            dirPath = "4-Dir";
            grid.MakeFourDirectional();
        }

        private void GenerateMaze()
        {
            if (generateMazeThread != null && generateMazeThread.IsAlive)
                generateMazeThread.Abort();

            generateMazeThread = new Thread(new ThreadStart(() => 
            { 
                maze.Generate(graph, grid, graph.AtPos(0, 0)); 
            })) 
            { 
                IsBackground = true 
            };
            generateMazeThread.Start();
        }

        private void AddWall(Tile tile)
        {
            if (tile == null || tile.IsWall || tile == start || tile == goal)
                return;

            tile.AddWall();
        }
        private void RemoveWall(Tile tile)
        {
            tile.RemoveWall();
        }
    }
}
