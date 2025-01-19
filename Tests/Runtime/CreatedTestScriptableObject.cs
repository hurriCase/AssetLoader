using AssetLoader.Runtime;
using UnityEngine;

namespace AssetLoader.Tests.Runtime
{
    [Resource(name: TestsConfig.CreatedTestScriptableObjectName, resourcePath: TestsConfig.ConfigsPath)]
    internal sealed class CreatedTestScriptableObject : ScriptableObject
    {
        internal string TestString => TestsConfig.TestString;
    }
}