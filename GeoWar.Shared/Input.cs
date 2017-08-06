using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace GeoWar
{
    static class Input
    {
        // track the current and previous state of the inputs
        // previous is needed to know when buttons were pressed
        private static KeyboardState keyboardState;
        private static KeyboardState lastKeyboardState;
        private static GamePadState gamepadState;
        private static GamePadState lastGamepadState;
        private static MouseState mouseState;
        private static MouseState lastMouseState;

        // track whether or not the user is using the mouse or not
        // since we want the mouse icon to disappear when its not being used
        // not moved since the last update
        private static bool isAimingWithMouse = false;
        public static bool isAimingWithKeyboard = false;
        private static bool isAimingWithGamepad = false;

        // these are the keyboard controls for aiming, we need this list to track whether
        // or not the user is using them to aim or not
        private static Keys[] keyboardAimKeys = new Keys[4] {Keys.Up, Keys.Down, Keys.Left, Keys.Right};

        /// <summary>
        /// property for getting the current mouse position
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(mouseState.Position.X, mouseState.Position.Y);
            }
        }

        /// <summary>
        /// function to run on each update cycle in GameRoot to retrieve the input from 
        /// the user and move the current state info to the laststate property 
        /// finally we also decide whether or not the user is using the mouse or not
        /// so that we know whether or not to display the mouse icon
        /// </summary>
        public static void Update()
        {
            lastKeyboardState = keyboardState;
            lastGamepadState = gamepadState;
            lastMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
            mouseState = Mouse.GetState();

            // if the right thumbstick has a value other than zero then it means
            // the player is pushing it. So set IsAimingWithGamepad to true
            if (gamepadState.ThumbSticks.Right == Vector2.Zero)
            {
                isAimingWithGamepad = false;
            }
            else
            {
                isAimingWithGamepad = true;
            }

            // if any of the keyboardAimKeys are down set isAimingWithKeyboard to true
            if (keyboardAimKeys.Any(key => keyboardState.IsKeyDown(key)))
            {
                isAimingWithKeyboard = true;
            }
            else
            {
                isAimingWithKeyboard = false;
            }

            // if player has touched any of the keyboard or gamepad aim controls then
            // he is not aiming with the mouse to set aiming with mouse to false
            if (isAimingWithKeyboard == true || isAimingWithGamepad == true)
            {
                isAimingWithMouse = false;
            }
            // if the player hasn't touched the keyboard or gamepad aim controls
            // and the mouse has moved since the last update then he must be using the 
            // the mouse to aim to set isAimingWithMouse to true
            else if (mouseState.Position != lastMouseState.Position)
            {
                isAimingWithMouse = true;
            }
        }
    }
}
