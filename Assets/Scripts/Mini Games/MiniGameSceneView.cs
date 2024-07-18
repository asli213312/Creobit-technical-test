using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSceneView : AbstractMiniGameTypeView
{
    [Header("Additional data")]
    [SerializeField] private string miniGameSceneName;

    protected override void Prepare(AbstractMiniGameView miniGameView, System.Action<AbstractMiniGameView> onCreated) 
    {
        SceneManager.LoadScene(miniGameSceneName);
        SceneManager.sceneLoaded += (newScene, mode) => OnSceneLoaded(newScene, mode, onCreated);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode, System.Action<AbstractMiniGameView> onCreated) 
    {
        SceneManager.sceneLoaded -= (newScene, mode) => OnSceneLoaded(newScene, mode, onCreated);

        AbstractMiniGameView view = FindObjectOfType(MiniGameView.GetType()) as AbstractMiniGameView;
        if (view == null) 
        {
            Debug.LogError("Couldn't find corrected view on scene to render minigame. Corrected view by data: " + MiniGameView.MiniGameData);
            return;
        }

        onCreated?.Invoke(view);
        view.Render(view.MiniGameData, LoadedAssets);

        GameInstance = view;
    }
}