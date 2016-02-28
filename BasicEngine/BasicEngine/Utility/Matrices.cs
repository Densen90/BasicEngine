using OpenTK;
using System;

namespace BasicEngine.Utility
{
    public static class Matrices
    {
        public static Matrix4 TranslationMatrix(Vector3 translation)
        {
            Vector4 one = new Vector4(1, 0, 0, translation.X);
            Vector4 two = new Vector4(0, 1, 0, translation.Y);
            Vector4 three = new Vector4(0, 0, 1, translation.Z);
            Vector4 four = new Vector4(0, 0, 0, 1);

            return new Matrix4(one, two, three, four);
        }

        public static Matrix4 ScalingMatrix(Vector3 scale)
        {
            Vector4 one = new Vector4(scale.X, 0, 0, 0);
            Vector4 two = new Vector4(0, scale.Y, 0, 0);
            Vector4 three = new Vector4(0, 0, scale.Z, 0);
            Vector4 four = new Vector4(0, 0, 0, 1);

            return new Matrix4(one, two, three, four);
        }

        public static Matrix4 RotationMatrix(Vector3 rotation)
        {
            return Matrix4.Identity;
        }
    }
}
