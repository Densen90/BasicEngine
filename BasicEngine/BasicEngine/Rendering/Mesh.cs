using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace BasicEngine.Rendering
{
    class Mesh
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct VertexObj
        {
            public Vector2 TexCoord;
            public Vector3 Normal;
            public Vector3 Vertex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Triangle
        {
            public int Index0;
            public int Index1;
            public int Index2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Quad
        {
            public int Index0;
            public int Index1;
            public int Index2;
            public int Index3;
        }

        private VertexObj[] vertices;
        public VertexObj[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        private Triangle[] triangles;
        public Triangle[] Triangles
        {
            get { return triangles; }
            set { triangles = value; }
        }

        private Quad[] quads;
        public Quad[] Quads
        {
            get { return quads; }
            set { quads = value; }
        }

        private Vector3[] vecPosDat;
        private Vector3[] nPosDat;
        Matrix4[] mviewdata;

        int verticesBufferId;
        int trianglesBufferId;
        int quadsBufferId;

        int attrVertexPos, attrVertexNorm, uniformMView;
        int vbo_position, vbo_Norm, vbo_mview;
        int shaderProgram;

        public Mesh(string fileName)
        {
            //TODO: FileService
            //if (MeshLoader.Load(this, fileName))
            //{
            //    Console.WriteLine("Mesh.Mesh: Created Mesh with: ");
            //    Console.WriteLine("\t" + vertices.Length + " Vertices");
            //    Console.WriteLine("\t" + triangles.Length + " Triangles");
            //    Console.WriteLine("\t" + quads.Length + " Quads");
            //}
            //else
            //{
            //    Console.WriteLine("Mesh.Mesh: Model file " + fileName + " does not exist");
            //}

            Init();

        }

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

            //our cube, 36 vertices for 12 triangles in modelSpace
            g_vertex_buffer_data = new Vector3[]{
                new Vector3(-1.0f,-1.0f,-1.0f),
                new Vector3(-1.0f,-1.0f, 1.0f),
                new Vector3(-1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f,-1.0f),
                new Vector3(-1.0f,-1.0f,-1.0f),
                new Vector3(-1.0f, 1.0f,-1.0f),
                new Vector3(1.0f,-1.0f, 1.0f),
                new Vector3(-1.0f,-1.0f,-1.0f),
                new Vector3(1.0f,-1.0f,-1.0f),
                new Vector3(1.0f, 1.0f,-1.0f),
                new Vector3(1.0f,-1.0f,-1.0f),
                new Vector3(-1.0f,-1.0f,-1.0f),
                new Vector3(-1.0f,-1.0f,-1.0f),
                new Vector3(-1.0f, 1.0f, 1.0f),
                new Vector3(-1.0f, 1.0f,-1.0f),
                new Vector3(1.0f,-1.0f, 1.0f),
                new Vector3(-1.0f,-1.0f, 1.0f),
                new Vector3(-1.0f,-1.0f,-1.0f),
                new Vector3(-1.0f, 1.0f, 1.0f),
                new Vector3(-1.0f,-1.0f, 1.0f),
                new Vector3(1.0f,-1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(1.0f,-1.0f,-1.0f),
                new Vector3(1.0f, 1.0f,-1.0f),
                new Vector3(1.0f,-1.0f,-1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(1.0f,-1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f,-1.0f),
                new Vector3(-1.0f, 1.0f,-1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(-1.0f, 1.0f,-1.0f),
                new Vector3(-1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(-1.0f, 1.0f, 1.0f),
                new Vector3(1.0f,-1.0f, 1.0f),
            };

            //color for each vertex of the cube
            Vector3[] g_color_buffer_data = new Vector3[]{
                new Vector3(0.583f,  0.771f,  0.014f),
                new Vector3(0.609f,  0.115f,  0.436f),
                new Vector3(0.327f,  0.483f,  0.844f),
                new Vector3(0.822f,  0.569f,  0.201f),
                new Vector3(0.435f,  0.602f,  0.223f),
                new Vector3(0.310f,  0.747f,  0.185f),
                new Vector3(0.597f,  0.770f,  0.761f),
                new Vector3(0.559f,  0.436f,  0.730f),
                new Vector3(0.359f,  0.583f,  0.152f),
                new Vector3(0.483f,  0.596f,  0.789f),
                new Vector3(0.559f,  0.861f,  0.639f),
                new Vector3(0.195f,  0.548f,  0.859f),
                new Vector3(0.014f,  0.184f,  0.576f),
                new Vector3(0.771f,  0.328f,  0.970f),
                new Vector3(0.406f,  0.615f,  0.116f),
                new Vector3(0.676f,  0.977f,  0.133f),
                new Vector3(0.971f,  0.572f,  0.833f),
                new Vector3(0.140f,  0.616f,  0.489f),
                new Vector3(0.997f,  0.513f,  0.064f),
                new Vector3(0.945f,  0.719f,  0.592f),
                new Vector3(0.543f,  0.021f,  0.978f),
                new Vector3(0.279f,  0.317f,  0.505f),
                new Vector3(0.167f,  0.620f,  0.077f),
                new Vector3(0.347f,  0.857f,  0.137f),
                new Vector3(0.055f,  0.953f,  0.042f),
                new Vector3(0.714f,  0.505f,  0.345f),
                new Vector3(0.783f,  0.290f,  0.734f),
                new Vector3(0.722f,  0.645f,  0.174f),
                new Vector3(0.302f,  0.455f,  0.848f),
                new Vector3(0.225f,  0.587f,  0.040f),
                new Vector3(0.517f,  0.713f,  0.338f),
                new Vector3(0.053f,  0.959f,  0.120f),
                new Vector3(0.393f,  0.621f,  0.362f),
                new Vector3(0.673f,  0.211f,  0.457f),
                new Vector3(0.820f,  0.883f,  0.371f),
                new Vector3(0.982f,  0.099f,  0.879f),
            };

            //generate 1 buffer, put resulting identifier in vertexBuffer
            GL.GenBuffers(1, out vertexBuffer);
            //The following command will talk about our 'vertexBuffer' buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            //Give the vertices to OpenGL
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(g_vertex_buffer_data.Length * Vector3.SizeInBytes), g_vertex_buffer_data, BufferUsageHint.StaticDraw);

            //do the same for the color data
            GL.GenBuffers(1, out colorBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(g_color_buffer_data.Length * Vector3.SizeInBytes), g_color_buffer_data, BufferUsageHint.StaticDraw);
        }

        public void Prepare()
        {

        }

        public void Render()
        {
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
            GL.DrawArrays(PrimitiveType.Triangles, 0, g_vertex_buffer_data.Length);   //starting from vertex0; 3 vertices total -> 1 triangle
            GL.DisableVertexAttribArray(0);

        }
    }
}
