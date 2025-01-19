using AssetLoader.Runtime;
using UnityEngine;

namespace AssetLoader.Tests.Runtime
{
    [Resource(name: "NotCreatedTestScriptableObject", resourcePath: "Configs")]
    internal sealed class NotCreatedTestScriptableObject : ScriptableObject
    {
        internal string TestString => TestsConfig.TestString;
    }
}