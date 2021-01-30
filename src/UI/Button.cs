using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Window;

namespace UI
{
    public enum ButtonType
    {
        Small,
        Wide
    }

    class Button : GameObject
    {
        private SpriteFont font;
        private ClickEvent clickEvent;
        private float scale;

        private readonly ButtonType type;
        private readonly string text;
        private readonly float
            textScale,
            upScale,
            defScale;

        public string Text => text;

        public delegate void ClickEvent(GameWindow window);

        public Button(Vector2 position, Point size, 
            ButtonType type, ClickEvent clickEvent, 
            string text, float textScale, float scale, float upScale) : base(position, size)
        {
            this.clickEvent = clickEvent;
            this.type = type;
            this.text = text;
            this.textScale = textScale;
            this.scale = defScale = scale;
            this.upScale = upScale;
        }

        public void Update(GameWindow window)
        {
            base.Update();

            if (IsClicked())
                clickEvent?.Invoke(window);

            scale = IsHold() ? upScale : defScale;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Texture, camera.TopLeftCorner + new Vector2(Position.X + Origin.X * defScale, Position.Y + Origin.Y * defScale) / camera.Zoom, 
                SourceRect, Color.White, 0.0f, Origin, scale / camera.Zoom, SpriteEffects.None, 0.0f);

            StringManager.CameraDrawStringMid(spriteBatch, camera, font, text, DestRect.Center.ToVector2(), new Color(59, 76, 93), textScale);
        }

        public bool IsClicked() 
            => KeyMouseReader.LeftClick() && BoundingBox.Contains(KeyMouseReader.MousePos);
        public bool IsHold() => BoundingBox.Contains(KeyMouseReader.MousePos);

        public override void LoadContent()
        {
            switch (type)
            {
                case ButtonType.Small:
                    AssignTexture("Button_Small"); break;
                case ButtonType.Wide:
                    AssignTexture("Button_Wide"); break;
            }
            font = ResourceManager.RequestFont("8bit");
        }
    }
}
