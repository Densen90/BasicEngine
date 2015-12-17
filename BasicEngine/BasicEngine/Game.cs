using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BasicEngine
{
    class Game : GameWindow
    {
        int programID;
        int attribute_vcol;
        int attribute_vpos;
        int uniform_mview;
        int vbo_position;
        int vbo_color;
        int vbo_mview;

        Vector3[] vertdata;
        Vector3[] coldata;
        Matrix4[] mviewdata;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            initProgram();

            vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)};


            coldata = new Vector3[] { new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f,  1f, 0f)};


            mviewdata = new Matrix4[]{
                Matrix4.Identity
            };



            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);


            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);

            GL.DrawArrays(BeginMode.Triangles, 0, 3);


            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);


            GL.Flush();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);



            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.UniformMatrix4(uniform_mview, false, ref mviewdata[0]);

            GL.UseProgram(programID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        void initProgram()
        {
            //TODO: FileService Class which handles the Files, not in ShaderManager
            BasicEngine.Managers.ShaderManager man = new BasicEngine.Managers.ShaderManager();
            man.CreateProgram("test", @"C:\Projects\OpenGL\basic_engine_git\BasicEngine\BasicEngine\Shaders\vertex.glsl", @"C:\Projects\OpenGL\basic_engine_git\BasicEngine\BasicEngine\Shaders\fragment.glsl");
            programID = man.GetShader("test");

            GL.LinkProgram(programID);
            Console.WriteLine(GL.GetProgramInfoLog(programID));

            attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(programID, "vColor");
            uniform_mview = GL.GetUniformLocation(programID, "modelview");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
        }
    }
}
