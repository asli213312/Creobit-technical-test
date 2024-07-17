using UnityEngine;
using UnityEditor;

public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles for Windows")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            BuildTarget.StandaloneWindows);
    }
}