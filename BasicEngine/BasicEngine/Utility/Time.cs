using OpenTK;
using System;

namespace BasicEngine.Utility
{
    static class Time
    {
        public static float DeltaTime { get; set; }
        public static float FPS { get; set; }

        public static void UpdateEvent(object sender, FrameEventArgs e)
        {
            DeltaTime = (float) e.Time;
            FPS = (float) Math.Round(1.0 / e.Time, MidpointRounding.AwayFromZero);
        }
    }
}
