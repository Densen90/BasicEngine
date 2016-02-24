using OpenTK.Graphics.OpenGL;
using BasicEngine.Rendering;
using BasicEngine.Managers;
using System.IO;

namespace BasicEngine.Utility
{
    public static class ShaderLoader
    {
        public static Shader LoadFromString(string shaderName, string vertexFilename, string fragmentFilename)
        {
            if (ShaderManager.ShaderExists(shaderName)) return ShaderManager.GetShader(shaderName);

            if (!File.Exists(vertexFilename)) throw new FileNotFoundException("Shader File " + vertexFilename + "does not exist");
            if (!File.Exists(fragmentFilename)) throw new FileNotFoundException("Shader File " + fragmentFilename + "does not exist");

            string vertexSrc = File.ReadAllText(vertexFilename);
            string fragmentSrc = File.ReadAllText(fragmentFilename);

            Shader shader = new Shader(shaderName);

            shader.Compile(vertexSrc, ShaderType.VertexShader);
            shader.Compile(fragmentSrc, ShaderType.FragmentShader);
            shader.Link();

            ShaderManager.AddShader(shaderName, shader);

            return shader;
        }
    }
}
