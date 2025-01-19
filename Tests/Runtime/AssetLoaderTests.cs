using System.Collections;
using System.Linq;
using AssetLoader.Runtime;
using AssetLoader.Runtime.Config;
using AssetLoader.Runtime.DontDestroyLoader;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AssetLoader.Tests.Runtime
{
    internal sealed class AssetLoaderPlayModeTests
    {
        private GameObject _testObject;

        [OneTimeSetUp]
        public void SetUp()
        {
            var testCustomConfig = new TestCustomConfig();
            AssetLoaderInitializer.Init(testCustomConfig);
            AutoPersistent.MakePersistent();

            ResourceLoader<GameObject>.ClearCache();
            ResourceLoader<AssetLoaderConfig>.ClearCache();
        }

        [UnityTest]
        public IEnumerator DontDestroyOnLoadComponent_PutObjectOnDOntDestroyOnLoadScene()
        {
            // Act
            yield return new WaitForSeconds(0.1f); // Wait for test scene loading

            // Assert
            var persistentObject = GameObject.Find(TestsConfig.DontDestroyOnLoadTestObject);
            Assert.That(persistentObject, Is.Not.Null);
            Assert.That(persistentObject.GetComponent<DontDestroyOnLoadComponent>(), Is.Not.Null);
            Assert.That(persistentObject.scene.buildIndex, Is.EqualTo(-1)); // Index of DontDestroyOnLoad scene
            Assert.That(persistentObject.scene.name, Is.EqualTo("DontDestroyOnLoad")); // Name of DontDestroyOnLoad scene
        }

        [UnityTest]
        public IEnumerator ResourceLoader_LoadsResourcesInPlayMode()
        {
            // Arrange & Act
            var config = AssetLoaderConfig.Instance;
            yield return null;

            // Assert
            Assert.That(config, Is.Not.Null);
            Assert.That(config.DontDestroyPath, Is.Not.Empty);
        }

        [UnityTest]
        public IEnumerator CustomConfig_OverridesDefaultConfig()
        {
            // Arrange
            var customConfig = new TestCustomConfig();

            // Act
            AssetLoaderInitializer.Init(customConfig);
            yield return null;

            // Assert
            Assert.That(AssetLoaderInitializer.LoaderConfig.DontDestroyPath, Is.EqualTo(TestsConfig.CustomPath));
        }

        [UnityTest]
        public IEnumerator ResourceLoader_LoadConfigFromResourceFolderWithExplicitPath_ShouldLoadCorrectObject()
        {
            // Act
            var testScriptableObject = ResourceLoader<CreatedTestScriptableObject>.Load(TestsConfig.ExplicitPath);
            yield return null;

            // Assert
            Assert.That(testScriptableObject, Is.Not.Null);
            Assert.That(testScriptableObject.TestString, Is.EqualTo(TestsConfig.TestString));
        }

        [UnityTest]
        public IEnumerator ResourceLoader_LoadConfigFromResourceFolder_ShouldLoadCorrectObject()
        {
            // Act
            var testScriptableObject = ResourceLoader<CreatedTestScriptableObject>.Load();
            yield return null;

            // Assert
            Assert.That(testScriptableObject, Is.Not.Null);
            Assert.That(testScriptableObject.TestString, Is.EqualTo(TestsConfig.TestString));
        }

        [UnityTest]
        public IEnumerator ResourceLoader_TryLoadConfigFromResourceFolder_ShouldLoadCorrectObjectAndReturnTrue()
        {
            // Act
            var result = ResourceLoader<CreatedTestScriptableObject>.TryLoad(out var testScriptableObject);
            yield return null;

            // Assert
            Assert.That(result, Is.True);
            Assert.That(testScriptableObject, Is.Not.Null);
            Assert.That(testScriptableObject.TestString, Is.EqualTo(TestsConfig.TestString));
        }

        [UnityTest]
        public IEnumerator ResourceLoader_TryLoadConfigFromThatNotCreated_ShouldNotLoadObjectAndReturnFalse()
        {
            // Act
            var result = ResourceLoader<NotCreatedTestScriptableObject>.TryLoad(out var testScriptableObject);
            yield return null;

            // Assert
            Assert.That(result, Is.False);
            Assert.That(testScriptableObject, Is.Null);
        }

        [UnityTest]
        public IEnumerator ResourceLoader_LoadConfigFromThatNotCreated_ShouldThrowWarningThatResourceDontExist()
        {
            // Act
            var testScriptableObject = ResourceLoader<NotCreatedTestScriptableObject>.Load();
            yield return null;

            LogAssert.Expect(LogType.Warning, TestsConfig.NotFoundResourceLogWarning);

            // Assert
            Assert.That(testScriptableObject, Is.Null);
        }

        [UnityTest]
        public IEnumerator ResourceLoader_LoadAllConfigsFromResourceFolder_ShouldLoadAllObjects()
        {
            // Act
            var scriptableObjects = ResourceLoader<ScriptableObject>.LoadAll("Configs");
            yield return null;

            // Assert
            Assert.That(scriptableObjects, Is.Not.Null);
            Assert.That(scriptableObjects.Length, Is.EqualTo(2));

            var createdObject =
                scriptableObjects.FirstOrDefault(x => x is CreatedTestScriptableObject) as CreatedTestScriptableObject;
            var anotherCreatedObject =
                scriptableObjects.FirstOrDefault(x => x is AnotherCreatedTestScriptableObject) as
                    AnotherCreatedTestScriptableObject;
            Assert.That(createdObject, Is.Not.Null);
            Assert.That(anotherCreatedObject, Is.Not.Null);
            Assert.That(createdObject.TestString, Is.EqualTo(TestsConfig.TestString));
            Assert.That(anotherCreatedObject.AnotherTestString, Is.EqualTo(TestsConfig.AnotherTestString));
        }
    }

    internal sealed class TestCustomConfig : IAssetLoaderConfig
    {
        public string DontDestroyPath => TestsConfig.CustomPath;
    }

    internal static class TestsConfig
    {
        internal const string DontDestroyOnLoadTestObject = "DontDestroyOnLoadTestObject";

        internal const string NotFoundResourceLogWarning =
            "[ResourceLoader::Load] Failed to load resource at path: Configs/NotCreatedTestScriptableObject";

        internal const string ExplicitPath = "Configs/CreatedTestScriptableObject";
        internal const string CustomPath = "CustomPath";
        internal const string TestString = "TestString";
        internal const string AnotherTestString = "AnotherTestString";
    }
}