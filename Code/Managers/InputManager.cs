using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Managers
{
    class InputManager
    {
        private static Vector2 mouseDelta = Vector2.Zero;
        private static Vector2 lastMousePos = Vector2.Zero;

        public static void Initialise(GameWindow currentGameWindow)
        {
            // hide the cursor
            currentGameWindow.CursorVisible = false;

            // compute mouse delta movement
            currentGameWindow.UpdateFrame += (object sender, FrameEventArgs f) => 
            {
                Vector2 coords = new Vector2(currentGameWindow.Mouse.GetState().X, currentGameWindow.Mouse.GetState().Y);
                mouseDelta = lastMousePos - coords;
                lastMousePos = coords;
                //Rectangle bounds = currentGameWindow.Bounds;
                //OpenTK.Input.Mouse.SetPosition(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
            };
        }

        public static Vector2 GetMouseDelta()
        {
            return mouseDelta;
        }

        public static bool GetKeyDown(Key key)
        {
            return OpenTK.Input.Keyboard.GetState().IsKeyDown(key);
        }

        public static bool GetKeyUp(Key key)
        {
            return OpenTK.Input.Keyboard.GetState().IsKeyUp(key);
        }
        public static bool MousestateDown(MouseButton button)
        {
            return OpenTK.Input.Mouse.GetState().IsButtonDown(button);
        }
        public static bool MousestateUp(MouseButton button)
        {
            return OpenTK.Input.Mouse.GetState().IsButtonUp(button);
        }
    }
}
