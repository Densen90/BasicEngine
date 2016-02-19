using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;

public class MeshLoader
{
    public static bool Load(Mesh mesh, string fileName)
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

    static char[] splitCharacters = new char[] { ' ' };
    static char[] faceParamaterSplitter = new char[] { '/' };

    static List<Vector3> vertices;
    static List<Vector3> normals;
    static List<Vector2> texCoords;
    static Dictionary<Mesh.ObjVertex, int> objVerticesIndexDictionary;
    static List<Mesh.ObjVertex> objVertices;
    static List<Mesh.ObjTriangle> objTriangles;
    static List<Mesh.ObjQuad> objQuads;

    static void Load(Mesh mesh, TextReader textReader)
    {
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        texCoords = new List<Vector2>();
        objVerticesIndexDictionary = new Dictionary<Mesh.ObjVertex, int>();
        objVertices = new List<Mesh.ObjVertex>();
        objTriangles = new List<Mesh.ObjTriangle>();
        objQuads = new List<Mesh.ObjQuad>();

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
                            Mesh.ObjTriangle objTriangle = new Mesh.ObjTriangle();
                            objTriangle.Index0 = ParseFaceParameter(parameters[1]);
                            objTriangle.Index1 = ParseFaceParameter(parameters[2]);
                            objTriangle.Index2 = ParseFaceParameter(parameters[3]);
                            objTriangles.Add(objTriangle);
                            break;

                        case 5:
                            Mesh.ObjQuad objQuad = new Mesh.ObjQuad();
                            objQuad.Index0 = ParseFaceParameter(parameters[1]);
                            objQuad.Index1 = ParseFaceParameter(parameters[2]);
                            objQuad.Index2 = ParseFaceParameter(parameters[3]);
                            objQuad.Index3 = ParseFaceParameter(parameters[4]);
                            objQuads.Add(objQuad);
                            break;
                    }
                    break;
            }
        }

        mesh.Vertices = objVertices.ToArray();
        mesh.Triangles = objTriangles.ToArray();
        mesh.Quads = objQuads.ToArray();

        Console.WriteLine("Finished Loading file: " + Environment.NewLine + 
            mesh.Vertices.Length + " Vertices" + Environment.NewLine + 
            mesh.Triangles.Length + " Triangles" + Environment.NewLine + 
            mesh.Quads.Length + " Quads");

        objVerticesIndexDictionary = null;
        vertices = null;
        normals = null;
        texCoords = null;
        objVertices = null;
        objTriangles = null;
        objQuads = null;
    }

    static int ParseFaceParameter(string faceParameter)
    {
        Vector3 vertex = new Vector3();
        Vector2 texCoord = new Vector2();
        Vector3 normal = new Vector3();

        string[] parameters = faceParameter.Split(faceParamaterSplitter);

        int vertexIndex = int.Parse(parameters[0]);
        if (vertexIndex < 0) vertexIndex = vertices.Count + vertexIndex;
        else vertexIndex = vertexIndex - 1;
        vertex = vertices[vertexIndex];

        if (parameters.Length > 1)
        {
            if (parameters[1] != "")
            {
                int texCoordIndex = int.Parse(parameters[1]);
                if (texCoordIndex < 0) texCoordIndex = texCoords.Count + texCoordIndex;
                else texCoordIndex = texCoordIndex - 1;
                texCoord = texCoords[texCoordIndex];
            }
        }

        if (parameters.Length > 2)
        {
            int normalIndex = int.Parse(parameters[2]);
            if (normalIndex < 0) normalIndex = normals.Count + normalIndex;
            else normalIndex = normalIndex - 1;
            normal = normals[normalIndex];
        }

        return FindOrAddObjVertex(ref vertex, ref texCoord, ref normal);
    }

    static int FindOrAddObjVertex(ref Vector3 vertex, ref Vector2 texCoord, ref Vector3 normal)
    {
        Mesh.ObjVertex newObjVertex = new Mesh.ObjVertex();
        newObjVertex.Vertex = vertex;
        newObjVertex.TexCoord = texCoord;
        newObjVertex.Normal = normal;

        int index;
        if (objVerticesIndexDictionary.TryGetValue(newObjVertex, out index))
        {
            return index;
        }
        else
        {
            objVertices.Add(newObjVertex);
            objVerticesIndexDictionary[newObjVertex] = objVertices.Count - 1;
            return objVertices.Count - 1;
        }
    }
}