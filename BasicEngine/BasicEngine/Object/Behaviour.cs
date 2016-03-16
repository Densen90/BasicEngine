using BasicEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicEngine.Object
{
    class Behaviour : IDisposable
    {
        public Model GameObject;

        Behaviour()
        {
            GameObject = new Model();
            EventDispatcher.AddListener("Update", Update);
        }

        Behaviour(Model m)
        {
            GameObject = m;
            EventDispatcher.AddListener("Update", Update);
        }

        public virtual void Update() { }

        public void SetGameObject(Model m)
        {
            GameObject = m;
        }

        public void Dispose()
        {
            GameObject.Dispose();
        }
    }
}
