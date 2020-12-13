using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utilities;
using Window.Camera;

namespace Pathfinding
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Graph graph;

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
            Camera.Initialize(Window, 2.5f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddTexture("Node", this.Content.Load<Texture2D>("Textures/node"));
            ResourceManager.AddTexture("Null", this.Content.Load<Texture2D>("Textures/null"));

            ResourceManager.AddFont("8bit", this.Content.Load<SpriteFont>("Fonts/8bit"));
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
