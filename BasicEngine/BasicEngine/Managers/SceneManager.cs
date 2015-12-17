using System;
using OpenTK.Graphics.OpenGL;
using BasicEngine.Core;
using System.Drawing;

namespace BasicEngine.Managers
{
    class SceneManager : IListener
    {
        private ShaderManager shaderManager = null;
        private ModelsManager modelsManager = null;

        public SceneManager()
        {
            shaderManager = new ShaderManager();
            //TODO: FileService Class which handles the Files, not in ShaderManager, give source code to shader manager
            shaderManager.CreateProgram("test", @"C:\Projects\OpenGL\basic_engine_git\BasicEngine\BasicEngine\Shaders\vertex.glsl", @"C:\Projects\OpenGL\basic_engine_git\BasicEngine\BasicEngine\Shaders\fragment.glsl");

            modelsManager = new ModelsManager();
        }

        public void Dispose()
        {
            shaderManager.Dispose();
            modelsManager.Dispose();
        }

        public void NotifyUpdateFrame()
        {
            modelsManager.Update();
        }

        public void NotifyDisplayFrame()
        {
            //called when window should be redisplayed, here called by glutPostRedisplay in idleFunc
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); //clear buffer --> color and depth
            GL.ClearColor(Color.Black);

            //the renderin/gpu calls of our models
            modelsManager.Draw();
        }

        public void NotifyEndFrame()
        {
            //nothing to do here now
        }

        public void NotifyReshape()
        {
            //nothing to do here now
        }
    }
}
