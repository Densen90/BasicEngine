using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicEngine.Utility
{
    class IndexVBO
    {
        public static void Index(Vector3[] in_vertices, Vector2[] in_uvs, Vector3[] in_normals, out int[] out_indexes, out Vector3[] out_vertices, out Vector2[] out_uvs, out Vector3[] out_normals)
        {
            List<Vector3> out_VList = new List<Vector3>();
            List<Vector2> out_UVList = new List<Vector2>();
            List<Vector3> out_NList = new List<Vector3>();
            List<int> outInd = new List<int>();

            for (int i=0; i<in_vertices.Length; i++)
            {
                int index;
                bool found = GetSimilarVertexIndex(in_vertices[i], in_uvs[i], in_normals[i], out_VList.ToArray(), out_UVList.ToArray(), out_NList.ToArray(), out index);
                
                if(found)
                {
                    // A similar vertex is already in the VBO, use it instead !
                    outInd.Add(index);
                }
                else
                {
                    // If not, it needs to be added in the output data.
                    out_VList.Add(in_vertices[i]);
                    out_UVList.Add(in_uvs[i]);
                    out_NList.Add(in_normals[i]);
                    outInd.Add(out_VList.Count - 1);
                }
            }

            out_indexes = outInd.ToArray();
            out_vertices = out_VList.ToArray();
            out_uvs = out_UVList.ToArray();
            out_normals = out_NList.ToArray();
        }

        // Searches through all already-exported vertices
        // for a similar one.
        // Similar = same position + same UVs + same normal
        // Look through all out vertixes sorted yet
        private static bool GetSimilarVertexIndex(Vector3 in_vertex, Vector2 in_uv, Vector3 in_normal, Vector3[] out_vertices, Vector2[] out_uvs, Vector3[] out_normals, out int result)
        {
            result = -1;
            //Linear search, optimization possible
            for(int i=0; i<out_vertices.Length; i++)
            {
                if( IsNear(in_vertex.X, out_vertices[i].X) &&
                    IsNear(in_vertex.Y, out_vertices[i].Y) &&
                    IsNear(in_vertex.X, out_vertices[i].Z) &&
                    IsNear(in_uv.X, out_uvs[i].X) &&
                    IsNear(in_uv.Y, out_uvs[i].Y) &&
                    IsNear(in_normal.X, out_normals[i].X) &&
                    IsNear(in_normal.Y, out_normals[i].Y) &&
                    IsNear(in_normal.Z, out_normals[i].Z))
                {
                    result = i;
                    return true;
                }
            }
            return false;
        }

        // Returns true if v1 can be considered equal to v2
        private static bool IsNear(float v1, float v2)
        {
            return Math.Abs(v1 - v2) < 0.01f;
        }
    }
}
