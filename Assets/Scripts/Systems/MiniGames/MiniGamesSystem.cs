using UnityEngine;

[RequireComponent(typeof(IMiniGamesRendererModule))]
public sealed class MiniGamesSystem : MonoBehaviour, ISystem
{
    #region Serialized Fields
    [Header("Data")]
    [SerializeField] private MiniGamesConfig config;

    [Header("Modules")]
    [SerializeReference, SerializeField] private MiniGamesRendererModule rendererModule;
    #endregion Serialized Fields

    #region Properties
    public MiniGamesConfig Config => config;
    public IMiniGamesRendererModule RendererModule => rendererModule;

    #endregion Properties

    #region UnityLoop Events
    private void Start() 
    {
        Initialize();
    }

    #endregion UnityLoop Events

    #region Public Methods
    public void Initialize() 
    {
        rendererModule.InitializeCore(this);
    }

    #endregion Public Methods
}