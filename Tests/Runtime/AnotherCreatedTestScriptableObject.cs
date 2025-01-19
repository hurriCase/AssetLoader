using AssetLoader.Runtime;
using UnityEngine;

namespace AssetLoader.Tests.Runtime
{
    [Resource(name: "AnotherCreatedTestScriptableObject", resourcePath: "Configs")]
    internal sealed class AnotherCreatedTestScriptableObject : ScriptableObject
    {
        internal string AnotherTestString => TestsConfig.AnotherTestString;
    }
}