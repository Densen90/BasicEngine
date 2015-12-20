using System;
using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace BasicEngine.Managers
{
    class ShaderManager : IDisposable
    {
        private static Dictionary<string, int> programDic = null;  //same dictionary for all Instances

        public ShaderManager()
        {
            if (programDic == null) programDic = new Dictionary<string, int>();
        }

        public void Dispose()
        {
            foreach(var kvp in programDic)
            {
                GL.DeleteProgram(kvp.Value);
            }
            programDic.Clear();
        }

        /// <summary>
        /// Creates a program for the shaders
        /// </summary>
        /// <param name="shaderName">name of the shader</param>
        /// <param name="vertexShaderFilename">filename of vertex shader with path</param>
        /// <param name="fragmentShaderFilename">filename of fragment shader with path</param>
        public void CreateProgram(string shaderName, string vertexShaderFilename, string fragmentShaderFilename)
        {
            string vertexShaderCode = ReadShader(vertexShaderFilename);
            string fragmentShaderCode = ReadShader(fragmentShaderFilename);

            int vertexShader = CreateShader(ShaderType.VertexShader, vertexShaderCode, shaderName);
            int fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentShaderCode, shaderName);

            int program = GL.CreateProgram();   //create program to fill our shader with
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);   //attach the shaders to our program

            GL.LinkProgram(program);
            int linkSuccess;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linkSuccess);
            if(linkSuccess==0)
            {
                Console.WriteLine("ERROR: ShaderManager.CreateProgram:\n\tLinker error at shader " + shaderName);
                Console.WriteLine("\tLog: " + GL.GetProgramInfoLog(program));
                return;
            }

            if(programDic.ContainsKey(shaderName))
            {
                Console.WriteLine("Warning: ShaderManager.CreateProgram:\n\tA program for the shader " + shaderName + "does already exist. It will be overwritten");
                programDic[shaderName] = program;
            }
            else
            {
                programDic.Add(shaderName, program);
            }
        }

        /// <summary>
        /// Get the program for the given name
        /// </summary>
        /// <param name="shaderName">name of the shader</param>
        /// <returns>id of shader program</returns>
        public static int GetShader(string shaderName)
        {
            if(!programDic.ContainsKey(shaderName))
            {
                Console.WriteLine("ERROR: ShaderManager.GetShader:\n\tCannot Get program for shader " + shaderName + ", it does not exist");
                return 0;
            }

            return programDic[shaderName];
        }

        /// <summary>
        /// Loads a shader from a file and returns its content
        /// </summary>
        /// <param name="filename">The name of the file with its path</param>
        /// <returns>The content of the shader file</returns>
        private string ReadShader(string filename)
        {
            string shaderCode = string.Empty;

            if(File.Exists(filename))
            {
                shaderCode = File.ReadAllText(filename);
            }
            else
            {
                Console.WriteLine("ERROR: ShaderManager.ReadShader:\n\tCannot Open shader file: " + filename + ", File does not exist");
            }

            return shaderCode;
        }

        /// <summary>
        /// Creates a shader from given code
        /// </summary>
        /// <param name="type">The type of the shader (vertex, fragment)</param>
        /// <param name="source">The source code of the given shader</param>
        /// <param name="shaderName">The name for the shader</param>
        /// <returns>The id of the shader</returns>
        private int CreateShader(ShaderType type, string source, string shaderName)
        {
            int shader = GL.CreateShader(type); //create an empty shader

            GL.ShaderSource(shader, source);    //load the shader object with the code
            GL.CompileShader(shader);

            int compileSuccess;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out compileSuccess);
            if(compileSuccess==0)
            {
                Console.WriteLine("ERROR: ShaderManager:\n\tCompile error at shader " + shaderName);
                Console.WriteLine("\tLog: " + GL.GetShaderInfoLog(shader));
                return 0;
            }

            return shader;
        }
    }
}
