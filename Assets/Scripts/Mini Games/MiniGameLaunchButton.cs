using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MiniGamePrefabView))]
[RequireComponent(typeof(MiniGameSceneView))]
[RequireComponent(typeof(Button))]
public class MiniGameLaunchButton : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private Button loadDataButton;
    [SerializeField] private Button unloadDataButton;
    [SerializeField] private AbstractMiniGameTypeView miniGameTypeView;

    public event Action<AbstractMiniGameTypeView> OnClick;

    private List<UnityEngine.Object> _loadedAssets = new();

    private void Awake() 
    {
        selectButton.onClick.AddListener(() => OnClick?.Invoke(miniGameTypeView));
        loadDataButton.onClick.AddListener(() => StartCoroutine(LoadData()));
        unloadDataButton.onClick.AddListener(UnloadData);
    }

    private void Start() 
    {
        if (miniGameTypeView.GameView.MiniGameData.useSceneView) 
        {
            if (TryGetComponent(out MiniGameSceneView miniGameSceneView))
                miniGameTypeView = miniGameSceneView;
        }

        bool dataIsLoaded = _loadedAssets.Count > 0;
        CheckStatusButtonLoad(dataIsLoaded);
    }

    private void OnDestroy() 
    {
        selectButton.onClick.RemoveListener(() => OnClick?.Invoke(miniGameTypeView));
        loadDataButton.onClick.RemoveListener(() => StartCoroutine(LoadData()));
        unloadDataButton.onClick.AddListener(UnloadData);
    }

    private IEnumerator LoadData() 
    {
        List<UnityEngine.Object> loadedAssets = new();
        bool isSuccessful = true;

        string bundleUrl = $"{Constants.SERVER_RESOURCES_PATH}{miniGameTypeView.GameView.MiniGameData.assetBundleName}";
        string bundleName = $"{miniGameTypeView.GameView.MiniGameData.assetBundleName}";

        yield return AssetBundleManager.Instance.LoadAssetBundle(bundleUrl, bundleName, (bundle) =>
        {
            foreach (var assetName in miniGameTypeView.GameView.Assets)
            {
                UnityEngine.Object loadedAsset = bundle.LoadAsset<UnityEngine.Object>(assetName);
                if (loadedAsset != null)
                {
                    loadedAssets.Add(loadedAsset);
                }
                else
                {
                    Debug.LogError("Failed to load asset: " + assetName);
                    isSuccessful = false;
                    break;
                }   
            }

            miniGameTypeView.LoadAssets(loadedAssets);
            _loadedAssets = loadedAssets;

            CheckStatusButtonLoad(isSuccessful);
        });
    }

    private void UnloadData() 
    {
        AssetBundleManager.Instance.UnloadAssetBundle(miniGameTypeView.GameView.MiniGameData.assetBundleName, true, (isUnloaded) => 
            {
                if (isUnloaded)
                    Debug.Log($"Minigame data for: {name} is unloaded!");
                else
                    Debug.LogError($"Minigame data for: {name} failed to unload!");

                CheckStatusButtonUnload(isUnloaded);
            });
    }

    private void CheckStatusButtonUnload(bool isSuccessful) 
    {
        if (isSuccessful) 
        {
            loadDataButton.interactable = true;
            selectButton.interactable = false;
            unloadDataButton.interactable = false;
            _loadedAssets.Clear();
        }
        else 
        {
            selectButton.interactable = false;
            loadDataButton.interactable = false;
            unloadDataButton.interactable = true;
        }
    }

    private void CheckStatusButtonLoad(bool isSuccessful) 
    {
        if (isSuccessful) 
        {
            loadDataButton.interactable = true;
            selectButton.interactable = true;
        }
        else 
        {
            loadDataButton.interactable = false;
            selectButton.interactable = false;
        }

        if (_loadedAssets.Count > 0) 
        {
            unloadDataButton.interactable = true;
            loadDataButton.interactable = false;
        }
        else
        {
            unloadDataButton.interactable = false;
            loadDataButton.interactable = true;
        }
    }
}