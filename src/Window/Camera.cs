using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utilities;

namespace Window
{
    class Camera
    {
        private Vector2
            position,
            viewportSize,
            zoomLimit;
        private Point dragPos;
        private float
            speed,     // speed when using keyboard
            rotation,  // z-rotation of camera
            zoom,      // current zoom of camera
            deltaZoom; // How much zoom changes per scroll

        public Matrix WorldMatrixNoRotation
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateScale(zoom, zoom, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public Matrix WorldMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-position, 0)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(zoom, zoom, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-ViewportCenter, 0)) *
                    Matrix.CreateScale(1.0f / zoom, 1.0f / zoom, 1.0f) *
                    Matrix.Transpose(Matrix.CreateRotationZ(rotation)) *
                    Matrix.CreateTranslation(new Vector3(position, 0));
            }
        }

        public Vector2 TopLeftCorner  => position - ViewportCenter * (1 / zoom);
        public Vector2 ViewportCenter => new Vector2(viewportSize.X / 2, viewportSize.Y / 2);

        public float Zoom => zoom;

        public Camera(GameWindow window, Vector2 zoomLimit, float speed, float rotation, float deltaZoom)
        {
            this.zoomLimit = zoomLimit;
            this.speed = speed;
            this.rotation = rotation;
            this.deltaZoom = deltaZoom;

            viewportSize = window.ClientBounds.Size.ToVector2();
            position = ViewportCenter;

            dragPos = Point.Zero;

            zoom = 1.0f;
        }

        public void Reset()
        {
            position = ViewportCenter;
            dragPos = Point.Zero;
            zoom = 1.0f;
        }

        public void Update(GameTime gameTime)
        {
            MouseMovement();
            KeyboardMovement(gameTime);
        }

        private void MouseMovement()
        {
            // -- Camera Drag --

            if (KeyMouseReader.MiddleMouseClick())
            {
                dragPos = ViewToWorld(KeyMouseReader.MousePos.ToVector2()).ToPoint();
            }
            if (KeyMouseReader.MiddleMouseHold() && dragPos != Point.Zero)
            {
                Point deltaDragPos = ViewToWorld(KeyMouseReader.MousePos.ToVector2()).ToPoint();
                position += (dragPos - deltaDragPos).ToVector2();
            }

            // -- Camera Zoom --

            if (KeyMouseReader.ScrollUp())
            {
                zoom += deltaZoom;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
            if (KeyMouseReader.ScrollDown())
            {
                zoom -= deltaZoom;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
        }
        private void KeyboardMovement(GameTime gameTime)
        {
            if (KeyMouseReader.KeyHold(Keys.Up))
            {
                position.Y -= speed * (1 / zoom) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Down))
            {
                position.Y += speed * (1 / zoom) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Left))
            {
                position.X -= speed * (1 / zoom) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (KeyMouseReader.KeyHold(Keys.Right))
            {
                position.X += speed * (1 / zoom) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (KeyMouseReader.KeyHold(Keys.OemPlus))
            {
                zoom += deltaZoom / 2;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
            if (KeyMouseReader.KeyHold(Keys.OemMinus))
            {
                zoom -= deltaZoom / 2;
                zoom = MathHelper.Clamp(zoom, zoomLimit.X, zoomLimit.Y);
            }
        }

        public Vector2 ViewToWorld(Vector2 vector)
        {
            return Vector2.Transform(vector, ViewMatrix);
        }
        public Point ViewToWorld(Point point)
        {
            return Vector2.Transform(point.ToVector2(), ViewMatrix).ToPoint();
        }
    }
}
