using System;

namespace Lockstep.Game
{
    [Serializable]
    [NoBackup]
    public class Component : BaseComponent
    {
        public Entity entity { get { return (Entity)baseEntity; } }
        public IGameStateService GameStateService { get { return entity.GameStateService; } }
        public IDebugService DebugService { get { return entity.DebugService; } }
    }
}