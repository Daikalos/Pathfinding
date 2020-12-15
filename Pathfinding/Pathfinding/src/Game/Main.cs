using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utilities;
using LilyPath;
using Window.Camera;

namespace Pathfinding
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private DrawBatch drawBatch;

        private Texture2D node;
        private SpriteFont font;

        private Graph graph;

        private List<Vertex> path;

        private Vertex current;
        private Vertex start;
        private Vertex goal;

        private Button button_A_Star;
        private Button button_Dijkstra;
        private Button button_BFS;
        private Button button_DFS;
        private Button button_FindPath;
        private Button button_DelPath;
        private Button button_ClearGrid;

        private IPathfinder pathfinder;
        private string currPath;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f); //60 fps
            MaxElapsedTime = TargetElapsedTime;

            IsMouseVisible = true;

            graph = new Graph(128, 64, 128, 32, 4, 4);
            graph.GenerateGraph();

            Camera.Initialize(Window, 600.0f);

            drawBatch = new DrawBatch(GraphicsDevice);

            path = new List<Vertex>();

            button_A_Star = new Button(new Vector2(16, 16), new Point(128, 48), Select_A_Star, "A*", 1.0f, 1.0f, 1.05f);
            button_Dijkstra = new Button(new Vector2(16, 80), new Point(128, 48), Select_Dijkstra, "Dijkstra", 0.40f, 1.0f, 1.05f);
            button_BFS = new Button(new Vector2(16, 144), new Point(128, 48), Select_BFS, "BFS", 1.0f, 1.0f, 1.05f);
            button_DFS = new Button(new Vector2(16, 208), new Point(128, 48), Select_DFS, "DFS", 1.0f, 1.0f, 1.05f);
            button_FindPath = new Button(new Vector2(16, Window.ClientBounds.Height - 128 - 32), new Point(452, 64), FindPath, "Find Path", 1.0f, 1.0f, 1.02f);
            button_DelPath = new Button(new Vector2(16, Window.ClientBounds.Height - 64 - 16), new Point(452, 64), DelPath, "Delete Path", 1.0f, 1.0f, 1.02f);
            button_ClearGrid = new Button(new Vector2(Window.ClientBounds.Width - 128 - 16, Window.ClientBounds.Height - 48 - 16), new Point(128, 48), ClearGrid, "Clear", 0.6f, 1.0f, 1.05f);

            pathfinder = null;
            currPath = String.Empty;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddTexture("Node", this.Content.Load<Texture2D>("Sprites/node"));
            ResourceManager.AddTexture("Border_Short", this.Content.Load<Texture2D>("Sprites/border_short"));
            ResourceManager.AddTexture("Border_Long", this.Content.Load<Texture2D>("Sprites/border_long"));
            ResourceManager.AddTexture("Null", this.Content.Load<Texture2D>("Sprites/null"));

            ResourceManager.AddFont("8bit", this.Content.Load<SpriteFont>("Fonts/8bit"));

            button_A_Star.SetTexture("Border_Short");
            button_Dijkstra.SetTexture("Border_Short");
            button_BFS.SetTexture("Border_Short");
            button_DFS.SetTexture("Border_Short");
            button_FindPath.SetTexture("Border_Long");
            button_DelPath.SetTexture("Border_Long");
            button_ClearGrid.SetTexture("Border_Short");

            node = ResourceManager.RequestTexture("Node");
            font = ResourceManager.RequestFont("8bit");
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            Camera.MoveCamera(gameTime);

            button_A_Star.Update(Window);
            button_Dijkstra.Update(Window);
            button_BFS.Update(Window);
            button_DFS.Update(Window);
            button_FindPath.Update(Window);
            button_DelPath.Update(Window);
            button_ClearGrid.Update(Window);

            current = graph.AtMousePos(Camera.ViewToWorld(KeyMouseReader.MousePos));

            if (KeyMouseReader.KeyPressed(Keys.Q))
            {
                start = graph.AtMousePos(Camera.ViewToWorld(KeyMouseReader.MousePos));
            }
            if (KeyMouseReader.KeyPressed(Keys.E))
            {
                goal = graph.AtMousePos(Camera.ViewToWorld(KeyMouseReader.MousePos));
            }

            if (KeyMouseReader.LeftHold())
            {
                DelWall(graph.AtMousePos(Camera.ViewToWorld(KeyMouseReader.MousePos)));
            }
            if (KeyMouseReader.RightHold())
            {
                AddWall(graph.AtMousePos(Camera.ViewToWorld(KeyMouseReader.MousePos)));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DodgerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp, null, null, null, Camera.TranslationMatrix);
            drawBatch.Begin(DrawSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp, null, null, null, Camera.TranslationMatrix);

            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x) // Draw Grid
                {
                    bool isWall = (graph.AtPos(new Point(x, y)).EdgeCount == 0);
                    Color color = (!isWall) ? Color.LightGray : Color.DimGray;

                    spriteBatch.Draw(node, new Rectangle(x * (graph.VertW + graph.GapW), y * (graph.VertH + graph.GapH), graph.VertW, graph.VertH), color);
                }
            }

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

            button_A_Star.Draw(spriteBatch);
            button_Dijkstra.Draw(spriteBatch);
            button_BFS.Draw(spriteBatch);
            button_DFS.Draw(spriteBatch);
            button_FindPath.Draw(spriteBatch);
            button_DelPath.Draw(spriteBatch);
            button_ClearGrid.Draw(spriteBatch);

            StringManager.CameraDrawStringLeft(spriteBatch, font, "Path: " + currPath, new Vector2(160, 40), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, font, "Curr:", new Vector2(16, 300), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, font, "Start:", new Vector2(16, 340), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, font, "Goal: ", new Vector2(16, 380), new Color(59, 76, 93), 0.9f);
            StringManager.CameraDrawStringLeft(spriteBatch, font, "Cnt: ", new Vector2(16, 440), new Color(59, 76, 93), 0.9f);

            if (current != null)
                StringManager.CameraDrawStringLeft(spriteBatch, font, current.Position.X + "," + current.Position.Y, new Vector2(200, 300), new Color(59, 76, 93), 0.9f);
            if (start != null)
                StringManager.CameraDrawStringLeft(spriteBatch, font, start.Position.X + "," + start.Position.Y, new Vector2(200, 340), new Color(59, 76, 93), 0.9f);
            if (goal != null)
                StringManager.CameraDrawStringLeft(spriteBatch, font, goal.Position.X + "," + goal.Position.Y, new Vector2(200, 380), new Color(59, 76, 93), 0.9f);

            StringManager.CameraDrawStringLeft(spriteBatch, font, path.Count.ToString(), new Vector2(200, 440), new Color(59, 76, 93), 0.9f);

            spriteBatch.End();
            drawBatch.End();

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

            path = pathfinder.PathTo(graph, start, goal);
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
                    DelWall(graph.AtPos(x, y));
                }
            }
        }

        private void AddWall(Vertex vertex)
        {
            if (vertex == goal || vertex == null || vertex.EdgeCount == 0)
                return;

            vertex.Edges.Clear();
        }
        private void DelWall(Vertex vertex)
        {
            if (vertex == null || vertex.EdgeCount > 0)
                return;

            for (int y = -1; y <= 1; ++y) // Left and Right
            {
                for (int x = -1; x <= 1; ++x) //Top and Bottom
                {
                    if (y == 0 && x == 0)
                        continue;

                    if (graph.WithinBoard(vertex.Position.X + x, vertex.Position.Y + y))
                    {
                        new Edge(vertex, graph.AtPos(vertex.Position.X + x, vertex.Position.Y + y));
                    }
                }
            }
        }
    }
}
