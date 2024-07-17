public interface IMiniGamesRendererModule : IModule<MiniGamesSystem> 
{
    void RenderSelectedMiniGame(AbstractMiniGameTypeView miniGameTypeView);
}