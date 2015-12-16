using System;

namespace BasicEngine.Core
{
    interface IListener : IDisposable
    {
        //drawing functions, called in the displayCallback() method from Init_GLUT class
        //splitted because then we have a good flow in the rendering loop
        //to better split the CPU commands (Physics, AI etc.) from the GPU(rendering) stuff
        //example: physics and collission at beginning of each frame in notifyBeginFrame(), 
        //and after that we can draw everything in notifyDisplayFrame() and do other stuff in notifyEndFrame()
        void NotifyUpdateFrame();
        void NotifyDisplayFrame();
        void NotifyEndFrame();
        void NotifyReshape();
    }
}
