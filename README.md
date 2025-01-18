# Unity Asset Loader Package

A type-safe resource loading system that uses attributes to define asset paths and caches loaded resources.

## Core Components

### ResourceAttribute
Configures how a resource should be loaded:

```csharp
// Basic usage - loads from Resources/MyPrefab
[Resource(name: "MyPrefab")]
public class MyComponent : MonoBehaviour { }

// Custom resource path - loads from Resources/Prefabs/UI/MyPrefab
[Resource(resourcePath: "Prefabs/UI", name: "MyPrefab")]
public class UIComponent : MonoBehaviour { }

// Asset path for editor tools (optional)
[Resource(assetPath: "Assets/Prefabs/UI/MyPrefab.prefab", name: "MyPrefab")]
public class EditorComponent : MonoBehaviour { }
```

### ResourceLoader<T>
Generic loader that handles caching and loading of resources:

```csharp
// Load a single resource (uses path from attribute)
var prefab = ResourceLoader<MyComponent>.Load();

// Try pattern
if (ResourceLoader<MyComponent>.TryLoad(out var component)) {
    // Use component
}

// Load all resources in a path
var prefabs = ResourceLoader<GameObject>.LoadAll("Prefabs/UI");

// Clear cache if needed
ResourceLoader<MyComponent>.ClearCache();
```

## How Path Resolution Works

1. ResourceLoader checks for ResourceAttribute on type
2. If name is provided, uses that as minimum path
3. If resourcePath is provided, combines it with name
4. Falls back to type name if no attribute found

Path resolution order:
- `resourcePath/name` if both provided
- `name` if only name provided
- Type name as fallback
- Validates against parent directory traversal (`..`)

## Caching

- Resources are cached by type and path
- Single resources and resource arrays cached separately
- Cache can be:
    - Cleared completely with `ClearCache()`
    - Cleared for specific resource with `RemoveFromCache()`
- Cache key format: `TypeName:ResourcePath`

## Common Issues

- Missing ResourceAttribute: Will log warning and use type name
- Invalid path (null/empty): Will log error and return null
- Path with `..`: Will log error and return null
- Resource not found: Will log warning and return null
- Resource array not found: Will log warning and return null

## Best Practices

1. Always provide at least a name in ResourceAttribute
2. Use resourcePath for better organization
3. Clear cache when reloading assets
4. Use TryLoad when resource might not exist
5. Cache references to frequently used resources locally