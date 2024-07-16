using UnityEngine;

[CreateAssetMenu(menuName = "Creobit-technical-test/MiniGames/Config")]
public class MiniGamesConfig : ScriptableObject
{
    [SerializeField, SerializeReference] public AbstractMiniGameData[] miniGames;
}