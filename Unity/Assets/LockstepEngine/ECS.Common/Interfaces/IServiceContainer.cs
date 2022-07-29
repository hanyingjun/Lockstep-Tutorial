namespace Lockstep.Game
{
    public interface IServiceContainer
    {
        T GetService<T>() where T : IService;
        IService[] GetAllServices();
    }

}