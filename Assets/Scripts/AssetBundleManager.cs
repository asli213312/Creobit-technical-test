using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager Instance { get; private set; }

    private Dictionary<string, AssetBundle> _loadedBundles = new Dictionary<string, AssetBundle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoadAssetBundle(string bundleUrl, string bundleName, Action<AssetBundle> onComplete)
    {
        Debug.Log($"Start loading AssetBundle: {bundleName} from {bundleUrl}");

        if (_loadedBundles.ContainsKey(bundleName))
        {
            Debug.Log($"AssetBundle {bundleName} already loaded.");
            onComplete?.Invoke(_loadedBundles[bundleName]);
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load AssetBundle: {www.error}");
                yield break;
            }

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            
            if (bundle == null)
            {
                Debug.LogError($"Failed to get AssetBundle content from {bundleUrl}");
                yield break;
            }

            _loadedBundles[bundleName] = bundle;
            Debug.Log($"Successfully loaded AssetBundle: {bundleName}");
            onComplete?.Invoke(bundle);
        }
    }

    public T GetAsset<T>(string bundleName, string assetName) where T : UnityEngine.Object
    {
        if (_loadedBundles.ContainsKey(bundleName))
        {
            return _loadedBundles[bundleName].LoadAsset<T>(assetName);
        }

        Debug.LogError("AssetBundle not loaded: " + bundleName);
        return null;
    }

    public void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects, Action<bool> isUnloaded)
    {
        if (_loadedBundles.ContainsKey(bundleName))
        {
            _loadedBundles[bundleName].Unload(unloadAllLoadedObjects);
            _loadedBundles.Remove(bundleName);
            isUnloaded?.Invoke(true);
            return;
        }

        isUnloaded?.Invoke(false);
    }
}