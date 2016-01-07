using OpenTK;
using System;

namespace BasicEngine.Rendering
{
    class Camera
    {
        // TODO: no singleton, but for now for testing
        private static Camera instance = null;

        private float horizontalAngle = (float)Math.PI;
        private float verticalAngle = 0f;

        public static Camera Instance
        {
            get
            {
                if (instance == null) instance = new Camera();
                return instance;
            }
        }

        private Camera()
        {
            FOV = (float)Math.PI / 4f;
            Aspect = 4f / 3f;
            NearPlane = 0.1f;
            FarPlane = 100f;
            Position = new Vector3(0, 0, 5);
            Direction = Vector3.Zero;

            CalculateMatrices(0, 0, Vector3.Zero);
        }

        //Creating the ProjectionMatrix for transformation from camera to homogenous space
        private Matrix4 projectionMatrix = Matrix4.Identity;
        public Matrix4 ProjectionMatrix
        {
            get{ return projectionMatrix; }
        }

        //Create View Matrix for transformation from World to Camera Space
        private Matrix4 viewMatrix = Matrix4.Identity;
        public Matrix4 ViewMatrix
        {
            get{ return viewMatrix; }
        }

        public float FOV{ get; set; }

        public float Aspect{ get; set; }

        public float NearPlane{ get; set; }

        public float FarPlane{ get; set; }

        public Vector3 Position{ get; set; }

        public Vector3 Direction{ get; set; }

        public void CalculateMatrices(float horizTurn, float vertTurn, Vector3 move)
        {
            horizontalAngle += horizTurn;
            verticalAngle += vertTurn;
            // Direction : Spherical coordinates to Cartesian coordinates conversion
            Direction = new Vector3((float)Math.Cos(verticalAngle) * (float)Math.Sin(horizontalAngle), (float)Math.Sin(verticalAngle), (float)Math.Cos(verticalAngle) * (float)Math.Cos(horizontalAngle));
            // Right vector, on this vector we can move sidewards
            Vector3 right = new Vector3((float)Math.Sin(horizontalAngle-Math.PI/2f), 0, (float)Math.Cos(horizontalAngle - Math.PI / 2f));
            // Up vector : perpendicular to both direction and right
            Vector3 up = Vector3.Cross(right, Direction);

            Position += right*move.X + up*move.Y + Direction * move.Z;

            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, Aspect, NearPlane, FarPlane);
            viewMatrix = Matrix4.LookAt(Position, Position + Direction, up);
        }
    }
}
