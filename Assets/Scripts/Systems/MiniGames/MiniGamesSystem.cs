using UnityEngine;

[RequireComponent(typeof(IMiniGamesRendererModule))]
public sealed class MiniGamesSystem : MonoBehaviour, ISystem
{
    [Header("Data")]
    [SerializeField] private MiniGamesConfig config;

    [Header("Modules")]
    [SerializeReference, SerializeField] private MiniGamesRendererModule rendererModule;

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