using UnityEngine;

public class MiniGamePrefabView : AbstractMiniGameTypeView
{
    [Header("Additional data")]
    [SerializeField] private Transform spawnPoint;

    protected override void Prepare(AbstractMiniGameData data) 
    {
        AbstractMiniGameView view = Instantiate(MiniGameView, spawnPoint);
        view.Render(data);
    }
}