using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace BasicEngine.Rendering
{
    class Model : IGameObject, IDisposable
    {
        protected Matrix4[] modelViewDataMatrix = new Matrix4[] { Matrix4.Identity };
        private int program;
        private int vertexArrayObject;
        private List<int> vertexBufferObjects;

        public void Dispose()
        {
            GL.DeleteVertexArray(vertexArrayObject);
            GL.DeleteBuffers(vertexBufferObjects.Count, vertexBufferObjects.ToArray());
            vertexBufferObjects.Clear();
        }

        public virtual void Draw()
        {
            // ovverride in subclass
        }

        public virtual void Update()
        {
            // ovverride in subclass
        }

        public int GetVertexArrayObject()
        {
            return vertexArrayObject;
        }

        public List<int> GetVertexbufferObjects()
        {
            return vertexBufferObjects;
        }

        public void SetProgram(int program)
        {
            this.program = program;
        }
    }
}
