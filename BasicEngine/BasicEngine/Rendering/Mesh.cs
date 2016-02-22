using System;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using BasicEngine.Rendering;

public class Mesh
{
    public ObjVertex[] Vertices
    {
        get { return vertices; }
        set { vertices = value; }
    }
    ObjVertex[] vertices;

    public ObjTriangle[] Triangles
    {
        get { return triangles; }
        set { triangles = value; }
    }
    ObjTriangle[] triangles;

    public ObjQuad[] Quads
    {
        get { return quads; }
        set { quads = value; }
    }
    ObjQuad[] quads;

    public Shader Shader{ get; set; }

    private int vertexArrayID;  //VAO
    private int vertexBuffer;
    private int normalBuffer;
    private int trianglesBufferId;
    private int quadsBufferId;

    private int matrixID, mID, vID, mvID, lightPosID;
    private Matrix4 MVP;

    public Mesh(string fileName)
    {
        MeshLoader.Load(this, fileName);
        Shader = BasicEngine.Managers.ShaderManager.GetShader("DefaultShader");
        Init();
    }

    void Init()
    {
        GL.GenVertexArrays(1, out vertexArrayID);
        GL.BindVertexArray(vertexArrayID);

        //Creating our MVP Matrix --> TODO: Camera Class?
        matrixID = Shader.GetUniformLocation("mvpMatrix");   //Get the Handle for our uniform in our shader
        mID = Shader.GetUniformLocation("M");
        vID = Shader.GetUniformLocation("V");
        mvID = Shader.GetUniformLocation("MV");
        lightPosID = Shader.GetUniformLocation("LightPosWorldspace");

        //generate 1 buffer, put resulting identifier in vertexBuffer
        GL.GenBuffers(1, out vertexBuffer);
        //The following command will talk about our 'vertexBuffer' buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        //Give the vertices to OpenGL
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Select(v=>v.Vertex).ToArray().Length * Vector3.SizeInBytes), vertices.Select(v => v.Vertex).ToArray(), BufferUsageHint.StaticDraw);

        //same for vertex Normals
        GL.GenBuffers(1, out normalBuffer);
        GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Select(v => v.Normal).ToArray().Length * Vector3.SizeInBytes), vertices.Select(v => v.Normal).ToArray(), BufferUsageHint.StaticDraw);

        //same for the indexed triangles
        GL.GenBuffers(1, out trianglesBufferId);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf(typeof(ObjTriangle))), triangles, BufferUsageHint.StaticDraw);

        //and for the indexed quads
        GL.GenBuffers(1, out quadsBufferId);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
        GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf(typeof(ObjQuad))), quads, BufferUsageHint.StaticDraw);
    }

    public void Render()
    {
        //Calculate MVP
        Matrix4 M = Matrix4.Identity;
        Matrix4 V = Camera.Instance.ViewMatrix;
        Matrix4 MV = M * V;
        MVP = MV * Camera.Instance.ProjectionMatrix;

        //TODO: do in seperate class
        Vector3 lightPos = new Vector3(-2, 2, 4);

        //Use the shader
        Shader.Begin();

        //send the modelViewMatrix to our Shader as uniform
        GL.UniformMatrix4(matrixID, false, ref MVP);
        GL.UniformMatrix4(mID, false, ref M);
        GL.UniformMatrix4(vID, false, ref V);
        GL.UniformMatrix4(mvID, false, ref MV);
        GL.Uniform3(lightPosID, ref lightPos);

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


        GL.BindBuffer(BufferTarget.ElementArrayBuffer, trianglesBufferId);
        GL.DrawElements(PrimitiveType.Triangles, triangles.Length * 3, DrawElementsType.UnsignedInt, IntPtr.Zero);

        if (quads.Length > 0)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, quadsBufferId);
            GL.DrawElements(PrimitiveType.Quads, quads.Length * 4, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        GL.DisableVertexAttribArray(0);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ObjVertex
    {
        public Vector3 Vertex;
        public Vector3 Normal;
        public Vector2 TexCoord;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ObjTriangle
    {
        public int Index0;
        public int Index1;
        public int Index2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ObjQuad
    {
        public int Index0;
        public int Index1;
        public int Index2;
        public int Index3;
    }
}