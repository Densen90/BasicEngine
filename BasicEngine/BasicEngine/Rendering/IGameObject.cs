using System;
using System.Collections.Generic;

namespace BasicEngine.Rendering
{
    interface IGameObject : IDisposable
    {
        void Draw();
        void Update();
    }
}
