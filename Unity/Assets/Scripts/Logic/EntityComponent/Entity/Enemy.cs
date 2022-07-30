using System;

namespace Lockstep.Game
{
    [Serializable]
    public partial class Enemy : Entity
    {
        [Backup]
        public CBrain brain { get { return this.GetComponent<CBrain>(); } }

        protected override void DoInitialize()
        {
            base.DoInitialize();
            AddComponent<CBrain>();
        }

        protected override void DoFillComponentsConfig()
        {
            base.DoFillComponentsConfig();
            EnemyConfig config = ServiceContainer.GetService<IGameConfigService>().GetEntityConfig(base.PrefabId) as EnemyConfig;
            if (config != null)
            {
                GetComponent<CAnimator>().configId = config.animationId;
                GetComponent<CSkillBox>().configId = config.skillId;
            }
        }
    }
}