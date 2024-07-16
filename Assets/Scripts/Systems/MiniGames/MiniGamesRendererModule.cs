using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamesRendererModule : MonoBehaviour, IMiniGamesRendererModule
{
    [SerializeField] private MiniGameLaunchButton[] miniGameButtons;

    private Action<AbstractMiniGameData> OnSelectedData;
    private MiniGamesSystem _system;

    public void InitializeCore(MiniGamesSystem system) 
    {
        _system = system;
    }

    public void RenderSelectedMiniGame(AbstractMiniGameData data) 
    {
        data.view.Create(data);
    }

    private void SelectMiniGameByData(AbstractMiniGameData selectedData) 
    {
        AbstractMiniGameData foundData = _system.Config.miniGames.FirstOrDefault(x => x == selectedData);

        if (foundData == null) 
        {
            Debug.LogError("Couldn't find data by selected launch button. Selected data name: " + selectedData.name, selectedData);
            return;
        }

        OnSelectedData?.Invoke(foundData);
    }
}