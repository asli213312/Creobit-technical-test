using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetLoadException : Exception
{
    public AssetLoadException(string message) : base(message) { }
    public AssetLoadException(string message, Exception innerException) : base(message, innerException) { }
}

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

    public IEnumerator LoadAssetBundle(string bundleUrl, string bundleName, Action<AssetBundle> onComplete, Action<Exception> onError)
    {
        Debug.Log($"Start loading AssetBundle: {bundleName} from {bundleUrl}");

        Exception caughtException = null;

        if (_loadedBundles.ContainsKey(bundleName))
        {
            caughtException = new AssetLoadException($"AssetBundle {bundleName} already loaded.");
            onError?.Invoke(caughtException);
            throw caughtException;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            caughtException = new AssetLoadException("No internet connection available.");
            onError?.Invoke(caughtException);
            throw caughtException;
        }

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                caughtException = new AssetLoadException($"Failed to load AssetBundle: {www.error}");
                onError?.Invoke(caughtException);
                throw caughtException;
            }

            try
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                if (bundle == null)
                {
                    throw new AssetLoadException($"Failed to get AssetBundle content from {bundleUrl}");
                }

                _loadedBundles[bundleName] = bundle;
                Debug.Log($"Successfully loaded AssetBundle: {bundleName}");
                onComplete?.Invoke(bundle);
            }
            catch (Exception e)
            {
                caughtException = new Exception($"Exception during loading AssetBundle: {e.Message}");
                onError?.Invoke(caughtException);
            }
        }
    }

    public IEnumerator LoadAsyncAssetsFromBundle(string bundleName, string[] assetNames, Action<List<UnityEngine.Object>> onComplete, Action<Exception> onError)
    {
        Exception caughtException = null;

        if (!_loadedBundles.ContainsKey(bundleName))
        {
            caughtException = new AssetLoadException($"AssetBundle {bundleName} not loaded.");
            throw caughtException;
        }

        AssetBundle bundle = _loadedBundles[bundleName];
        List<UnityEngine.Object> loadedAssets = new List<UnityEngine.Object>();

        foreach (var assetName in assetNames)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync<UnityEngine.Object>(assetName);
            yield return request;

            try
            {
                if (request.asset != null)
                {
                    loadedAssets.Add(request.asset as UnityEngine.Object);
                }
                else
                {
                    throw new AssetLoadException($"Failed to load asset: {assetName} from bundle: {bundleName}");
                }
            }
            catch (Exception e)
            {
                caughtException = new AssetLoadException($"Exception during loading asset: {assetName} from bundle: {bundleName}", e);
                onError?.Invoke(caughtException);
            }
        }

        onComplete?.Invoke(loadedAssets);

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