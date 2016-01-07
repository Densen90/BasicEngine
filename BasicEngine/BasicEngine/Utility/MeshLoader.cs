using System;
using System.Collections.Generic;
using System.IO;
using BasicEngine.Rendering;
using OpenTK;

namespace BasicEngine.Utility
{
    /// <summary>
    /// This class can load and Parse .obj files for meshes
    /// </summary>
    class MeshLoader
    {
        static List<int> vertexIndices, uvIndices, normalIndices;
        static List<Vector3> vertices;
        static List<Vector3> normals;
        static List<Vector2> texCoords;

        static char[] splitCharacters = new char[] { ' ' };
        static char[] faceParamaterSplitter = new char[] { '/' };

        public static bool Load(Mesh mesh, string fileName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    Load(mesh, streamReader);
                    streamReader.Close();
                    return true;
                }
            }
            catch { return false; }
        }

        static void Load(Mesh mesh, TextReader textReader)
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            texCoords = new List<Vector2>();
            vertexIndices = new List<int>();
            uvIndices = new List<int>();
            normalIndices = new List<int>();

            string line;
            while ((line = textReader.ReadLine()) != null)
            {
                line = line.Trim(splitCharacters);
                line = line.Replace("  ", " ");
                line = line.Replace(".", ",");

                string[] parameters = line.Split(splitCharacters);

                switch (parameters[0])
                {
                    case "p": // Point
                        break;

                    case "v": // Vertex
                        float x = float.Parse(parameters[1]);
                        float y = float.Parse(parameters[2]);
                        float z = float.Parse(parameters[3]);
                        vertices.Add(new Vector3(x, y, z));
                        break;

                    case "vt": // TexCoord
                        float u = float.Parse(parameters[1]);
                        float v = float.Parse(parameters[2]);
                        texCoords.Add(new Vector2(u, v));
                        break;

                    case "vn": // Normal
                        float nx = float.Parse(parameters[1]);
                        float ny = float.Parse(parameters[2]);
                        float nz = float.Parse(parameters[3]);
                        normals.Add(new Vector3(nx, ny, nz));
                        break;

                    case "f":
                        switch (parameters.Length)
                        {
                            case 4:
                                Vector3 vInd = ParseFaceParameter(parameters[1]);
                                Vector3 uvInd = ParseFaceParameter(parameters[1]);
                                Vector3 nInd = ParseFaceParameter(parameters[1]);
                                vertexIndices.Add((int)vInd.X); vertexIndices.Add((int)vInd.Y); vertexIndices.Add((int)vInd.Z);
                                uvIndices.Add((int)uvInd.X); uvIndices.Add((int)uvInd.Y); uvIndices.Add((int)uvInd.Z);
                                normalIndices.Add((int)nInd.X); normalIndices.Add((int)nInd.Y); normalIndices.Add((int)nInd.Z);
                                break;
                        }
                        break;
                }
            }

            SortList();

            vertices = null;
            normals = null;
            texCoords = null;
        }

        private static Vector3 ParseFaceParameter(string parameter)
        {
            string[] indices = parameter.Split(faceParamaterSplitter);
            if(indices.Length<3)
            {
                Console.WriteLine("MeshLoader: Face parameter has less than 3 values.");
                return Vector3.Zero;
            }

            Vector3 indVec = new Vector3(int.Parse(indices[0]), int.Parse(indices[1]), int.Parse(indices[2]));
            return indVec;
        }

        private static void SortList()
        {
            for(int i=0; i<vertexIndices.Count; i++)
            {
                //Get the index from the vertex
                int vertexIndex = vertexIndices[i];
                //position is vertexIndex-1 --> C' indexing starts at 0, Obj indexing starts at 1
                Vector3 vertex = vertices[vertexIndex-1];

            }
        }
    }
}
