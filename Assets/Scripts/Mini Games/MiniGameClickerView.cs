using UnityEngine;

public class MiniGameClickerView : AbstractMiniGameView
{
    [SerializeField] private MiniGameClickerData data;
    protected override AbstractMiniGameData Data { get => data; set => data = value as MiniGameClickerData; }

    protected override void OnRender() 
    {
        
    } 
}