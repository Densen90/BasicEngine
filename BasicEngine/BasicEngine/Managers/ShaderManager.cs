using System;
using System.Collections.Generic;
using BasicEngine.Rendering;

namespace BasicEngine.Managers
{
    class ShaderManager
    {
        private static Dictionary<string, Shader> programDic = new Dictionary<string, Shader>();  //same dictionary for all Instances

        public static void AddShader(string shaderName, Shader shader)
        {
            if (programDic.ContainsKey(shaderName)) throw new Exception(("ERROR: ShaderManager.GetShader:\n\t" + shaderName + ", already exists"));

            programDic.Add(shaderName, shader);
        }

        /// <summary>
        /// Get the program for the given name
        /// </summary>
        /// <param name="shaderName">name of the shader</param>
        /// <returns>id of shader program</returns>
        public static Shader GetShader(string shaderName)
        {
            if (!programDic.ContainsKey(shaderName)) throw new Exception(("ERROR: ShaderManager.GetShader:\n\t" + shaderName + ", does not exist"));

            return programDic[shaderName];
        }

        public static void RemoveShader(string shaderName)
        {
            if (!programDic.ContainsKey(shaderName)) throw new Exception(("ERROR: ShaderManager.GetShader:\n\t" + shaderName + ", does not exist"));

            programDic.Remove(shaderName);
        }

        public static bool ShaderExists(string shaderName)
        {
            return programDic.ContainsKey(shaderName);
        }
    }
}
