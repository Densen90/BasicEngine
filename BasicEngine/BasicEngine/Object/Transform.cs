﻿using OpenTK;
using System.Linq;

namespace BasicEngine.Object
{
    class Transform
    {
        private Vector3[] vertices;

        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
                
            set
            {
                Vector3 dist = value - position;
                Translate(dist);
            }
        }

        public Transform()
        {
            position = Vector3.Zero;
            this.vertices = new Vector3[0];
        }

        public Transform(Vector3[] vertices)
        {
            this.vertices = vertices;
            CalculatePosition();
        }

        //TODO: TRANSFORM VERTICES TO VEC4
        public void Translate(Vector3 dir)
        {
            //Matrix4 translationMatrix = Matrices.TranslationMatrix(dir);

            for(int i=0; i<vertices.Length; i++)
            {
                //GL.Translate(vertices[i].X, vertices[i].Y, vertices[i].Z);
                //Vector4 t = Vector4.Transform(new Vector4(vertices[i].X, vertices[i].Y, vertices[i].Z, 1), translationMatrix);
                //Console.WriteLine("Old: " + vertices[i] + ", New: " + t);
                vertices[i] = new Vector3(vertices[i].X + dir.X, vertices[i].Y + dir.Y, vertices[i].Z + dir.Z);
                //vertices[i] = new Vector3(t.X, t.Y, t.Z);
            }

            position = vertices.Length>0 ? CalculatePosition() : position+dir;
        }

        public void Scale(Vector3 scale)
        {

        }

        private Vector3 CalculatePosition()
        {
            float xMed = vertices.Sum(s => s.X) / vertices.Length;
            float yMed = vertices.Sum(s => s.Y) / vertices.Length;
            float zMed = vertices.Sum(s => s.Z) / vertices.Length;

            return new Vector3(xMed, yMed, zMed);
        }
    }
}
