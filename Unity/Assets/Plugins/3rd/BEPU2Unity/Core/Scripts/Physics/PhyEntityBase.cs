namespace vwengame.bephysics
{
    using BEPUphysics.BroadPhaseEntries;
    using BEPUphysics.BroadPhaseEntries.MobileCollidables;
    using BEPUphysics.Entities;
    using BEPUphysics.NarrowPhaseSystems.Pairs;
    using UnityEngine;
    using UnityEngine.Serialization;

    public abstract class PhyEntityBase : MonoBehaviour
    {
        static long GID = 0;

        public static long GenID()
        {
            return ++GID;
        }

        [SerializeField] protected bool _isUseRigibody = false;

        [FormerlySerializedAs("isTrigger")] [SerializeField] protected bool _isTrigger = false;

        [FormerlySerializedAs("isStatic")] [SerializeField] private bool _isStatic = false;
        [FormerlySerializedAs("质量")] [SerializeField] protected float _mass = 1;
        [FormerlySerializedAs("静摩擦")] [SerializeField] protected float _staticFriction = 0.6f;
        [FormerlySerializedAs("动摩擦")] [SerializeField] protected float _kineticFriction = 0.6f;
        [FormerlySerializedAs("弹力")] [SerializeField] protected float _bounciness = 0;

        [SerializeField] protected bool _isBindCollisionEvent = false;

        private long _id;
        public long Id { get { return _id; } protected set { _id = value; } }

        public bool IsRigibody { get { return _isUseRigibody; } }

        public bool isTrigger { get { return _isTrigger; } }
        public float mass { get { return _mass; } }
        public float staticFriction { get { return _staticFriction; } }
        public float kineticFriction { get { return _kineticFriction; } }
        public float bounciness { get { return _bounciness; } }
        public bool IsBindCollisionEvent { get { return _isBindCollisionEvent; } }

        public event System.Action<Collidable> onCollisionEnter;
        public event System.Action<Collidable> onCollisionExit;

        /// <summary>
        /// 移动的话通过设定这个值完成
        /// </summary>
        public BEPUutilities.Vector3 Velocity
        {
            get { return entity.LinearVelocity; }
            set { entity.LinearVelocity = value; }
        }

        /// <summary>
        /// 旋转的话通过设定这个值完成
        /// </summary>
        public BEPUutilities.Vector3 AngleVelocity
        {
            get { return entity.AngularVelocity; }
            set { entity.AngularVelocity = value; }
        }

        // 物理实体
        private Entity _entity = null;
        public Entity entity { get { return _entity; } protected set { _entity = value; } }

        private void Awake()
        {
            Id = PhyEntityBase.GenID();
            DoAwake();
            if (entity != null)
            {
                entity.Id = Id;
                if (IsRigibody)
                {
                    entity.CollisionInformation.CollisionRules.Personal = BEPUphysics.CollisionRuleManagement.CollisionRule.Defer;
                    entity.Material.StaticFriction = staticFriction;
                    entity.Material.KineticFriction = kineticFriction;
                    entity.Material.Bounciness = bounciness;
                }
                else
                {
                    entity.CollisionInformation.CollisionRules.Personal = isTrigger ? BEPUphysics.CollisionRuleManagement.CollisionRule.NoSolver : BEPUphysics.CollisionRuleManagement.CollisionRule.Normal;
                }

                if (IsBindCollisionEvent)
                {
                    entity.CollisionInformation.Events.DetectingInitialCollision += CollisionEnter;
                    entity.CollisionInformation.Events.CollisionEnded += CollisionExit;
                }
            }
        }

        protected virtual void OnEnable()
        {
            BEPUPhysicsMgr.AddPyhsicsEntity(this);
        }

        protected virtual void OnDisable()
        {
            BEPUPhysicsMgr.RemovePhysicsEntity(this);
        }

        public void OnFixedUpdate(FixMath.NET.Fix64 deltaTime)
        {
            DoFixedUpdate();
        }

        public void OnLateUpdate()
        {
            DoLateUpdate();
        }

        protected virtual void DoAwake() { }

        protected virtual void DoFixedUpdate() { }
        protected virtual void DoLateUpdate()
        {
            if (entity == null)
                return;
            transform.position = entity.Position;
            transform.rotation = entity.Orientation;
        }

        protected virtual void BEPUCollisionEnter(Collidable other)
        {
            if (onCollisionEnter != null)
                onCollisionEnter(other);
        }

        protected virtual void BEPUCollisionExit(Collidable other)
        {
            if (onCollisionExit != null)
                onCollisionExit(other);
        }

        private void CollisionEnter(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            //Debug.LogError(BEPUPhysicsMgr.GetEntity(pair.EntityA.Id));
            BEPUCollisionEnter(pair.CollidableA);
        }

        private void CollisionExit(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            BEPUCollisionExit(pair.CollidableA);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            //}

            //protected virtual void OnDrawGizmos()
            //{
            if (!this.enabled)
                return;

            Gizmos.color = Color.green;
            Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, GetGizmosSize());
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

            Gizmos.matrix *= cubeTransform;

            DrawGizmos();

            Gizmos.matrix = oldGizmosMatrix;
        }

        protected abstract Vector3 GetGizmosSize();

        protected abstract void DrawGizmos();

        protected Vector3 Multiply(ref Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}
