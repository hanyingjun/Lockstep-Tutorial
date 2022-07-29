using Lockstep.Collision2D;
using Lockstep.Math;
using System;

namespace Lockstep.Game
{
    [Serializable]
    [NoBackup]
    public partial class BaseComponent : IComponent
    {
        public BaseEntity baseEntity { get; private set; }
        public CTransform2D transform { get { return baseEntity.transform; } }

        public virtual void BindEntity(BaseEntity entity)
        {
            this.baseEntity = entity;
        }

        public virtual void DoAwake() { }
        public virtual void DoStart() { }
        public virtual void DoUpdate(LFloat deltaTime) { }
        public virtual void DoDestroy() { }
    }
}