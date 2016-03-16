using BasicEngine.Managers;
using OpenTK;
using System;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace BasicEngine
{
    public class FrameworkException : Exception
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public FrameworkException(string type, string msg)
        {
            Type = type;
            Message = msg;
        }
    }

    public class Framework
    {
        private static Framework instance = null;
        public static Framework Instance
        {
            get { if (instance == null) instance = new Framework(); return instance; }
        }

        private GameWindow gameWindow;
        public GameWindow GameWindow
        {
            get { return gameWindow; }
        }

        private ModelsManager modelsManager;
        public ModelsManager ModelsManager
        {
            get { return modelsManager; }
        }

        private Color backgroundColor = Color.LightGreen;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public VSyncMode VSync
        {
            get { return gameWindow.VSync; }
            set { gameWindow.VSync = value; }
        }

        [STAThread]
        static void Main()
        {
            //Framework f = new Framework();
        }

        public Framework()
        {
            if (instance != null) throw new FrameworkException("Create Error", "There is already an instance of the framework running!");

            instance = this;
            gameWindow = new GameWindow(1024, 768);

            gameWindow.Load += LoadEvent;
            gameWindow.Resize += ResizeEvent;
            gameWindow.UpdateFrame += UpdateEvent;
            gameWindow.RenderFrame += RenderEvent;
            gameWindow.Disposed += DisposeEvent;
            gameWindow.KeyDown += KeyDownEvent;

            gameWindow.UpdateFrame += Utility.Time.UpdateEvent;

            gameWindow.KeyDown += Input.Control.KeyDownEvent;
            gameWindow.KeyUp += Input.Control.KeyUpEvent;

            modelsManager = new ModelsManager();

            gameWindow.Run(60.0);
        }

        private void LoadEvent(object sender, EventArgs e)
        {
            gameWindow.VSync = VSyncMode.On;
        }

        private void DisposeEvent(object sender, EventArgs e)
        {
            modelsManager.Dispose();
        }

        private void RenderEvent(object sender, FrameEventArgs e)
        {
            //called when window should be redisplayed, here called by glutPostRedisplay in idleFunc
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //clear buffer --> color and depth
            GL.ClearColor(backgroundColor);
            GL.Enable(EnableCap.DepthTest);

            //the renderin/gpu calls of our models
            modelsManager.Draw();

            gameWindow.SwapBuffers();
        }

        private void UpdateEvent(object sender, FrameEventArgs e)
        {
            modelsManager.Update();
            Input.Control.ResetKeyPress();  //reset keydown and keyup every frame, to recognize it only once
        }

        private void ResizeEvent(object sender, EventArgs e)
        {
            //Do something here
        }

        private void KeyDownEvent(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape: gameWindow.Close(); break;
            }
        }
    }
}
