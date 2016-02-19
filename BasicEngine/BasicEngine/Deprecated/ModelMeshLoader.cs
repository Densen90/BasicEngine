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
    [Obsolete("ModelMeshLoader is deprecated, please use MeshLoader instead.")]
    class ModelMeshLoader
    {
        static List<int> vertexIndices, uvIndices, normalIndices;
        static List<Vector3> vertices;
        static List<Vector3> normals;
        static List<Vector2> texCoords;

        static char[] splitCharacters = new char[] { ' ' };
        static char[] faceParamaterSplitter = new char[] { '/' };

        public static bool Load(ModelMesh mesh, string fileName)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    Console.WriteLine("Load");
                    Load(mesh, streamReader);
                    streamReader.Close();
                    return true;
                }
            }
            catch { return false; }
        }

        static void Load(ModelMesh mesh, TextReader textReader)
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            texCoords = new List<Vector2>();
            vertexIndices = new List<int>();
            uvIndices = new List<int>();
            normalIndices = new List<int>();

            string line;
            Console.WriteLine("Reading Lines");
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
                                Vector3 Ind1 = ParseFaceParameter(parameters[1]);
                                Vector3 Ind2 = ParseFaceParameter(parameters[2]);
                                Vector3 Ind3 = ParseFaceParameter(parameters[3]);
                                vertexIndices.Add((int)Ind1.X); vertexIndices.Add((int)Ind2.X); vertexIndices.Add((int)Ind3.X);
                                uvIndices.Add((int)Ind1.Y); uvIndices.Add((int)Ind2.Y); uvIndices.Add((int)Ind3.Y);
                                normalIndices.Add((int)Ind1.Z); normalIndices.Add((int)Ind2.Z); normalIndices.Add((int)Ind3.Z);
                                break;

                            case 5:
                                Console.WriteLine("Having a Quad here");
                                Vector3 Ind11 = ParseFaceParameter(parameters[1]);
                                Vector3 Ind22 = ParseFaceParameter(parameters[2]);
                                Vector3 Ind33 = ParseFaceParameter(parameters[3]);
                                Vector3 Ind44 = ParseFaceParameter(parameters[4]);
                                vertexIndices.Add((int)Ind11.X); vertexIndices.Add((int)Ind22.X); vertexIndices.Add((int)Ind33.X); vertexIndices.Add((int)Ind44.X);
                                uvIndices.Add((int)Ind11.Y); uvIndices.Add((int)Ind22.Y); uvIndices.Add((int)Ind33.Y); uvIndices.Add((int)Ind44.Y);
                                normalIndices.Add((int)Ind11.Z); normalIndices.Add((int)Ind22.Z); normalIndices.Add((int)Ind33.Z); normalIndices.Add((int)Ind44.Z);
                                break;

                        }
                        break;
                }
            }

            SortList(mesh);

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

            int vi = indices[0] != "" ? int.Parse(indices[0]) : 0;
            int ti = indices[1] != "" ? int.Parse(indices[1]) : 0;
            int ni = indices[2] != "" ? int.Parse(indices[2]) : 0;

            Vector3 indVec = new Vector3(vi, ti, ni);
            return indVec;
        }

        private static void SortList(ModelMesh mesh)
        {
            Console.WriteLine("Sorting Vertices: " + vertexIndices.Count + "/" + vertices.Count);
            List<Vector3> sortVertList = new List<Vector3>();
            for(int i=0; i<vertexIndices.Count; i++)
            {
                //Get the index from the vertex
                int vertexIndex = vertexIndices[i];
                if (vertexIndex == 0) continue; //there is no index 0, only if field was empty in file
                //position is vertexIndex-1 --> C' indexing starts at 0, Obj indexing starts at 1
                Vector3 vertex = vertices[vertexIndex-1];
                sortVertList.Add(vertex);
            }

            Console.WriteLine("Sorting Normals: " + normalIndices.Count);
            List<Vector3> sortNormList = new List<Vector3>();
            for (int i = 0; i < normalIndices.Count; i++)
            {
                //Get the index from the vertex
                int normalIndex = normalIndices[i];
                if (normalIndex == 0) continue; //there is no index 0, only if field was empty in file
                //position is normalIndex-1 --> C' indexing starts at 0, Obj indexing starts at 1
                Vector3 normal = normals[normalIndex - 1];
                sortNormList.Add(normal);
            }

            Console.WriteLine("Sorting UVs: " + uvIndices.Count);
            List<Vector2> sortUVList = new List<Vector2>();
            for (int i = 0; i < uvIndices.Count; i++)
            {
                //Get the index from the vertex
                int uvIndex = uvIndices[i];
                if (uvIndex == 0)
                {
                    sortUVList.Add(Vector2.Zero);
                    continue; //there is no index 0, only if field was empty in file
                }
                //position is normalIndex-1 --> C' indexing starts at 0, Obj indexing starts at 1
                Vector2 uv = texCoords[uvIndex - 1];
                sortUVList.Add(uv);
            }

            Console.WriteLine("Finish Sorting");

            mesh.Vertices = sortVertList.ToArray();
            mesh.Normals = sortNormList.ToArray();
            mesh.UVs = sortUVList.ToArray();
        }
    }
}
