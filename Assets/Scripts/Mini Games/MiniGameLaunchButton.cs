using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameLaunchButton : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private AbstractMiniGameData miniGameData;

    public Action<AbstractMiniGameData> OnClick;

    private void Awake() 
    {
        selectButton.onClick.AddListener(() => OnClick?.Invoke(miniGameData));
    }

    private void OnDestroy() 
    {
        selectButton.onClick.RemoveListener(() => OnClick?.Invoke(miniGameData));
    }
}