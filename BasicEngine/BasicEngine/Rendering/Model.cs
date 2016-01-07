using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace BasicEngine.Rendering
{
    class Model : IGameObject, IDisposable
    {
        protected int program;
        private Mesh mesh;

        public Model(Mesh mesh)
        {
            this.mesh = mesh;
        }

        public void Dispose()
        {

        }

        public virtual void Draw()
        {
            // ovverride in subclass
            mesh.Render();
        }

        public virtual void Update()
        {
            // ovverride in subclass
        }

        public void SetProgram(int program)
        {
            this.program = program;
        }
    }
}
