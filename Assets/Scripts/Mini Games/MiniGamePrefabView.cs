using UnityEngine;

public class MiniGamePrefabView : AbstractMiniGameTypeView
{
    [Header("Additional data")]
    [SerializeField] private Transform spawnPoint;

    protected override void Prepare(AbstractMiniGameView miniGameView, System.Action<AbstractMiniGameView> onCreated) 
    {
        AbstractMiniGameView view = Instantiate(miniGameView, spawnPoint);

        onCreated?.Invoke(view);

        view.Render(miniGameView.MiniGameData, LoadedAssets);

        GameInstance = view;
    }
}