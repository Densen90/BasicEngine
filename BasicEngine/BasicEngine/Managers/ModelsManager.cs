using System;
using System.Collections.Generic;
using BasicEngine.Rendering;
using BasicEngine.Object;

namespace BasicEngine.Managers
{
    class ModelsManager : IDisposable
    {
        private Dictionary<string, IGameObject> gameModelDic = null;

        public ModelsManager()
        {
            if (gameModelDic == null) gameModelDic = new Dictionary<string, IGameObject>();
            //Create the default Shader
            Utility.ShaderLoader.LoadFromString("DefaultShader", @"..\..\Shaders\vertexDefault.glsl", @"..\..\Shaders\fragmentDefault.glsl");

            Model mod = new Model("rabbit.obj");
            gameModelDic.Add("mod", mod);
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

        public void DeleteModel(string modelName)
        {
            if(gameModelDic.ContainsKey(modelName))
            {
                IGameObject model = gameModelDic[modelName];
                model.Dispose();
                gameModelDic.Remove(modelName);
            }
            else
            {
                Console.WriteLine("ModelsManager.DeleteModel: Model " + modelName + " does not exist.");
            }
        }

        IGameObject GetModel(string modelName)
        {
            if (gameModelDic.ContainsKey(modelName))
            {
                return gameModelDic[modelName];
            }
            else
            {
                Console.WriteLine("ModelsManager.GetModel: Model " + modelName + " does not exist.");
                return null;
            }
        }
    }
}
