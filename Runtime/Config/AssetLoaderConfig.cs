﻿using UnityEngine;

namespace AssetLoader.Runtime.Config
{
    [Resource(name: "AssetLoaderConfig")]
    internal sealed class AssetLoaderConfig : ScriptableObject, IAssetLoaderConfig
    {
        internal static AssetLoaderConfig Instance => _instance ?? (_instance = ResourceLoader<AssetLoaderConfig>.Load());
        private static AssetLoaderConfig _instance;

        [field: SerializeField] public string DontDestroyPath { get; private set; } = "DontDestroyOnLoad";
    }
}