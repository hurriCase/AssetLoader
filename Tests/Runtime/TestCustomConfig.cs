using AssetLoader.Runtime.Config;

namespace AssetLoader.Tests.Runtime
{
    internal sealed class TestCustomConfig : IAssetLoaderConfig
    {
        public string DontDestroyPath => TestsConfig.TestDontDestroyOnLoad;
    }
}