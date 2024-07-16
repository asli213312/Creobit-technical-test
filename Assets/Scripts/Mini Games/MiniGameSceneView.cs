using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSceneView : AbstractMiniGameTypeView
{
    [Header("Additional data")]
    [SerializeField] private Scene miniGameScene;

    protected override void Prepare(AbstractMiniGameData data) 
    {
        SceneManager.LoadScene(miniGameScene.name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        AbstractMiniGameView view = FindObjectOfType<AbstractMiniGameView>();
        view.Render(view.MiniGameData);
    }
}