namespace BasicEngine.Rendering
{
    public enum LightType
    {
        Directional,
        Point,
    }

    class Light
    {
        public LightType Type { get; set; }
    }
}
