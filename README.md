# Asset Loader

A type-safe resource loading system for Unity that provides attribute-based path configuration, resource caching, and automated DontDestroyOnLoad object management.

## Quick Start

1. **Load Resources**
```csharp
// Load with attribute path
var resource = ResourceLoader<MyResource>.Load();

// Load with explicit path
var resource = ResourceLoader<MyResource>.Load("Configs/MyConfig");

// Try loading with error handling
if (ResourceLoader<MyResource>.TryLoad(out var resource)) {
    // Use resource
}
```

2. **Configure Resource Paths**
```csharp
// Using ResourceAttribute
[Resource(name: "MyConfig", resourcePath: "Configs")]
public class MyConfig : ScriptableObject { }
```

3. **Setup Persistent Objects**
```csharp
// Add DontDestroyOnLoadComponent to make object persistent
public class GameManager : MonoBehaviour 
{
    private void Awake()
    {
        gameObject.AddComponent<DontDestroyOnLoadComponent>();
    }
}
```

## Core Features

- Attribute-based or explicit path resource loading
- Type-safe loading with caching
- Optional path validation and error handling
- Automated DontDestroyOnLoad management
- Support for loading single resources and collections

## Essential Configuration

### Asset Loader Config

```csharp
[Resource(name: "AssetLoaderConfig")]
internal sealed class AssetLoaderConfig : ScriptableObject, IAssetLoaderConfig
{
    [field: SerializeField] 
    public string DontDestroyPath { get; private set; } = "DontDestroyOnLoad";
}
```

### Custom Configuration

```csharp
public class CustomConfig : IAssetLoaderConfig
{
    public string DontDestroyPath => "CustomPath";
}

// Apply custom config
AssetLoaderInitializer.Init(new CustomConfig());
```

## Resource Loading

```csharp
// Basic loading
var resource = ResourceLoader<MyResource>.Load();

// Explicit path loading
var resource = ResourceLoader<MyResource>.Load("Path/To/Resource");

// Loading multiple resources
var resources = ResourceLoader<ScriptableObject>.LoadAll("Configs");

// Safe loading with error handling
if (ResourceLoader<MyResource>.TryLoad(out var resource)) {
    // Use resource
}
```

## Best Practices

1. **Path Configuration**
   - Use ResourceAttribute for standard paths
   - Use explicit paths for dynamic loading
   - Keep paths relative to Resources folder

2. **Error Handling**
   - Prefer TryLoad when resource might not exist
   - Check LoadAll results for null
   - Handle missing resources gracefully

3. **Cache Management**
   - Clear cache when reloading is needed
   - Use RemoveFromCache for specific resources
   - Cache frequently accessed resources

## API Reference

### ResourceLoader<T>
```csharp
public static class ResourceLoader<T> where T : Object
{
    public static T Load(string path = null);
    public static bool TryLoad(out T resource, string path = null);
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
    public ResourceAttribute(string assetPath = "", 
                           string name = "", 
                           string resourcePath = "");
}
```

### IAssetLoaderConfig
```csharp
public interface IAssetLoaderConfig
{
    string DontDestroyPath { get; }
}
```

## Common Issues & Solutions

1. **Resource Not Found**
   - Verify resource exists in Resources folder
   - Check path matches attribute or explicit path
   - Ensure correct path format (forward slashes)

2. **Duplicate Objects**
   - Use unique names for persistent objects
   - Remove duplicate DontDestroyOnLoadComponent
   - Check initialization timing

## Technical Details

### Loading Process
1. Check for explicit path or ResourceAttribute
2. Validate path format
3. Load from Resources
4. Cache for reuse

### Caching System
- Separate caches for single and array resources
- Type and path-based cache keys
- Automatic caching on successful loads