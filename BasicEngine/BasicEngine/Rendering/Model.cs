using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace BasicEngine.Rendering
{
    class Model : IGameObject, IDisposable
    {
        protected Matrix4[] modelViewDataMatrix = new Matrix4[] { Matrix4.Identity };
        protected int program;
        private Mesh mesh;

        public Model(Mesh mesh)
        {
            this.mesh = mesh;
            mesh.Prepare();
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
