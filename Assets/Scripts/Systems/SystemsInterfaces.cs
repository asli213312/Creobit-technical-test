public interface ISystem 
{
    void Initialize();
}

public interface IModule<in TSystem> where TSystem : ISystem
{
    void InitializeCore(TSystem system);
}