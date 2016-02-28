using BasicEngine.Utility;
using OpenTK;
using System;
using System.Linq;

namespace BasicEngine
{
    class Transform
    {
        public Vector3 Position { get; set; }

        private Vector3[] vertices;

        public Transform(Vector3[] vertices)
        {
            this.vertices = vertices;
            CalculatePosition();
        }

        //TODO: TRANSFORM VERTICES TO VEC4
        public void Translate(Vector3 dir, out Vector3[] verts)
        {
            Matrix4 translationMatrix = Matrices.TranslationMatrix(dir);

            for(int i=0; i<vertices.Length; i++)
            {
                Vector4 t = Vector4.Transform(new Vector4(vertices[i].X, vertices[i].Y, vertices[i].Z, 1), translationMatrix);
                vertices[i] = new Vector3(t.X, t.Y, t.Z);
            }

            verts = vertices;
            CalculatePosition();
        }

        public void Scale(Vector3 scale)
        {

        }

        private void CalculatePosition()
        {
            float xMed = vertices.Sum(s => s.X) / vertices.Length;
            float yMed = vertices.Sum(s => s.Y) / vertices.Length;
            float zMed = vertices.Sum(s => s.Z) / vertices.Length;

            Position = new Vector3(xMed, yMed, zMed);
        }
    }
}
