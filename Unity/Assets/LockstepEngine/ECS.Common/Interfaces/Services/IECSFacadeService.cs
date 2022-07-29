namespace Lockstep.Game {
    public interface IECSFacadeService :IService {
        IContexts CreateContexts();
        object CreateSystems(object contexts, IServiceContainer services);
    }
}