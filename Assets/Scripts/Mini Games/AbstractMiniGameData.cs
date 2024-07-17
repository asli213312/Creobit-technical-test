using UnityEngine;

public abstract class AbstractMiniGameData : ScriptableObject
{
    [Header("Base data")]
    [SerializeField] public string assetBundleName;
    [SerializeField, Tooltip("True, if need minigame in different scene")] public bool useSceneView;

    [Header("Additional data")]
    [SerializeField] private UnityEngine.SceneManagement.Scene blank;
}