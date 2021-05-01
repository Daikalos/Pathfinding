using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utilities
{
    static class StringUtilities
    {
        /// <summary>
        /// Draw a string with the farthest left point of the string as reference (standard, but includes Y correction as origin)
        /// </summary>
        public static void DrawLeft(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            spriteBatch.DrawString(font, text, position, color, 0.0f, new Vector2(0.0f, font.MeasureString(text).Y / 2.0f), scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the middle point of the string as reference
        /// </summary>
        public static void DrawMiddle(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            spriteBatch.DrawString(font, text, position, color, 0.0f, font.MeasureString(text) / 2.0f, scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the farthest right point of the string as reference
        /// </summary>
        public static void DrawRight(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            spriteBatch.DrawString(font, text, position, color, 0.0f, new Vector2(font.MeasureString(text).X, font.MeasureString(text).Y / 2.0f), scale, SpriteEffects.None, 0.0f);
        }
    }
}
