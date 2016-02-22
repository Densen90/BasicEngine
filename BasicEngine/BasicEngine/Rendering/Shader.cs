using System;
using OpenTK.Graphics.OpenGL;

namespace BasicEngine.Rendering
{
    public class ShaderException : Exception
    {
        public string Type { get; set; }

        public ShaderException(string type, string msg)
        {
            Type = type;
        }
    }

    public class Shader : IDisposable
    {
        public string Name { get; set; }
        private int program;

        public Shader()
        {
            program = GL.CreateProgram();
            Name = "DefaultShader";
        }

        public Shader(string name)
        {
            program = GL.CreateProgram();
            Name = name;
        }

        public void Dispose()
        {
            if(program!=0)
            {
                GL.DeleteProgram(program);
            }
        }

        public void Compile(string shaderSrc, ShaderType type)
        {
            int shader = GL.CreateShader(type); //create an empty shader

            if (shader == 0) throw new ShaderException(type.ToString(), "Could not create shader with this type for: " + Name);

            GL.ShaderSource(shader, shaderSrc);    //load the shader object with the code
            GL.CompileShader(shader);

            int compileSuccess;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out compileSuccess);
            if (compileSuccess != 1) throw new ShaderException(type.ToString(), "Log for " + Name + Environment.NewLine + GL.GetShaderInfoLog(shader));

            GL.AttachShader(program, shader);
        }

        public int GetUniformLocation(string uName)
        {
            return GL.GetUniformLocation(program, uName);
        }

        public void Begin()
        {
            GL.UseProgram(program);
        }

        public void End()
        {
            GL.UseProgram(0);
        }

        public void Link()
        {
            try
            {
                GL.LinkProgram(program);
            }
            catch(Exception e)
            {
                throw new ShaderException("Link", "Linker error for " + Name);
            }

            int linkSuccess;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linkSuccess);
            if (linkSuccess != 1) throw new ShaderException("Link", Name + ":" + Environment.NewLine + GL.GetProgramInfoLog(program));
        }
    }
}
