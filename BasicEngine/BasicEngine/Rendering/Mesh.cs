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
            if (MeshLoader.Load(this, fileName))
            {
                Console.WriteLine("Mesh.Mesh: Created Mesh with: ");
                Console.WriteLine("\t" + vertices.Length + " Vertices");
                Console.WriteLine("\t" + triangles.Length + " Triangles");
                Console.WriteLine("\t" + quads.Length + " Quads");
            }
            else
            {
                Console.WriteLine("Mesh.Mesh: Model file " + fileName + " does not exist");
            }

            Init();

        }

        public void Init()
        {
            BasicEngine.Managers.ShaderManager man = new BasicEngine.Managers.ShaderManager();
            man.CreateProgram("test", @"C:\Projects\OpenGL\basic_engine_git\BasicEngine\BasicEngine\Shaders\vertex.glsl", @"C:\Projects\OpenGL\basic_engine_git\BasicEngine\BasicEngine\Shaders\fragment.glsl");
            shaderProgram = BasicEngine.Managers.ShaderManager.GetShader("test");

            attrVertexPos = GL.GetAttribLocation(shaderProgram, "vPosition");
            attrVertexNorm = GL.GetAttribLocation(shaderProgram, "nPosition");
            uniformMView = GL.GetUniformLocation(shaderProgram, "modelview");

            if (attrVertexPos == -1 || uniformMView == -1 || attrVertexNorm == -1)
            {
                Console.WriteLine("UAUAUAUA: Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_Norm);
            GL.GenBuffers(1, out vbo_mview);

            mviewdata = new Matrix4[]{
                Matrix4.Identity
            };

            vecPosDat = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vecPosDat[i] = new Vector3(vertices[i].Vertex.X / 80f, vertices[i].Vertex.Y / 80f, 0.0f);
                //Console.WriteLine("Setting Vector: " + vecPosDat[i]);
            }

            nPosDat = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) nPosDat[i] = vertices[i].Normal;
        }

        public void Prepare()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vecPosDat.Length * Vector3.SizeInBytes), vecPosDat, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attrVertexPos, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_Norm);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(nPosDat.Length * Vector3.SizeInBytes), nPosDat, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attrVertexNorm, 3, VertexAttribPointerType.Float, true, 0, 0);


            GL.UniformMatrix4(uniformMView, false, ref mviewdata[0]);

            GL.UseProgram(shaderProgram);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            /*
            if (verticesBufferId == 0)
            {
                GL.GenBuffers(1, out verticesBufferId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf(typeof(VertexObj))), vertices, BufferUsageHint.StaticDraw);
            }

            if (trianglesBufferId == 0)
            {
                GL.GenBuffers(1, out trianglesBufferId);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(Triangle))), triangles, BufferUsageHint.StaticDraw);
            }

            if (quadsBufferId == 0)
            {
                GL.GenBuffers(1, out quadsBufferId);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(Quad))), quads, BufferUsageHint.StaticDraw);
            }*/

        }

        public void Render()
        {
            Prepare();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);


            GL.EnableVertexAttribArray(attrVertexPos);
            GL.EnableVertexAttribArray(attrVertexNorm);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, vecPosDat.Length);

            GL.DisableVertexAttribArray(attrVertexPos);
            GL.EnableVertexAttribArray(attrVertexNorm);


            GL.Flush();

            /*

            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(VertexObj)), IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
            //GL.DrawElements(BeginMode.Triangles, triangles.Length * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DrawElements(PrimitiveType.Triangles, triangles.Length * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);

            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
                //GL.DrawElements(BeginMode.Quads, quads.Length * 4, DrawElementsType.UnsignedInt, IntPtr.Zero);
                GL.DrawElements(PrimitiveType.Quads, quads.Length * 4, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            GL.PopClientAttrib();
            */

        }
    }
}
