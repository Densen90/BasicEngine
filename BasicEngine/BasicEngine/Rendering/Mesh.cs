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

        private Vector3[] indexedVertices;
        private Vector2[] indexedUVs;
        private Vector3[] indexedNormals;
        private int[] indices;

        //TODO: add Getter for Vertices, UVs, Normals to set in MeshLoader

        int vertexArrayID;  //VAO
        int vertexBuffer;   //VBO
        int normalBuffer;
        int elementBuffer;  //for indexing
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

            //sort our indices
            IndexVBO.Index(Vertices, UVs, Normals, out indices, out indexedVertices, out indexedUVs, out indexedNormals);

            //generate 1 buffer, put resulting identifier in vertexBuffer
            GL.GenBuffers(1, out vertexBuffer);
            //The following command will talk about our 'vertexBuffer' buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            //Give the vertices to OpenGL
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(indexedVertices.Length * Vector3.SizeInBytes), indexedVertices, BufferUsageHint.StaticDraw);

            //same for vertex Normals
            GL.GenBuffers(1, out normalBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(indexedNormals.Length * Vector3.SizeInBytes), indexedNormals, BufferUsageHint.StaticDraw);

            //and one buffer for the indexes
            GL.GenBuffers(1, out elementBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);
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

            //2nd attribute buffer: normal
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
            GL.VertexAttribPointer(
                1,                      //layout(location = 1)
                3,
                VertexAttribPointerType.Float,
                true,
                0,
                0
            );

            //Index Buffer --> ElementArray
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);

            //Draw the triangle
            //GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Length);   //starting from vertex0; 3 vertices total -> 1 triangle
            GL.DrawElements(BeginMode.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);

        }
    }
}
