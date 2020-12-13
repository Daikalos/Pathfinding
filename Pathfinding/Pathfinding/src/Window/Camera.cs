using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utilities;

namespace Window.Camera
{
    static class Camera
    {
        private static Vector2 
            position,
            viewportSize,
            zoomLimit;
        private static Point 
            mouseOldPosition;
        private static float 
            moveSpeed,
            zoom,
            zoomValue;

        public static Matrix TranslationMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateScale(zoom, zoom, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public static Vector2 Position
        {
            get => position;
            set => position = value;
        }
        public static Vector2 TopLeftCorner
        {
            get => position - ViewportCenter * (1 / zoom);
        }
        public static Vector2 ViewportCenter
        {
            get => new Vector2(viewportSize.X / 2, viewportSize.Y / 2);
        }

        public static float Zoom
        {
            get => zoom;
        }

        public static void Initialize(GameWindow window, float speed)
        {
            position = ViewportCenter;
            viewportSize = window.ClientBounds.Size.ToVector2();
            zoomLimit = new Vector2(0.25f, 4.0f);

            mouseOldPosition = Point.Zero;

            moveSpeed = speed;
            zoom = 1.0f;
            zoomValue = 0.05f;
        }

        public static void Reset()
        {
            position = ViewportCenter;
            mouseOldPosition = Point.Zero;
            zoom = 1.0f;
        }

        public static void MoveCamera(GameTime gameTime)
        {
            MouseMovement();
            KeyboardMovement(gameTime);
        }

        private static void MouseMovement()
        {
            if (KeyMouseReader.MiddleMouseClick())
            {
                mouseOldPosition = ViewToWorld(KeyMouseReader.MousePos.ToVector2()).ToPoint();
            }
            if (KeyMouseReader.MiddleMouseHold() && mouseOldPosition != Point.Zero)
            {
                Point newPos = ViewToWorld(KeyMouseReader.MousePos.ToVector2()).ToPoint();
                Point deltaPos = mouseOldPosition - newPos;

                position += deltaPos.ToVector2();
            }

            if (KeyMouseReader.ScrollUp())
            {
                zoom += zoomValue;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
            if (KeyMouseReader.ScrollDown())
            {
                zoom -= zoomValue;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
        }
        private static void KeyboardMovement(GameTime gameTime)
        {
            if (KeyMouseReader.KeyHold(Keys.Up))
            {
                position.Y -= moveSpeed * (1 / zoom) * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Down))
            {
                position.Y += moveSpeed * (1 / zoom) * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Left))
            {
                position.X -= moveSpeed * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Right))
            {
                position.X += moveSpeed * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (KeyMouseReader.KeyHold(Keys.OemPlus))
            {
                zoom += zoomValue / 2;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
            if (KeyMouseReader.KeyHold(Keys.OemMinus))
            {
                zoom -= zoomValue / 2;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
        }

        public static Vector2 ViewToWorld(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(TranslationMatrix));
        }
    }
}
