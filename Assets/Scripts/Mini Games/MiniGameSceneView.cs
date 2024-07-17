using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSceneView : AbstractMiniGameTypeView
{
    [Header("Additional data")]
    [SerializeField] private string miniGameSceneName;

    protected override void Prepare(AbstractMiniGameView miniGameView) 
    {
        SceneManager.LoadScene(miniGameSceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        AbstractMiniGameView view = FindObjectOfType(MiniGameView.GetType()) as AbstractMiniGameView;
        if (view == null) 
        {
            Debug.LogError("Couldn't find corrected view on scene to render minigame. Corrected view by data: " + MiniGameView.MiniGameData);
            return;
        }

        view.Render(view.MiniGameData, LoadedAssets);

        GameInstance = view;
    }
}