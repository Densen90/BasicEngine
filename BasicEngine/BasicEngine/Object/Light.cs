namespace BasicEngine.Object
{
    public enum LightType
    {
        Directional,
        Point,
    }

    class Light
    {
        //TODO: finish and set global light
        public LightType Type { get; set; }
        public Transform Transform { get; set; }
    }
}
