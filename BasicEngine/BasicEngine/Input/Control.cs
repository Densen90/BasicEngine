using OpenTK.Input;
using System.Collections.Generic;

namespace BasicEngine.Input
{
    static class Control
    {
        private static Key? keyDown = null;
        private static Key? keyUp = null;
        private static Key? keyPressed = null;
        private static Key? lastKeyPressed = null;

        private static List<Key> allPressedKeys = new List<Key>();

        public static Key? GetKeyDown()
        {
            return keyDown;
        }

        public static Key? GetKeyUp()
        {
            return keyUp;
        }

        public static Key? GetKeyPress()
        {
            return keyPressed;
        }

        public static List<Key> GetAllPressedKeys()
        {
            return allPressedKeys;
        }

        public static void KeyDownEvent(object sender, KeyboardKeyEventArgs e)
        {
            if(e.Key != lastKeyPressed)
            {
                keyDown = e.Key;
                keyPressed = e.Key;
                if(!allPressedKeys.Contains(e.Key)) allPressedKeys.Add(e.Key);
                lastKeyPressed = e.Key;
            }
        }

        public static void KeyUpEvent(object sender, KeyboardKeyEventArgs e)
        {
            keyUp = e.Key;
            keyDown = null;
            lastKeyPressed = null;
            keyPressed = null;

            if (allPressedKeys.Contains(e.Key)) allPressedKeys.Remove(e.Key);
        }

        public static void ResetKeyPress()
        {
            //reset keys every frame, to recognize only once when pressed
            keyDown = null;
            keyUp = null;
        }
    }
}
