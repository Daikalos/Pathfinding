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

        private Graph graph;

        private List<Vertex> curPath;

        private Button button_A_Star;
        private Button button_Dijsktra;
        private Button button_BFS;
        private Button button_DFS;

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

            graph = new Graph(64, 64);
            Camera.Initialize(Window, 20.0f);

            drawBatch = new DrawBatch(GraphicsDevice);

            curPath = new List<Vertex>();

            button_A_Star = new Button(new Vector2(16, 16), new Point(128, 48), FindPath_A_Star, "A*", 1.0f, 1.0f, 1.05f);
            button_Dijsktra = new Button(new Vector2(16, 80), new Point(128, 48), FindPath_Dijsktra, "Dijkstra", 0.40f, 1.0f, 1.05f);
            button_BFS = new Button(new Vector2(16, 144), new Point(128, 48), FindPath_BFS, "BFS", 1.0f, 1.0f, 1.05f);
            button_DFS = new Button(new Vector2(16, 208), new Point(128, 48), FindPath_DFS, "DFS", 1.0f, 1.0f, 1.05f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddTexture("Node", this.Content.Load<Texture2D>("Sprites/node"));
            ResourceManager.AddTexture("Border_Short", this.Content.Load<Texture2D>("Sprites/border_short"));
            ResourceManager.AddTexture("Null", this.Content.Load<Texture2D>("Sprites/null"));

            ResourceManager.AddFont("8bit", this.Content.Load<SpriteFont>("Fonts/8bit"));

            button_A_Star.SetTexture("Border_Short");
            button_Dijsktra.SetTexture("Border_Short");
            button_BFS.SetTexture("Border_Short");
            button_DFS.SetTexture("Border_Short");
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            Camera.MoveCamera(gameTime);

            button_A_Star.Update(Window);
            button_Dijsktra.Update(Window);
            button_BFS.Update(Window);
            button_DFS.Update(Window);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkRed);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp, null, null, null, Camera.TranslationMatrix);

            Texture2D node = ResourceManager.RequestTexture("Node");
            for (int y = 0; y < graph.Height; ++y)
            {
                for (int x = 0; x < graph.Width; ++x)
                {
                    spriteBatch.Draw(node, new Rectangle(x * (node.Width + 4), y * (node.Height + 4), node.Width, node.Height), Color.DarkSlateGray);
                }
            }

            button_A_Star.Draw(spriteBatch);
            button_Dijsktra.Draw(spriteBatch);
            button_BFS.Draw(spriteBatch);
            button_DFS.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void FindPath_A_Star(GameWindow window)
        {

        }
        private void FindPath_Dijsktra(GameWindow window)
        {

        }

        private void FindPath_BFS(GameWindow window)
        {

        }
        private void FindPath_DFS(GameWindow window)
        {

        }
    }
}
