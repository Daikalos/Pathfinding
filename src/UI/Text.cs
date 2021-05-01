using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace UI
{
    class Text
    {
        private readonly SpriteFont font;
        private readonly Vector2 position;
        private readonly Color color;
        private readonly float scale;

        private string text;

        public Text(string text, Vector2 position, Color color, float scale, SpriteFont font)
        {
            this.font = font;
            this.position = position;
            this.color = color;
            this.text = text;
            this.scale = scale;
        }

        public void Set(string text)
        {
            this.text = text;
        }

        public void DrawLeft(SpriteBatch spriteBatch)
        {
            StringUtilities.DrawLeft(spriteBatch, font, text, position, color, scale);
        }
        public void DrawMiddle(SpriteBatch spriteBatch)
        {
            StringUtilities.DrawMiddle(spriteBatch, font, text, position, color, scale);
        }
        public void DrawRight(SpriteBatch spriteBatch)
        {
            StringUtilities.DrawRight(spriteBatch, font, text, position, color, scale);
        }
    }
}
