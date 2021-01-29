using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Window.Camera;

namespace Utilities
{
    static class StringManager
    {
        /// <summary>
        /// Draw a string with the farthest left point of the string as reference (standard, but includes Y correction as origin)
        /// </summary>
        public static void DrawStringLeft(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
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
        public static void DrawStringMid(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
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
        public static void DrawStringRight(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
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
        public static void CameraDrawStringLeft(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, Camera.TopLeftCorner + new Vector2(
                (position.X / Camera.Zoom), 
                (position.Y / Camera.Zoom) - (font.MeasureString(text).Y / 2) * scale / Camera.Zoom), color, 0.0f, Vector2.Zero, scale / Camera.Zoom, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the middle point of the string as reference using the camera
        /// </summary>
        public static void CameraDrawStringMid(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, Camera.TopLeftCorner + new Vector2(
                (position.X / Camera.Zoom) - (font.MeasureString(text).X / 2) * scale / Camera.Zoom,
                (position.Y / Camera.Zoom) - (font.MeasureString(text).Y / 2) * scale / Camera.Zoom), color, 0.0f, Vector2.Zero, scale / Camera.Zoom, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draw a string with the farthest right point of the string as reference using the camera
        /// </summary>
        public static void CameraDrawStringRight(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
        {
            if (font == null)
                return;

            spriteBatch.DrawString(font, text, Camera.TopLeftCorner + new Vector2(
                (position.X / Camera.Zoom) - (font.MeasureString(text).X) * scale / Camera.Zoom,
                (position.Y / Camera.Zoom) - (font.MeasureString(text).Y / 2) * scale / Camera.Zoom), color, 0.0f, Vector2.Zero, scale / Camera.Zoom, SpriteEffects.None, 0.0f);
        }
    }
}
