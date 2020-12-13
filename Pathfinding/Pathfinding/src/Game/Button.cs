using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Window.Camera;

namespace Pathfinding
{
    class Button : GameObject
    {
        private SpriteFont font;
        private OnClickEvent onClickEvent;
        private float scale;
        private readonly string text;
        private readonly float
            textScale,
            upScale,
            saveScale;

        public string Text => text;

        public Button(Vector2 position, Point size, OnClickEvent clickEvent, string text, float textScale, float scale, float upScale) : base(position, size)
        {
            this.onClickEvent = clickEvent;
            this.text = text;
            this.textScale = textScale;
            this.scale = scale;
            this.upScale = upScale;

            saveScale = scale;
        }

        public void Update(GameWindow window)
        {
            base.Update();

            scale = saveScale;

            if (IsClicked())
            {
                onClickEvent?.Invoke(window);
            }
            if (IsHold())
            {
                scale = upScale;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Camera.TopLeftCorner + new Vector2(Position.X + Origin.X * saveScale, Position.Y + Origin.Y * saveScale) / Camera.Zoom, 
                SourceRect, Color.White, 0.0f, Origin, scale / Camera.Zoom, SpriteEffects.None, 0.0f);

            StringManager.CameraDrawStringMid(spriteBatch, font, text, DestRect.Center.ToVector2(), new Color(59, 76, 93), textScale);
        }

        public bool IsClicked()
        {
            return
                KeyMouseReader.LeftClick() &&
                BoundingBox.Contains(KeyMouseReader.MousePos);
        }
        public bool IsHold()
        {
            return BoundingBox.Contains(KeyMouseReader.MousePos);
        }

        public delegate void OnClickEvent(GameWindow window);

        public override void SetTexture(string name)
        {
            base.SetTexture(name);

            font = ResourceManager.RequestFont("8bit");
            SetOrigin(new Point(1));
        }
    }
}
