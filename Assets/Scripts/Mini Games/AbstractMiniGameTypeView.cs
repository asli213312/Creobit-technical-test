using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractMiniGameTypeView : MonoBehaviour
{
    [Header("Base data")]
    [SerializeField, SerializeReference] protected AbstractMiniGameView MiniGameView;

    public AbstractMiniGameView GameView => MiniGameView;
    public AbstractMiniGameView GameInstance { get; protected set; }

    protected List<Object> LoadedAssets = new();

    public void LoadAssets(List<Object> assets) 
    {
        LoadedAssets = assets;

        CheckLoadedAssets();
    }

    public void Create(MiniGameServices services) 
    {
        Prepare(MiniGameView, (miniGameView) => 
            {
                miniGameView.Initialize(services);
            });
    }

    protected abstract void Prepare(AbstractMiniGameView miniGameView, System.Action<AbstractMiniGameView> onCreated);

    private void CheckLoadedAssets() 
    {
        if (LoadedAssets.Count <= 0) 
        {
            Debug.LogError("Couldn't check loaded assets, list is null!", gameObject);
            return;
        }

        foreach (var loadedAsset in LoadedAssets)
        {
            bool found = false;

            foreach (var assetName in MiniGameView.AssetContainers.Select(x => x.name).ToArray())
            {
                if (loadedAsset.name != assetName) continue;

                found = true;
            }

            if (found == false)
                Debug.LogError("Couldn't find asset: " + loadedAsset.name);
        }

        Debug.Log("Loaded assets corrected! for " + MiniGameView.name, gameObject);
    }
}