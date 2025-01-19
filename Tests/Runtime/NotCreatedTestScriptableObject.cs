using AssetLoader.Runtime;
using UnityEngine;

namespace AssetLoader.Tests.Runtime
{
    [Resource(name: TestsConfig.NotCreatedTestScriptableObjectName, resourcePath: TestsConfig.ConfigsPath)]
    internal sealed class NotCreatedTestScriptableObject : ScriptableObject
    {
        internal string TestString => TestsConfig.TestString;
    }
}