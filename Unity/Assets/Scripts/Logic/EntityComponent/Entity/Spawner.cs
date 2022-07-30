using Lockstep.Math;
using System;

namespace Lockstep.Game
{
    [Serializable]
    public partial class Spawner : BaseEntity
    {
        [NoBackup]
        public SpawnerInfo Info = new SpawnerInfo();
        [Backup]
        [NonSerialized]
        public LFloat Timer;

        private SpawnerInfo _info = null;
        [NoBackup]
        public SpawnerInfo SpInfo
        {
            get
            {
                if (_info == null)
                {
                    SpawnerConfig config = ServiceContainer.GetService<IGameConfigService>().GetEntityConfig(base.PrefabId) as SpawnerConfig;
                    _info = config.entity.Info;
                }
                return _info;
            }
        }

        public override void DoStart()
        {
            Timer = 0;
        }

        public override void DoUpdate(LFloat deltaTime)
        {
            Timer += deltaTime;
            if (Timer > SpInfo.spawnInternal)
            {
                Timer = Timer - SpInfo.spawnInternal;
                Spawn();
            }
        }

        public void Spawn()
        {
            if (GameStateService.CurEnemyCount >= GameStateService.MaxEnemyCount)
            {
                return;
            }

            GameStateService.CurEnemyCount++;
            GameStateService.CreateEntity<Enemy>(SpInfo.prefabId, SpInfo.spawnPoint);
        }
    }
}