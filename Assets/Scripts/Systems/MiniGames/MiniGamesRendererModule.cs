using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class MiniGamesRendererModule : MonoBehaviour, IMiniGamesRendererModule
{
    [SerializeField] private MiniGameLaunchButton[] miniGameButtons;
    [SerializeField] private Button closeGameButton;

    [Header("Events")]
    [SerializeField] private UnityEvent onSpawnMiniGame;
    [SerializeField] private UnityEvent onCloseMiniGame;

    private Action<AbstractMiniGameData> OnSelectedData;
    private AbstractMiniGameTypeView _currentMiniGame;
    private MiniGamesSystem _system;

    private void Awake() 
    {
        foreach (var item in miniGameButtons)
        {
            item.OnClick += RenderSelectedMiniGame;
        }

        closeGameButton.onClick.AddListener(CloseCurrentMiniGame);
    }

    private void OnDestroy()
    {
        foreach (var item in miniGameButtons)
        {
            item.OnClick -= RenderSelectedMiniGame;
        }

        closeGameButton.onClick.RemoveListener(CloseCurrentMiniGame);
    }

    public void InitializeCore(MiniGamesSystem system) 
    {
        _system = system;
    }

    public void RenderSelectedMiniGame(AbstractMiniGameTypeView view) 
    {
        view.Create();
        onSpawnMiniGame?.Invoke();

        _currentMiniGame = view;
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
}