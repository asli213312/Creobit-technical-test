using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class MiniGamesRendererModule : MonoBehaviour, IMiniGamesRendererModule
{
    #region Serialized Fields
    [SerializeField] private MiniGameLaunchButton[] miniGameButtons;
    [SerializeField] private Button closeGameButton;

    [Header("Events")]
    [SerializeField] private UnityEvent onSuccessLoadMiniGame;
    [SerializeField] private UnityEvent onFailedLoadMiniGame;
    [SerializeField] private UnityEvent onSpawnMiniGame;
    [SerializeField] private UnityEvent onCloseMiniGame;

    #endregion Serialized Fields

    #region Private Fields
    private Action<AbstractMiniGameData> OnSelectMiniGame;
    private AbstractMiniGameTypeView _currentMiniGame;
    private MiniGamesSystem _system;
    private AssetResolver _assetResolver;
    #endregion Private Fields

    #region UnityLoop Events
    private void Awake() 
    {
        foreach (var item in miniGameButtons)
        {
            item.OnClick += RenderSelectedMiniGame;
            item.OnSuccessLoad += () => onSuccessLoadMiniGame?.Invoke();
            item.OnFailedLoad += () => onFailedLoadMiniGame?.Invoke();
        }

        closeGameButton.onClick.AddListener(CloseCurrentMiniGame);

        _assetResolver = new AssetResolver();
    }

    private void OnDestroy()
    {
        foreach (var item in miniGameButtons)
        {
            item.OnClick -= RenderSelectedMiniGame;
            item.OnSuccessLoad -= () => onSuccessLoadMiniGame?.Invoke();
            item.OnFailedLoad -= () => onFailedLoadMiniGame?.Invoke();
        }

        closeGameButton.onClick.RemoveListener(CloseCurrentMiniGame);

        _assetResolver = null;
    }
    #endregion UnityLoop Events

    #region Public Methods
    public void InitializeCore(MiniGamesSystem system) 
    {
        _system = system;
    }

    public void RenderSelectedMiniGame(AbstractMiniGameTypeView view) 
    {
        view.Create(new MiniGameServices(_assetResolver));

        _currentMiniGame = view;
        TryResolveAutomaticallyAssets();

        onSpawnMiniGame?.Invoke();
    }
    #endregion Public Methods

    #region Private Methods
    private void TryResolveAutomaticallyAssets()
    {
        foreach (var asset in _currentMiniGame.GameInstance.LoadedAssets)
        {
            AssetContainer selectedAssetContainer = null;

            foreach (var assetContainer in _currentMiniGame.GameInstance.AssetContainers)
            {
                if (assetContainer.name == asset.name)
                {
                    selectedAssetContainer = assetContainer;
                    Debug.Log("Asset founded! for: " + name, gameObject);
                    Debug.Log("Asset name: " + asset.name);
                    Debug.Log("Asset type: " + asset.GetType());
                    break;
                }
            }

            if (selectedAssetContainer == null)
            {
                Debug.LogError("Asset container not found for loaded asset: " + asset.name);
                continue;
            }

            ApplyAsset(selectedAssetContainer, asset);
        }
    }

    private void ApplyAsset(AssetContainer assetContainer, UnityEngine.Object asset)
    {
        switch (assetContainer.type)
        {
            case AssetType.Sprite:
                if (asset is Texture2D texture)
                {
                    Sprite selectedSprite = _assetResolver.ResolveAsset<Sprite>(assetContainer.type, asset);
                    if (assetContainer.targetObj.TryGetComponent(out UnityEngine.UI.Image targetImg))
                    {
                        targetImg.sprite = selectedSprite;
                    }
                }
                break;

            case AssetType.Material:
                if (asset is Material material)
                {
                    if (assetContainer.targetObj.TryGetComponent(out SkinnedMeshRenderer skinnedMesh))
                    {
                        skinnedMesh.material = material;
                    }
                    else if (assetContainer.targetObj.TryGetComponent(out MeshRenderer meshRenderer))
                    {
                        meshRenderer.material = material;
                    }
                }
                break;

            case AssetType.Texture:
                if (asset is Texture2D tex)
                {
                    if (assetContainer.targetObj.TryGetComponent(out SkinnedMeshRenderer skinnedMeshTex))
                    {
                        if (skinnedMeshTex.material != null)
                            skinnedMeshTex.material.mainTexture = tex;
                    }
                    else if (assetContainer.targetObj.TryGetComponent(out MeshRenderer meshRendererTex))
                    {
                        if (meshRendererTex.material != null)
                            meshRendererTex.material.mainTexture = tex;
                    }
                }
                break;

            case AssetType.Mesh:
                if (asset is not GameObject assetGO) return;

                if (assetGO.gameObject.TryGetComponent(out SkinnedMeshRenderer assetSkinRenderer)) 
                {
                    if (assetContainer.targetObj.TryGetComponent(out SkinnedMeshRenderer skinnedMeshMesh))
                    {
                        skinnedMeshMesh.sharedMesh = assetSkinRenderer.sharedMesh;
                    }
                }
                else if (assetGO.TryGetComponent(out MeshFilter assetMeshFilter))
                {
                    if (assetContainer.targetObj.TryGetComponent(out MeshFilter meshFilter))
                    {
                        meshFilter.sharedMesh = assetMeshFilter.sharedMesh;
                    }
                }
                break;
        }
    }

    private void CloseCurrentMiniGame() 
    {
        if (_currentMiniGame.GameInstance != null)
            Destroy(_currentMiniGame.GameInstance.gameObject);

        foreach (var gameButton in miniGameButtons)
        {
            this.Activate(gameButton.transform);
        }

        _currentMiniGame = null;
        onCloseMiniGame?.Invoke();
    }

    #endregion Private Methods
}