using AssetLoader.Runtime;
using UnityEngine;

namespace AssetLoader.Tests.Runtime
{
    [Resource(name: "CreatedTestScriptableObject", resourcePath: "Configs")]
    internal sealed class CreatedTestScriptableObject : ScriptableObject
    {
        internal string TestString => TestsConfig.TestString;
    }
}