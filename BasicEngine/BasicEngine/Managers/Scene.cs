using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicEngine.Utility;
using BasicEngine.Object;

namespace BasicEngine.Managers
{
    class Scene : IDisposable
    {
        public Dictionary<int, Behaviour> GameObjects = null;

        public void Update()
        {
            EventDispatcher.Invoke("Update");
        }

        public void Render()
        {
            EventDispatcher.Invoke("Render");
        }

        public void Dispose()
        {
            foreach(Behaviour b in GameObjects.Values)
            {
                b.Dispose();
            }
        }
    }
}
