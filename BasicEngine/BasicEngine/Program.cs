using BasicEngine.Managers;
using BasicEngine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;
using System.Drawing;
using OpenTK.Input;

namespace StarterKit
{
    class Application
    {
        private GameWindow gameWindow;
        private ModelsManager modelsManager;

        [STAThread]
        static void Main()
        {
            Application app = new Application();
        }

        public Application()
        {
            gameWindow = new GameWindow(1024, 768);

            //TODO: Lookat Visual from Scherzer
            gameWindow.Load += LoadEvent;
            gameWindow.Resize += ResizeEvent;
            gameWindow.UpdateFrame += UpdateEvent;
            gameWindow.RenderFrame += RenderEvent;
            gameWindow.Disposed += DisposeEvent;
            gameWindow.MouseMove += MouseMoveEvent;
            gameWindow.KeyDown += KeyDownEvent;

            modelsManager = new ModelsManager();

            gameWindow.Run(60.0);
        }

        private void RenderEvent(object sender, FrameEventArgs e)
        {
            //called when window should be redisplayed, here called by glutPostRedisplay in idleFunc
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //clear buffer --> color and depth
            GL.ClearColor(Color.CornflowerBlue);

            //the renderin/gpu calls of our models
            modelsManager.Draw();

            gameWindow.SwapBuffers();
        }

        private void UpdateEvent(object sender, FrameEventArgs e)
        {
            modelsManager.Update();
        }

        private void KeyDownEvent(object sender, KeyboardKeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape: gameWindow.Close(); break;
            }
        }

        private void MouseMoveEvent(object sender, MouseMoveEventArgs e)
        {
            Cursor.Position = new Point(1920 / 2, 1080 / 2);
            Console.WriteLine("X: " + e.XDelta + ", Y: " + e.YDelta);

            Camera.Instance.CalculateMatrices((0.005f * e.XDelta), (0.005f * e.YDelta));
        }


        private void DisposeEvent(object sender, EventArgs e)
        {
            modelsManager.Dispose();
        }

        private void LoadEvent(object sender, EventArgs e)
        {
            gameWindow.VSync = VSyncMode.On;
        }

        private void ResizeEvent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
