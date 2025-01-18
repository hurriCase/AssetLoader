# Unity Asset Loader Package

A type-safe resource loading system for Unity that provides attribute-based path configuration, resource caching, and automated DontDestroyOnLoad object management.

## Quick Start

1. **Load Resources**
```csharp
// Load a single resource
var prefab = ResourceLoader<MyComponent>.Load();

// Load all prefabs in a folder
var prefabs = ResourceLoader<GameObject>.LoadAll("Prefabs/UI");
```

2. **Configure Resource Paths**
```csharp
// Basic usage - loads from Resources/MyPrefab
[Resource(name: "MyPrefab")]
public class MyComponent : MonoBehaviour { }

// Custom path - loads from Resources/Prefabs/UI/MyPrefab 
[Resource(resourcePath: "Prefabs/UI", name: "MyPrefab")]
public class UIComponent : MonoBehaviour { }
```

3. **Setup Persistent Objects**
```csharp
// Mark objects to persist between scenes
public class MyManager : MonoBehaviour 
{
    private void Awake()
    {
        gameObject.AddComponent<DontDestroyOnLoadComponent>();
    }
}
```

## Core Features

- Attribute-based resource path configuration
- Type-safe resource loading and caching
- Automated DontDestroyOnLoad object management
- Configurable paths and validation
- Error handling and logging

## Essential Configuration

### DontDestroyOnLoad Path

Change the default persistent objects path (`Resources/DontDestroyOnLoad`):

1. **Using ScriptableObject**:
```csharp
[CreateAssetMenu(fileName = "AssetLoaderConfig", menuName = "AssetLoader/Config")]
internal sealed class AssetLoaderConfig : ScriptableObject, IAssetLoaderConfig 
{
    [SerializeField] private string _dontDestroyPath = "CustomPath/Persistent";
    public string DontDestroyPath => _dontDestroyPath;
}
```

2. **Using Runtime Config**:
```csharp
[Resource("Assets/Resource/DontDestroyOnLoad", "AssetLoaderConfig", "DontDestroyOnLoad")]
internal sealed class AssetLoaderConfig : ScriptableSingleton<AssetLoaderConfig>, IAssetLoaderConfig
{
    public string DontDestroyPath => "DontDestroyOnLoad";
}

// No manual initialization needed - automatically handled by ScriptableSingleton
```

### Resource Loading

```csharp
// With error handling
if (ResourceLoader<MyComponent>.TryLoad(out var component)) {
    // Use component
}

// Load multiple
if (ResourceLoader<GameObject>.TryLoadAll("Prefabs/UI", out var prefabs)) {
    foreach (var prefab in prefabs) {
        // Use prefab
    }
}
```

## Best Practices

1. **Resource Organization**
    - Group related resources in dedicated folders
    - Use clear, descriptive names
    - Keep persistent object count minimal

2. **Error Handling**
    - Use TryLoad/TryLoadAll for safer loading
    - Handle missing resources gracefully
    - Check logs for validation errors

3. **Performance**
    - Cache frequently accessed resources
    - Clear cache when reloading assets
    - Use ResourceAttribute for path optimization

## Common Issues & Solutions

1. **Missing Resources**
    - Verify path in ResourceAttribute
    - Check Resources folder structure
    - Ensure prefabs are marked as Resources

2. **Duplicate Persistent Objects**
    - Use unique names for persistent prefabs
    - Check for duplicate DontDestroyOnLoadComponent
    - Verify initialization order

3. **Path Validation**
    - Avoid parent directory traversal (`..`)
    - Use forward slashes for paths
    - Keep paths relative to Resources folder

## Technical Details

### Resource Loading Pipeline

1. Check ResourceAttribute on type
2. Resolve and validate path
3. Load from Resources folder
4. Cache for future use

### Caching System

- Type and path-based cache
- Separate single/array resource caches
- Automatic cache management
- Manual cache clearing available

## API Reference

### ResourceLoader<T>
```csharp
public static class ResourceLoader<T> where T : Object
{
    public static T Load();
    public static bool TryLoad(out T resource);
    public static T[] LoadAll(string path);
    public static bool TryLoadAll(string path, out T[] resources);
    public static void ClearCache();
    public static void RemoveFromCache();
}
```

### ResourceAttribute
```csharp
public sealed class ResourceAttribute : Attribute
{
    public ResourceAttribute(
        string assetPath = "", 
        string name = "", 
        string resourcePath = ""
    );
}
```

### IAssetLoaderConfig
```csharp
public interface IAssetLoaderConfig
{
    string DontDestroyPath { get; }
}
```

## Advanced Usage

### Custom Resource Resolution
```csharp
// Custom path provider
public class CustomPathProvider : IResourcePathProvider 
{
    public string GetPath(Type type) => $"Custom/{type.Name}";
}

// Register provider
ResourceLoader.SetPathProvider(new CustomPathProvider());
```

### Manual Cache Management
```csharp
// Clear specific type
ResourceLoader<MyComponent>.ClearCache();

// Remove single entry
ResourceLoader<MyComponent>.RemoveFromCache();
```

### Async Loading
```csharp
// Load resource asynchronously
var request = ResourceLoader<GameObject>.LoadAsync("Prefabs/Large");
yield return request;
var prefab = request.asset;
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

This package is licensed under the MIT License - see the LICENSE file for details.