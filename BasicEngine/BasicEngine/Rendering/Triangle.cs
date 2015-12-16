using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace BasicEngine.Rendering
{
    class Triangle : Model
    {
        public void Create()
        {
            int vbo, vao;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);    //Create and bind our vertex array object

          //  List<VertexFormat> vertices = new List<VertexFormat>();
           // vertices.Add( new VertexFormat( new Vector3(0.25f, -0.25f, 0.0f), new Vector4(1f, 0f, 0f, 1f)) );
          //  vertices.Add( new VertexFormat(new Vector3(-0.25f, -0.25f, 0.0f), new Vector4(0f, 1f, 0f, 1f)) );
           // vertices.Add( new VertexFormat(new Vector3(0.25f, -0.25f, 0.0f), new Vector4(0f, 0f, 1f, 1f)) );

            GL.GenBuffers(1, out vbo); //generate VBO container and get ID for it
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); //bind to context

        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
