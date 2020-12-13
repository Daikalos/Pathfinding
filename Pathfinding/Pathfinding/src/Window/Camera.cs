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
            curZoom,
            zoomValue;

        public static Matrix TranslationMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateScale(curZoom, curZoom, 1.0f) *
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
            get => position - ViewportCenter * (1 / curZoom);
        }
        public static Vector2 ViewportCenter
        {
            get => new Vector2(viewportSize.X / 2, viewportSize.Y / 2);
        }

        public static float Zoom
        {
            get => curZoom;
        }

        public static void Initialize(GameWindow window, float speed)
        {
            position = ViewportCenter;
            viewportSize = window.ClientBounds.Size.ToVector2();
            zoomLimit = new Vector2(0.5f, 2.0f);

            mouseOldPosition = Point.Zero;

            moveSpeed = speed;
            curZoom = 1.0f;
            zoomValue = 0.05f;
        }

        public static void Reset()
        {
            position = ViewportCenter;
            mouseOldPosition = Point.Zero;
            curZoom = 1.0f;
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
                curZoom += zoomValue;
                curZoom = MathHelper.Clamp(curZoom, zoomLimit.X, zoomLimit.Y);
            }
            if (KeyMouseReader.ScrollDown())
            {
                curZoom -= zoomValue;
                curZoom = MathHelper.Clamp(curZoom, zoomLimit.X, zoomLimit.Y);
            }
        }
        private static void KeyboardMovement(GameTime gameTime)
        {
            if (KeyMouseReader.KeyHold(Keys.Up))
            {
                position.Y -= moveSpeed * (1 / curZoom) * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Down))
            {
                position.Y += moveSpeed * (1 / curZoom) * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                curZoom += zoomValue / 2;
                curZoom = MathHelper.Clamp(curZoom, zoomLimit.X, zoomLimit.Y);
            }
            if (KeyMouseReader.KeyHold(Keys.OemMinus))
            {
                curZoom -= zoomValue / 2;
                curZoom = MathHelper.Clamp(curZoom, zoomLimit.X, zoomLimit.Y);
            }
        }

        public static Vector2 ViewToWorld(Vector2 aPosition)
        {
            return Vector2.Transform(aPosition, Matrix.Invert(TranslationMatrix));
        }
    }
}
