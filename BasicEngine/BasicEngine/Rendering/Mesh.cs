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


        public Mesh(string fileName)
        {
            //TODO: FileService
            if(MeshLoader.Load(this, fileName))
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

        int verticesBufferId;
        int trianglesBufferId;
        int quadsBufferId;

        public void Prepare()
        {
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
            }
        }

        public void Render()
        {
            Prepare();

            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);
            GL.EnableClientState(EnableCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferId);
            GL.InterleavedArrays(InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf(typeof(VertexObj)), IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
            GL.DrawElements(BeginMode.Triangles, triangles.Length * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);

            if (quads.Length > 0)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
                GL.DrawElements(BeginMode.Quads, quads.Length * 4, DrawElementsType.UnsignedInt, IntPtr.Zero);
            }

            GL.PopClientAttrib();
        }
    }
}
