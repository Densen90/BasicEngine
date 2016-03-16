using System;
using BasicEngine.Rendering;
using OpenTK.Input;
using BasicEngine.Input;
using System.Collections.Generic;
using OpenTK;
using BasicEngine.Utility;

namespace BasicEngine.Object
{
    class Model : IGameObject, IDisposable
    {
        public Transform Transform { get; set; }
        public Mesh Mesh{ get; set; }

        public int InstanceID { get; private set; }

        public Model()
        {
            Init("cube.obj");
        }

        public Model(string modelFile)
        {
            Init(modelFile);
        }

        private void Init(string modelFile)
        {
            this.Mesh = new Mesh(@"..\..\ModelFiles\", modelFile);
            this.Transform = new Transform(Mesh.Vertices);

            InstanceID = this.GetHashCode();
            Console.WriteLine("Position: " + this.Transform.Position + ", InstanceID: " + InstanceID);

            EventDispatcher.AddListener("Render", Draw);
        }

        public void Dispose()
        {

        }

        public void Draw()
        {
            Mesh.Render();
        }

        public void Update()
        {
            //List<Key> pressedKeys = Control.GetAllPressedKeys();
            //float moveSpeed = 0.015f;

            //if (pressedKeys.Count > 0)
            //{
            //    float horizAngle = 0f, vertAngle = 0f;
            //    Vector3 move = new Vector3();
            //    if (pressedKeys.Contains(Key.Left)) horizAngle += moveSpeed;
            //    if (pressedKeys.Contains(Key.Right)) horizAngle -= moveSpeed;
            //    if (pressedKeys.Contains(Key.Up)) vertAngle += moveSpeed;
            //    if (pressedKeys.Contains(Key.Down)) vertAngle -= moveSpeed;
            //    if (pressedKeys.Contains(Key.A)) move.X -= moveSpeed * 4f;
            //    if (pressedKeys.Contains(Key.D)) move.X += moveSpeed * 4f;
            //    if (pressedKeys.Contains(Key.W)) move.Y += moveSpeed * 4f;
            //    if (pressedKeys.Contains(Key.S)) move.Y -= moveSpeed * 4f;
            //    if (pressedKeys.Contains(Key.Q)) move.Z += moveSpeed * 4f;
            //    if (pressedKeys.Contains(Key.E)) move.Z -= moveSpeed * 4f;

            //    Camera.Instance.CalculateMatrices(horizAngle, vertAngle, move);
            //}

            //Light.Instance.Transform.Position += new Vector3(0,0,-1)*Time.DeltaTime;
        }
    }
}
