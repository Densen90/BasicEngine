using System;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using BasicEngine.Rendering;
using System.Drawing;
using System.Drawing.Imaging;

public class Mesh
{
    //TODO: NOT A STRUCT TO COMBINE VERTEX, NORMAL & UV
    //TODO: VERTEX AS VEC4
    private Vector3[] vertices;
    public Vector3[] Vertices
    {
        get { return vertices; }
        set { vertices = value; }
    }

    private Vector3[] normals;
    public Vector3[] Normals
    {
        get { return normals; }
        set { normals = value; }
    }

    private Vector2[] uvs;
    public Vector2[] UVs
    {
        get { return uvs; }
        set { uvs = value; }
    }

    private ObjTriangle[] triangles;
    public ObjTriangle[] Triangles
    {
        get { return triangles; }
        set { triangles = value; }
    }

    private ObjQuad[] quads;
    public ObjQuad[] Quads
    {
        get { return quads; }
        set { quads = value; }
    }

    public Shader Shader{ get; set; }

    public Bitmap Texture;

    private int vertexArrayID;  //VAO
    private int vertexBuffer;
    private int normalBuffer;
    private int texBuffer;
    private int trianglesBufferId;
    private int quadsBufferId;

    private int texID;

    private int matrixID, mID, vID, mvID, lightPosID, mainTexID;
    private Matrix4 MVP;

    public Mesh(string path, string fileName)
    {
        MeshLoader.Load(this, path, fileName);

        if(Texture!=null)
        {
            Console.WriteLine("Found Texture in Mesh, Generating ID");
            LoadImage(Texture);
        }

        Shader = BasicEngine.Managers.ShaderManager.GetShader("DefaultShader");
        Init();
    }

    void Init()
    {
        GL.GenVertexArrays(1, out vertexArrayID);
        GL.BindVertexArray(vertexArrayID);

        matrixID = Shader.GetUniformLocation("mvpMatrix");   //Get the Handle for our uniform in our shader
        mID = Shader.GetUniformLocation("M");
        vID = Shader.GetUniformLocation("V");
        mvID = Shader.GetUniformLocation("MV");
        lightPosID = Shader.GetUniformLocation("LightPosWorldspace");
        mainTexID = Shader.GetUniformLocation("MainTexture");

        
    }

    private void Prepare()
    {
        //generate 1 buffer, put resulting identifier in vertexBuffer
        GL.GenBuffers(1, out vertexBuffer);
        //The following command will talk about our 'vertexBuffer' buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
        //Give the vertices to OpenGL
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);

        //same for vertex Normals
        GL.GenBuffers(1, out normalBuffer);
        GL.BindBuffer(BufferTarget.ArrayBuffer, normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);

        //same for TexCoords
        GL.GenBuffers(1, out texBuffer);
        GL.BindBuffer(BufferTarget.ArrayBuffer, texBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(uvs.Length * Vector3.SizeInBytes), uvs, BufferUsageHint.StaticDraw);

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
        Prepare();
        //Calculate MVP
        Matrix4 M = Matrix4.Identity;
        Matrix4 V = Camera.Instance.ViewMatrix;
        Matrix4 MV = M * V;
        MVP = MV * Camera.Instance.ProjectionMatrix;

        //TODO: do in seperate class
        Vector3 lightPos = new Vector3(-3, 3, 5);

        //Use the shader
        Shader.Begin();

        //send the modelViewMatrix to our Shader as uniform
        GL.UniformMatrix4(matrixID, false, ref MVP);
        GL.UniformMatrix4(mID, false, ref M);
        GL.UniformMatrix4(vID, false, ref V);
        GL.UniformMatrix4(mvID, false, ref MV);
        GL.Uniform3(lightPosID, ref lightPos);

        if (Texture != null)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texID);
            GL.BindTexture(TextureTarget.Texture2D, texID);
            GL.Uniform1(mainTexID, texID);
        }

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

        //3rd attribute buffer: texcoord
        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, texBuffer);
        GL.VertexAttribPointer(
            2,                      //layout(location = 2)
            2,
            VertexAttribPointerType.Float,
            false,
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

    private void LoadImage(Bitmap image)
    {
        texID = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, texID);
        BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

        image.UnlockBits(data);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
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