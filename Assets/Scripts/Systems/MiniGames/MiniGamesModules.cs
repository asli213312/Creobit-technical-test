public interface IMiniGamesRendererModule : IModule<MiniGamesSystem> 
{
    void RenderSelectedMiniGame(AbstractMiniGameData miniGameData);
}