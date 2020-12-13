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

        public static bool KeyPressed(Keys aKey)
        {
            return currentKeyState.IsKeyDown(aKey) && previousKeyState.IsKeyUp(aKey);
        }
        public static bool KeyHold(Keys aKey)
        {
            return currentKeyState.IsKeyDown(aKey);
        }
        public static KeyboardState CurrentKeyState => currentKeyState;
        public static KeyboardState PreviousKeyState => previousKeyState;

        public static bool MiddleMouseClick()
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
        }
        public static bool LeftClick()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }
        public static bool RightClick()
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }

        public static bool MiddleMouseHold()
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed;
        }
        public static bool LeftHold()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool RightHold()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool ScrollUp()
        {
            return currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue;
        }
        public static bool ScrollDown()
        {
            return currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue;
        }

        public static Point MousePos => currentMouseState.Position;
        public static MouseState PreviousMouseState => previousMouseState;

        //Should be called at beginning of Update in Game
        public static void Update()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
    }
}