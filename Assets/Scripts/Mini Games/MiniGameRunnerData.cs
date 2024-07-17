using UnityEngine;

[CreateAssetMenu(menuName = "Game/MiniGames/RunnerData")]
public class MiniGameRunnerData : AbstractMiniGameData
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float mouseSensitivity;
}