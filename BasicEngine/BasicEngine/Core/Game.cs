using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BasicEngine.Core
{
    class Game : GameWindow
    {
        private IListener listener = null;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if(listener!=null)
            {
                listener.NotifyDisplayFrame();
                this.SwapBuffers();
                listener.NotifyEndFrame();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (listener != null)
            {
                listener.NotifyReshape();
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if(listener!=null)
            {
                listener.NotifyUpdateFrame();
            }
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (listener != null)
            {
                listener.Dispose();
            }
        }

        public void SetListener(IListener listener)
        {
            this.listener = listener;
        }
    }
}
