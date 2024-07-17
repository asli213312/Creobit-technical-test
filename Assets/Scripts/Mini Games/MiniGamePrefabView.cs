using UnityEngine;

public class MiniGamePrefabView : AbstractMiniGameTypeView
{
    [Header("Additional data")]
    [SerializeField] private Transform spawnPoint;

    protected override void Prepare(AbstractMiniGameView miniGameView) 
    {
        AbstractMiniGameView view = Instantiate(miniGameView, spawnPoint);
        view.Render(miniGameView.MiniGameData, LoadedAssets);

        GameInstance = view;
    }
}