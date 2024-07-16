using UnityEngine;

public abstract class AbstractMiniGameTypeView : MonoBehaviour
{
    [Header("Base data")]
    [SerializeField, SerializeReference] protected AbstractMiniGameView MiniGameView;

    public void Create(AbstractMiniGameData data) 
    {
        if (data != MiniGameView.MiniGameData) return;

        Prepare(data);
    }

    protected abstract void Prepare(AbstractMiniGameData data);
}