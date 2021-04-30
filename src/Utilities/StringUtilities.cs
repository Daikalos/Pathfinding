using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Window;

namespace Utilities
{
    static class StringUtilities
    {
        /// <summary>
        /// Draw a string with the farthest left point of the string as reference (standard, but includes Y correction as origin)
        /// </summary>
        public static void DrawL(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, new Vector2(
                (position.X),
                (position.Y - (font.MeasureString(text).Y / 2) * scale)), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the middle point of the string as reference
        /// </summary>
        public static void DrawM(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, new Vector2(
                (position.X - (font.MeasureString(text).X / 2) * scale),
                (position.Y - (font.MeasureString(text).Y / 2) * scale)), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the farthest right point of the string as reference
        /// </summary>
        public static void DrawR(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, new Vector2(
                (position.X) - (font.MeasureString(text).X * scale),
                (position.Y) - (font.MeasureString(text).Y / 2) * scale), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the farthest left point of the string as reference (standard, but includes Y correction as origin) using the camera
        /// </summary>
        public static void DrawLC(SpriteBatch spriteBatch, Camera camera, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, camera.TopLeftCorner + new Vector2(
                (position.X / camera.Zoom), 
                (position.Y / camera.Zoom) - (font.MeasureString(text).Y / 2) * scale / camera.Zoom), color, 0.0f, Vector2.Zero, scale / camera.Zoom, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the middle point of the string as reference using the camera
        /// </summary>
        public static void DrawMC(SpriteBatch spriteBatch, Camera camera, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            spriteBatch.DrawString(font, text, camera.TopLeftCorner + new Vector2(
                (position.X / camera.Zoom) - (font.MeasureString(text).X / 2) * scale / camera.Zoom,
                (position.Y / camera.Zoom) - (font.MeasureString(text).Y / 2) * scale / camera.Zoom), color, 0.0f, Vector2.Zero, scale / camera.Zoom, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the farthest right point of the string as reference using the camera
        /// </summary>
        public static void DrawRC(SpriteBatch spriteBatch, Camera camera, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            spriteBatch.DrawString(font, text, camera.TopLeftCorner + new Vector2(
                (position.X / camera.Zoom) - (font.MeasureString(text).X) * scale / camera.Zoom,
                (position.Y / camera.Zoom) - (font.MeasureString(text).Y / 2) * scale / camera.Zoom), color, 0.0f, Vector2.Zero, scale / camera.Zoom, SpriteEffects.None, 0.0f);
        }
    }
}
