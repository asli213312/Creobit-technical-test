using UnityEngine;

public abstract class AbstractMiniGameView : MonoBehaviour
{
    public AbstractMiniGameData MiniGameData => Data;
    protected abstract AbstractMiniGameData Data { get; set; }

    public void Render(AbstractMiniGameData data) 
    {
        if (data != Data) 
        {
            Debug.LogError("MiniGame view incorrect data to render! object name:" + name, gameObject);
            return;
        }

        OnRender();
    }

    protected abstract void OnRender();
}