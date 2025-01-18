using System.Collections.Generic;
using AssetLoader.Runtime.Config;
using UnityEngine;

namespace Client.Scripts.Patterns.DontDestroyLoader
{
    public sealed class AutoPersistent : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void MakePersistent()
        {
            if (ResourceLoader<GameObject>.TryLoadAll(AssetLoaderInitializer.LoaderConfig.DontDestroyPath,
                    out var dontDestroyObjects) is false)
                return;

            var instantiatedObjects = new HashSet<string>();

            foreach (var dontDestroy in dontDestroyObjects)
            {
                if (dontDestroy.TryGetComponent<DontDestroyOnLoadComponent>(out _) is false)
                    continue;

                if (instantiatedObjects.Contains(dontDestroy.name))
                    continue;

                var instance = Instantiate(dontDestroy);
                instance.name = dontDestroy.name;
                DontDestroyOnLoad(instance);
                instantiatedObjects.Add(dontDestroy.name);
            }
        }
    }
}