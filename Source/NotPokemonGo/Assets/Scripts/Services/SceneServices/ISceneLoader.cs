using System;

namespace Services.SceneServices
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
    }
}