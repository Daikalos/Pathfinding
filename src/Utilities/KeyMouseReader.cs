using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utilities
{
    static class KeyMouseReader
    {
        private static KeyboardState
            currentKeyState,
            previousKeyState = Keyboard.GetState();
        private static MouseState
            currentMouseState,
            previousMouseState = Mouse.GetState();

        public static KeyboardState CurrentKeyState => currentKeyState;
        public static KeyboardState PreviousKeyState => previousKeyState;

        public static Point MousePos => currentMouseState.Position;
        public static MouseState PreviousMouseState => previousMouseState;

        public static bool KeyPressed(Keys aKey) 
            => currentKeyState.IsKeyDown(aKey) && previousKeyState.IsKeyUp(aKey);
        public static bool KeyHold(Keys aKey)
            => currentKeyState.IsKeyDown(aKey);

        public static bool MiddleMouseClick()
            => currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
        public static bool LeftClick()
            => currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        public static bool RightClick()
            => currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;

        public static bool MiddleMouseHold()
            => currentMouseState.MiddleButton == ButtonState.Pressed;
        public static bool LeftHold()
            => currentMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightHold()
            => currentMouseState.RightButton == ButtonState.Pressed;

        public static bool ScrollUp()
            => currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue;
        public static bool ScrollDown()
            => currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue;

        // Should be called at beginning of Update in Game
        public static void Update()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
    }
}