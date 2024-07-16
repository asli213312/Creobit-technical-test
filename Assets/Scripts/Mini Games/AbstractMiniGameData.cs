using UnityEngine;

public abstract class AbstractMiniGameData : ScriptableObject
{
    [Header("Base data")]
    [SerializeField, SerializeReference] public AbstractMiniGameTypeView view;
    [SerializeField] public string assetBundlePath;
}