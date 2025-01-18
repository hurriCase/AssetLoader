using System;

namespace AssetLoader.Runtime
{
    public sealed class ResourceAttribute : Attribute
    {
        public string AssetPath { get; }
        public string ResourcePath { get; }
        public string Name { get; }

        public ResourceAttribute(string assetPath = "", string name = "", string resourcePath = "")
        {
            AssetPath = assetPath;
            Name = name;
            ResourcePath = resourcePath;
        }

        public bool TryGetFullResourcePath(out string fullResourcePath)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                fullResourcePath = null;
                return false;
            }

            fullResourcePath = string.IsNullOrWhiteSpace(ResourcePath) ? Name : $"{ResourcePath}/{Name}";
            return true;
        }
    }
}