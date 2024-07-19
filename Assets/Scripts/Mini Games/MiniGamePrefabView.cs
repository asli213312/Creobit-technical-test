using UnityEngine;

public class MiniGamePrefabView : AbstractMiniGameTypeView
{
    #region Serialized Fields
    [Header("Additional data")]
    [SerializeField] private Transform spawnPoint;

    #endregion Serialized Fields

    #region Protected Methods
    protected override void Prepare(AbstractMiniGameView miniGameView, System.Action<AbstractMiniGameView> onCreated) 
    {
        AbstractMiniGameView view = Instantiate(miniGameView, spawnPoint);

        onCreated?.Invoke(view);

        view.Render(miniGameView.MiniGameData, LoadedAssets);

        GameInstance = view;
    }

    #endregion Protected Methods
}