using BasicEngine.Utility;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace BasicEngine.Rendering
{
    class Mesh
    {
        int shaderProgram;

        public Mesh(string fileName)
        {
            //TODO: FileService
            if (MeshLoader.Load(this, fileName))
            {
                Console.WriteLine("Mesh.Mesh: Created Mesh with: " + Vertices.Length + " Vertices");
            }
            else
            {
                Console.WriteLine("Mesh.Mesh: Model file " + fileName + " does not exist");
            }

            Init();

        }

        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }
        public Vector2[] UVs { get; set; }

        //TODO: add Getter for Vertices, UVs, Normals to set in MeshLoader

        int vertexArrayID;  //VAO
        int vertexBuffer;   //VBO
        int colorBuffer;    //VBO
        int matrixID;
        Matrix4 MVP;
        Vector3[] g_vertex_buffer_data;

        public void Init()
        {
            GL.GenVertexArrays(1, out vertexArrayID);
            GL.BindVertexArray(vertexArrayID);

            //TODO: making static class?
            BasicEngine.Managers.ShaderManager man = new BasicEngine.Managers.ShaderManager();
            man.CreateProgram("test2", @"..\..\Shaders\vertex2.glsl", @"..\..\Shaders\fragment2.glsl");
            shaderProgram = BasicEngine.Managers.ShaderManager.GetShader("test2");

            //Creating our MVP Matrix --> TODO: Camera Class?
            matrixID = GL.GetUniformLocation(shaderProgram, "mvpMatrix");   //Get the Handle for our uniform in our shader
            //Creating the ProjectionMatrix for transformation from camera to homogenous space
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI/4f, 4f/3f, 0.1f, 10.0f);
            //Create View Matrix for transformation from World to Camera Space
            Matrix4 view = Matrix4.LookAt(
                                    new Vector3(4, 2, 2), //Camera is at (4,3,3), in World Space
                                    new Vector3(0, 0, 0), //looks at the origin
                                    Vector3.UnitY);//Up Vector
            //Model matrix : an identity matrix (model will be at the origin)
            Matrix4 model = Matrix4.Identity;   //TODO: here TranslationMatrix*RotationMatrix*ScaleMatrix -> Identity no transformation
            MVP = model * view * projection;

            //generate 1 buffer, put resulting identifier in vertexBuffer
            GL.GenBuffers(1, out vertexBuffer);
            //The following command will talk about our 'vertexBuffer' buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            //Give the vertices to OpenGL
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * Vector3.SizeInBytes), Vertices, BufferUsageHint.StaticDraw);

            //do the same for the color data, give vertex positions as color data for testing
            GL.GenBuffers(1, out colorBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Vertices.Length * Vector3.SizeInBytes), Vertices, BufferUsageHint.StaticDraw);
        }

        public void Prepare()
        {

        }

        public void Render()
        {
            //Calculate MVP
            MVP = Matrix4.Identity * Camera.Instance.ViewMatrix * Camera.Instance.ProjectionMatrix;

            //Use the shader
            GL.UseProgram(shaderProgram);

            //send the modelViewMatrix to our Shader as uniform
            Matrix4 m = Matrix4.Identity;
            GL.UniformMatrix4(matrixID, false, ref MVP);

            //1st attribute buffer: vertices
            GL.EnableVertexAttribArray(0);  //attribute 0
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.VertexAttribPointer(
                0,                              //attribute 0. No particular reason for 0, but must match the layout in the shader --> layout (location=0)
                3,                              //size
                VertexAttribPointerType.Float,  //type
                false,                          //not normalized
                0,                              //stride
                0                               //array buffer offset
            );

            //2nd attribute buffer: color
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
            GL.VertexAttribPointer(
                1,                      //layout(location = 1)
                3,
                VertexAttribPointerType.Float,
                false,
                0,
                0
            );

            //Draw the triangle
            GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Length);   //starting from vertex0; 3 vertices total -> 1 triangle
            GL.DisableVertexAttribArray(0);

        }
    }
}
