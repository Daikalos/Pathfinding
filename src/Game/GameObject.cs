using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

abstract class GameObject
{
    public Texture2D Texture { get; set; }

    public Vector2 Position { get; set; }
    public Vector2 Origin { get; private set; }

    public virtual Rectangle BoundingBox => DestRect;
    public virtual Rectangle DestRect { get; set; }
    public virtual Rectangle SourceRect { get; set; }

    public Vector2 Middle => DestRect.Center.ToVector2();

    public Point Size { get; set; }

    protected GameObject(Vector2 position, Point size)
    {
        Position = position;
        Size = size;

        Origin = Vector2.Zero;
        DestRect = new Rectangle(
            (int)position.X, (int)position.Y, Size.X, Size.Y);
    }

    public virtual void Update()
    {
        DestRect = new Rectangle((int)Position.X, (int)Position.Y, Size.X, Size.Y);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, DestRect, SourceRect, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, DestRect, SourceRect, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, float depth)
    {
        spriteBatch.Draw(Texture, DestRect, SourceRect, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
    }

    public virtual void AssignTexture(string name)
    {
        Texture2D txt = ResourceManager.RequestTexture(name);

        if (txt == null)
            return;

        Texture = txt;
        SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
        SetOrigin(1, 1);
    }

    public abstract void LoadContent();

    public void SetOrigin(Point frames)
    {
        if (Texture == null || Texture.IsDisposed)
            return;

        Origin = new Vector2(Texture.Width / 2 / frames.X, Texture.Height / 2 / frames.Y);
    }
    public void SetOrigin(int x, int y)
    {
        if (Texture == null || Texture.IsDisposed)
            return;

        Origin = new Vector2(Texture.Width / 2 / x, Texture.Height / 2 / y);
    }
}
