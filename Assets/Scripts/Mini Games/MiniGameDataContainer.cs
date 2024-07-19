using UnityEngine;

[CreateAssetMenu(menuName = "Game/MiniGames/Data Container")]
public class MiniGameDataContainer : ScriptableObject
{
    [SerializeField] public string assetBundleName;
    [SerializeField] public string dataName;
}