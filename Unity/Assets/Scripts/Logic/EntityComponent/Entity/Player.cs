using Lockstep.Math;
using System;

namespace Lockstep.Game
{
    [Serializable]
    public partial class Player : Entity
    {
        [Backup]
        public int localId;
        public PlayerInput input = new PlayerInput();
        public CMover mover = new CMover();
        public CJumper jumper = new CJumper();

        protected override void DoInitialize()
        {
            base.DoInitialize();
            AddComponent<CMover>();
            AddComponent<CJumper>();
        }

        protected override void DoFillComponentsConfig()
        {
            base.DoFillComponentsConfig();
            PlayerConfig config = ServiceContainer.GetService<IGameConfigService>().GetEntityConfig(base.PrefabId) as PlayerConfig;
            if (config != null)
            {
                GetComponent<CAnimator>().configId = config.animationId;
                GetComponent<CSkillBox>().configId = config.skillId;
            }
        }

        public override void DoUpdate(LFloat deltaTime)
        {
            base.DoUpdate(deltaTime);
            if (input.skillId != 0)
            {
                Logging.Debug.Log("ִ�м��� " + input.skillId);
                Fire(input.skillId);
            }
        }
    }
}