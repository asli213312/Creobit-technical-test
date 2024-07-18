using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
    [SerializeField] private HorizontalLayoutGroup assetsPreviewLayout;
    [SerializeField] private Image assetPreviewPrefab;
    [SerializeField] private AbstractMiniGameTypeView miniGameTypeView;

    public event Action OnSuccessLoad;
    public event Action OnFailedLoad;
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
        unloadDataButton.onClick.RemoveListener(UnloadData);
    }

    private IEnumerator LoadData() 
    {
        loadDataButton.interactable = false;

        string bundleUrl = $"{Constants.SERVER_RESOURCES_PATH}{miniGameTypeView.GameView.MiniGameData.assetBundleName}";
        string bundleName = $"{miniGameTypeView.GameView.MiniGameData.assetBundleName}";

        yield return AssetBundleManager.Instance.LoadAssetBundle(bundleUrl, bundleName, 
            bundle =>
            {
                StartCoroutine(AssetBundleManager.Instance.LoadAsyncAssetsFromBundle(bundleName, miniGameTypeView.GameView.AssetContainers.Select(x => x.name).ToArray(),
                loadedAssets =>
                {
                    if (loadedAssets != null)
                    {
                        _loadedAssets = loadedAssets;
                        miniGameTypeView.LoadAssets(_loadedAssets);
                        CheckStatusButtonLoad(true);
                        RenderLoadedAssetsPreview();
                        OnSuccessLoad?.Invoke();
                    }
                    else
                    {
                        OnFailedLoad?.Invoke();
                    }
                },
                error => 
                {
                    try 
                    {
                        throw error;
                    }
                    catch (AssetLoadException e)
                    {
                        Debug.LogError("Loading assets failed: " + e.Message);
                        CheckStatusButtonLoad(false);
                        OnFailedLoad?.Invoke();
                        return;
                    }
                    catch (Exception e) 
                    {
                        Debug.LogError("Occured unknown error: " + e.Message);
                        CheckStatusButtonLoad(false);
                        OnFailedLoad?.Invoke();
                        return;
                    }
                }
                ));
            }, 
            error => 
            {
                try 
                {
                    throw error;
                }
                catch (AssetLoadException e)
                {
                    Debug.LogError("Loading assetBundle failed: " + e.Message);
                    OnFailedLoad?.Invoke();
                    return;
                }
                catch (Exception e) 
                {
                    Debug.LogError("Occured unknown error: " + e.Message);
                    OnFailedLoad?.Invoke();
                    return;
                }
                finally 
                {
                    loadDataButton.interactable = true;
                }
            }
        );
    }

    private void UnloadData() 
    {
        AssetBundleManager.Instance.UnloadAssetBundle(miniGameTypeView.GameView.MiniGameData.assetBundleName, true, (isUnloaded) => 
            {
                if (isUnloaded) 
                {
                    Debug.Log($"Minigame data for: {name} is unloaded!");
                    ClearExistsAssetsPreview();
                }
                else
                    Debug.LogError($"Minigame data for: {name} failed to unload!");

                CheckStatusButtonUnload(isUnloaded);
            });
    }

    private void RenderLoadedAssetsPreview() 
    {
        foreach (var asset in _loadedAssets)
        {
            #if UNITY_EDITOR 

            Image assetPreview = Instantiate(assetPreviewPrefab, assetsPreviewLayout.transform);
            assetPreview.sprite = UnityEditor.AssetPreview.GetAssetPreview(asset).ConvertToSprite();

            #endif
        }
    }

    private void ClearExistsAssetsPreview() 
    {
        if (assetsPreviewLayout.transform.childCount > 0) 
        {
            for (int i = 0; i < assetsPreviewLayout.transform.childCount; i++)
            {
                GameObject assetPreviewObj = assetsPreviewLayout.transform.GetChild(i).gameObject;
                Destroy(assetPreviewObj);
            }
        }
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