using Lockstep.Collision2D;
using Lockstep.Math;
using System;

namespace Lockstep.Game
{
    [Serializable]
    [NoBackup]
    public partial class Entity : BaseEntity
    {
        [Backup]
        public ColliderData colliderData = new ColliderData();
        [Backup]
        public CRigidbody rigidbody
        {
            get { return base.GetComponent<CRigidbody>(); }
        }
        [Backup]
        public CAnimator animator
        {
            get { return base.GetComponent<CAnimator>(); }
        }
        [Backup]
        public CSkillBox skillBox
        {
            get { return base.GetComponent<CSkillBox>(); }
        }

        [Backup]
        public int curHealth;
        [Backup]
        public int damage = 10;
        [Backup]
        public bool isFire;
        [Backup]
        public bool isInvincible;
        [Backup]
        public int maxHealth = 100;
        [Backup]
        public LFloat moveSpd = 5;
        [Backup]
        public LFloat turnSpd = 360;

        public bool IsDead { get { return curHealth <= 0; } }

        protected override void DoInitialize()
        {
            base.DoInitialize();
            AddComponent<CRigidbody>();
            AddComponent<CAnimator>();
            AddComponent<CSkillBox>();
        }

        public override void DoStart()
        {
            base.DoStart();
            rigidbody.DoStart();
            curHealth = maxHealth;
        }

        public override void DoUpdate(LFloat deltaTime)
        {
            rigidbody.DoUpdate(deltaTime);
            base.DoUpdate(deltaTime);
        }

        public bool Fire(int idx)
        {
            return skillBox.Fire(idx - 1);
        }

        public void StopSkill(int idx)
        {
            skillBox.ForceStop(idx);
        }

        public virtual void TakeDamage(BaseEntity atker, int amount, LVector3 hitPoint)
        {
            if (isInvincible || IsDead) return;
            DebugService.Trace($"{atker.EntityId} attack {EntityId}  damage:{amount} hitPos:{hitPoint}");
            curHealth -= amount;
            EntityView?.OnTakeDamage(amount, hitPoint);
            OnTakeDamage(amount, hitPoint);
            if (IsDead)
            {
                OnDead();
            }
        }

        protected virtual void OnTakeDamage(int amount, LVector3 hitPoint) { }

        protected virtual void OnDead()
        {
            EntityView?.OnDead();
            PhysicSystem.Instance.RemoveCollider(this);
            GameStateService.DestroyEntity(this);
        }
    }
}