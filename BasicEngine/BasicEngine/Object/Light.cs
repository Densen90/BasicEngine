using OpenTK;

namespace BasicEngine.Object
{
    public enum LightType
    {
        Directional,
        Point,
    }

    class Light
    {
        public LightType Type { get; set; }
        public Transform Transform { get; set; }

        //TODO: no singleton, just now for simplicity
        private static Light instance;
        public static Light Instance
        {
            get
            {
                if (instance == null) instance = new Light();
                return instance;
            }
        }

        public Light()
        {
            Type = LightType.Point;
            Transform = new Transform();
            Transform.Position = new Vector3(-3, 3, 5);
        }
    }
}
