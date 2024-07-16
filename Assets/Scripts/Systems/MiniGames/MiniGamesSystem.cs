using UnityEngine;

[RequireComponent(typeof(IMiniGamesRendererModule))]
public class MiniGamesSystem : MonoBehaviour, ISystem
{
    [Header("Data")]
    [SerializeField] private MiniGamesConfig config;

    [Header("Modules")]
    [SerializeField, SerializeReference] private IMiniGamesRendererModule rendererModule;

    public MiniGamesConfig Config => config;
    public IMiniGamesRendererModule RendererModule => rendererModule;

    private void Start() 
    {
        Initialize();
    }

    public void Initialize() 
    {
        rendererModule.InitializeCore(this);
    }
}