using System;
using System.Collections.Generic;

namespace BasicEngine.Object
{
    interface IGameObject : IDisposable
    {
        void Draw();
        void Update();
    }
}
