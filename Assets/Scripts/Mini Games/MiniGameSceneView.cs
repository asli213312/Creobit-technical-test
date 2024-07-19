using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSceneView : AbstractMiniGameTypeView
{
    #region Serialized Fields
    [Header("Additional data")]
    [SerializeField] private string miniGameSceneName;

    #endregion Serialized Fields

    #region Protected Methods
    protected override void Prepare(AbstractMiniGameView miniGameView, System.Action<AbstractMiniGameView> onCreated) 
    {
        SceneManager.LoadScene(miniGameSceneName);
        SceneManager.sceneLoaded += (newScene, mode) => OnSceneLoaded(newScene, mode, onCreated);
    }

    #endregion Protected Methods

    #region Private Methods
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

    #endregion Private Methods
}