using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMiniGameView : MonoBehaviour
{
    [SerializeField] private AssetContainer[] assetContainers;

    public AssetContainer[] AssetContainers => assetContainers;
    public AbstractMiniGameData MiniGameData => Data;
    public List<UnityEngine.Object> LoadedAssets { get; private set;} = new();
    protected abstract AbstractMiniGameData Data { get; set; }
    protected MiniGameServices Services { get; private set; }

    public void Initialize(MiniGameServices services) 
    {
        Services = services;
    }

    public void Render(AbstractMiniGameData data, List<UnityEngine.Object> assets) 
    {
        if (data != Data) 
        {
            Debug.LogError("MiniGame view incorrect data to render! object name:" + name, gameObject);
            return;
        }

        LoadedAssets = assets;
        OnRender(assets);
    }

    protected abstract void OnRender(List<UnityEngine.Object> assets);

    protected T GetAssetFromLoaded<T>() where T : UnityEngine.Object
    {
        foreach (var asset in LoadedAssets)
        {           
            AssetContainer selectedAssetContainer = null;

            foreach (var assetContainer in assetContainers)
            {
                if (assetContainer.name != asset.name) continue;

                selectedAssetContainer = assetContainer;
                Debug.Log("Asset founded! for: " + name, gameObject);
                Debug.Log("Asset name: " + asset.name);
                Debug.Log("Asset type: " + asset.GetType());
            }

            if (selectedAssetContainer == null) 
            {
                Debug.LogError("Couldn't find from loaded assets requested asset");
                return null;
            }

            return Services.AssetResolver.ResolveAsset<T>(selectedAssetContainer.type, asset) as T;
        }

        Debug.LogError("Couldn't find from loaded assets requested asset");
        return null;
    }
}