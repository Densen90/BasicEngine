using System;
using System.Collections.Generic;

namespace BasicEngine.Object
{
    public interface IGameObject : IDisposable
    {
        void Draw();
        void Update();
    }
}
