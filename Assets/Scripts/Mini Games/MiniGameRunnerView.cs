using UnityEngine;

public class MiniGameRunnerView : AbstractMiniGameView
{
    [SerializeField] private MiniGameRunnerData data;
    protected override AbstractMiniGameData Data { get => data; set => data = value as MiniGameRunnerData; }

    protected override void OnRender() 
    {
        
    }
}