using System;
using BasicEngine.Input;
using OpenTK.Input;
using System.Collections.Generic;
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

        public void Draw()
        {
            // ovverride in subclass
            mesh.Render();
        }

        public void Update()
        {
            List<Key> pressedKeys = Control.GetAllPressedKeys();
            float moveSpeed = 0.015f;
            // ovverride in subclass
            if(pressedKeys.Count>0)
            {
                float horizAngle = 0f, vertAngle = 0f;
                Vector3 move = new Vector3();
                if (pressedKeys.Contains(Key.Left)) horizAngle += moveSpeed;
                if (pressedKeys.Contains(Key.Right)) horizAngle -= moveSpeed;
                if (pressedKeys.Contains(Key.Up)) vertAngle += moveSpeed;
                if (pressedKeys.Contains(Key.Down)) vertAngle -= moveSpeed;
                if (pressedKeys.Contains(Key.A)) move.X -= moveSpeed*4f;
                if (pressedKeys.Contains(Key.D)) move.X += moveSpeed*4f;
                if (pressedKeys.Contains(Key.W)) move.Y += moveSpeed*4f;
                if (pressedKeys.Contains(Key.S)) move.Y -= moveSpeed*4f;
                if (pressedKeys.Contains(Key.Q)) move.Z += moveSpeed*4f;
                if (pressedKeys.Contains(Key.E)) move.Z -= moveSpeed*4f;

                Camera.Instance.CalculateMatrices(horizAngle, vertAngle, move);
            }
        }

        public void SetProgram(int program)
        {
            this.program = program;
        }
    }
}
