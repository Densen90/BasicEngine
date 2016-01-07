using OpenTK;
using System;

namespace BasicEngine.Rendering
{
    class Camera
    {
        // TODO: no singleton, but for now for testing
        private static Camera instance = null;

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
            Position = new Vector3(4, 2, 2);
            Direction = Vector3.Zero;
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

        public void CalculateMatrices(float horizontalAngle, float verticalAngle)
        {
            // Direction : Spherical coordinates to Cartesian coordinates conversion
            Vector3 direction = new Vector3((float)Math.Cos(verticalAngle) * (float)Math.Sin(horizontalAngle), (float)Math.Sin(verticalAngle), (float)Math.Cos(verticalAngle) * (float)Math.Cos(horizontalAngle));
            // Right vector
            Vector3 right = new Vector3((float)Math.Sin(horizontalAngle-Math.PI/2f), 0, (float)Math.Cos(horizontalAngle - Math.PI / 2f));
            // Up vector : perpendicular to both direction and right
            Vector3 up = Vector3.Cross(right, direction);

            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, Aspect, NearPlane, FarPlane);
            viewMatrix = Matrix4.LookAt(Position, Position+direction, up);
        }
    }
}
