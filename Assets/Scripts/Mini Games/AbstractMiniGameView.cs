using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMiniGameView : MonoBehaviour
{
    [SerializeField] private string[] assets;
    public string[] Assets => assets;
    public AbstractMiniGameData MiniGameData => Data;
    protected abstract AbstractMiniGameData Data { get; set; }
    protected List<UnityEngine.Object> LoadedAssets = new();

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
            UnityEngine.Object requestedAsset = null;
            
            foreach (var assetName in Assets)
            {
                if (assetName != asset.name) continue;

                requestedAsset = asset;
                Debug.Log("Asset founded! for: " + name, gameObject);
                Debug.Log("Asset name: " + requestedAsset.name);
                Debug.Log("Asset type: " + requestedAsset.GetType());
            }

            if (requestedAsset == null) 
            {
                Debug.LogError("Couldn't find from loaded assets requested asset");
                break;
            }

            switch(requestedAsset) 
            {
                case Texture2D:
                    Texture2D texture = requestedAsset as Texture2D;
                
                    Sprite selectedSprite = Sprite.Create(
                        texture, 
                        new Rect(0, 0, texture.width, texture.height), 
                        new Vector2(0.5f, 0.5f)
                    );

                    return selectedSprite as T;
                default: 
                    return requestedAsset as T;
            }
        }

        Debug.LogError("Couldn't find from loaded assets requested asset");
        return null;
    }
}