using System;
using System.Collections.Generic;
using BasicEngine.Rendering;
using BasicEngine.Object;

namespace BasicEngine.Managers
{
    public class ModelsManager : IDisposable
    {
        private Dictionary<int, IGameObject> gameModelDic = null;

        public ModelsManager()
        {
            if (gameModelDic == null) gameModelDic = new Dictionary<int, IGameObject>();
            //Create the default Shader
            Utility.ShaderLoader.LoadFromString("DefaultShader", @"..\..\Shaders\vertexDefault.glsl", @"..\..\Shaders\fragmentDefault.glsl");

            Model mod = new Model("rabbit.obj");
            gameModelDic.Add(mod.InstanceID, mod);
        }

        public void Dispose()
        {
            foreach(IGameObject go in gameModelDic.Values)
            {
                go.Dispose();
            }

            gameModelDic.Clear();
        }

        public void Draw()
        {
            foreach(IGameObject go in gameModelDic.Values)
            {
                go.Draw();
            }
        }

        public void Update()
        {
            foreach (IGameObject go in gameModelDic.Values)
            {
                go.Update();
            }
        }

        public void AddModel(int id, IGameObject model)
        {
            if (gameModelDic.ContainsKey(id))
            {
                Console.WriteLine("ModelsManager.AddModel: Model " + id + " does already exist.");
            }
            else
            {
                gameModelDic.Add(id, model);
            }
        }

        public void DeleteModel(int id)
        {
            if(gameModelDic.ContainsKey(id))
            {
                IGameObject model = gameModelDic[id];
                model.Dispose();
                gameModelDic.Remove(id);
            }
            else
            {
                Console.WriteLine("ModelsManager.DeleteModel: Model " + id + " does not exist.");
            }
        }

        IGameObject GetModel(int id)
        {
            if (gameModelDic.ContainsKey(id))
            {
                return gameModelDic[id];
            }
            else
            {
                Console.WriteLine("ModelsManager.GetModel: Model " + id + " does not exist.");
                return null;
            }
        }
    }
}
