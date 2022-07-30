using Lockstep.Math;

namespace Lockstep.Game
{
    public class EnemySystem : BaseSystem
    {
        private Spawner[] Spawners
        {
            get { return _gameStateService.GetSpawners(); }
        }
        private Enemy[] AllEnemy
        {
            get { return _gameStateService.GetEnemies(); }
        }

        public override void DoStart()
        {
            for (int i = 0; i < 3; i++)
            {
                var configId = 100 + i;
                var config = _gameConfigService.GetEntityConfig(configId) as SpawnerConfig;
                _gameStateService.CreateEntity<Spawner>(configId, config.entity.Info.spawnPoint);
            }
        }

        public override void DoUpdate(LFloat deltaTime)
        {
            foreach (var spawner in Spawners)
            {
                spawner.DoUpdate(deltaTime);
            }

            foreach (var enemy in AllEnemy)
            {
                enemy.DoUpdate(deltaTime);
            }
        }
    }
}