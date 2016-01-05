using BasicEngine.Utility;
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

        int vertexArrayID;
        int vertexBuffer;
        int matrixID;
        Matrix4 MVP;

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

            //our triangle vertices in modelSpace
            Vector3[] g_vertex_buffer_data = new Vector3[]{
                new Vector3(-1.0f, -1.0f, 0.0f),
                new Vector3(1.0f, -1.0f, 0.0f),
                new Vector3(0.0f,  1.0f, 0.0f),
            };
            short[] g_element_buffer_data = new short[] { 0, 1, 2 };

            //gernerate 1 buffer, put resulting identifier in vertexBuffer
            GL.GenBuffers(1, out vertexBuffer);
            //The following command will talk about our 'vertexBuffer' buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            //Give the vertices to OpenGL
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(g_vertex_buffer_data.Length * Vector3.SizeInBytes), g_vertex_buffer_data, BufferUsageHint.StaticDraw);


            //BasicEngine.Managers.ShaderManager man = new BasicEngine.Managers.ShaderManager();
            //man.CreateProgram("test", @"..\..\Shaders\vertex.glsl", @"..\..\Shaders\fragment.glsl");
            //shaderProgram = BasicEngine.Managers.ShaderManager.GetShader("test");

            //attrVertexPos = GL.GetAttribLocation(shaderProgram, "vPosition");
            //attrVertexNorm = GL.GetAttribLocation(shaderProgram, "nPosition");
            //uniformMView = GL.GetUniformLocation(shaderProgram, "modelview");

            //if (attrVertexPos == -1 || uniformMView == -1 || attrVertexNorm == -1)
            //{
            //    Console.WriteLine("UAUAUAUA: Error binding attributes");
            //}

            //GL.GenBuffers(1, out vbo_position);
            //GL.GenBuffers(1, out vbo_Norm);
            //GL.GenBuffers(1, out vbo_mview);

            //mviewdata = new Matrix4[]{
            //    Matrix4.Identity
            //};

            //vecPosDat = new Vector3[vertices.Length];
            //for (int i = 0; i < vertices.Length; i++)
            //{
            //    vecPosDat[i] = new Vector3(vertices[i].Vertex.X / 80f, vertices[i].Vertex.Y / 80f, 0.0f);
            //}

            //nPosDat = new Vector3[vertices.Length];
            //for (int i = 0; i < vertices.Length; i++) nPosDat[i] = vertices[i].Normal;
        }

        public void Prepare()
        {



            //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            //GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vecPosDat.Length * Vector3.SizeInBytes), vecPosDat, BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(attrVertexPos, 3, VertexAttribPointerType.Float, false, 0, 0);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_Norm);
            //GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(nPosDat.Length * Vector3.SizeInBytes), nPosDat, BufferUsageHint.StaticDraw);
            //GL.VertexAttribPointer(attrVertexNorm, 3, VertexAttribPointerType.Float, true, 0, 0);


            //GL.UniformMatrix4(uniformMView, false, ref mviewdata[0]);

            //GL.UseProgram(shaderProgram);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Render()
        {
            //Clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Use the shader
            GL.UseProgram(shaderProgram);

            //send the modelViewMatrix to our Shader as uniform
            Matrix4 m = Matrix4.Identity;
            GL.UniformMatrix4(matrixID, false, ref MVP);

            //1st attribute buffer: vertices
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.VertexAttribPointer(
                0,                              //attribute 0. No particular reason for 0, but must match the layout in the shader
                3,                              //size
                VertexAttribPointerType.Float,  //type
                false,                          //not normalized
                0,                              //stride
                0                               //array buffer offset
            );

            //Draw the triangle
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);   //starting from vertex0; 3 vertices total -> 1 triangle
            GL.DisableVertexAttribArray(0);


            //Prepare();

            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.Enable(EnableCap.DepthTest);


            //GL.EnableVertexAttribArray(attrVertexPos);
            //GL.EnableVertexAttribArray(attrVertexNorm);

            //GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vecPosDat.Length);

            //GL.DisableVertexAttribArray(attrVertexPos);
            //GL.EnableVertexAttribArray(attrVertexNorm);


            //GL.Flush();
        }
    }
}
