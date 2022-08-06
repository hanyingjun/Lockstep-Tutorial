namespace vwengame.bephysics
{
    using BEPUphysics.BroadPhaseEntries;
    using BEPUphysics.BroadPhaseEntries.MobileCollidables;
    using BEPUphysics.EntityStateManagement;
    using BEPUphysics.NarrowPhaseSystems.Pairs;
    using UnityEngine;

    public class PhyEntitySphere : PhyEntityBase
    {
        [SerializeField] private float _radius = 0.5f;
        public float radius { get { return _radius; } }

        protected override void DoAwake()
        {
            base.DoAwake();
            MotionState motionState = new MotionState()
            {
                Position = transform.position,
                Orientation = transform.rotation,
                LinearVelocity = new BEPUutilities.Vector3(),
                AngularVelocity = new BEPUutilities.Vector3(),
            };
            entity = new BEPUphysics.Entities.Prefabs.Sphere(motionState, radius, IsRigibody ? mass : 0);
            entity.CollisionInformation.Events.DetectingInitialCollision += CollisionEnter;
            entity.CollisionInformation.Events.CollisionEnded += CollisionExit;
            //box.CollisionInformation.CollisionRules.Personal = isTrigger ? CollisionRule.NoSolver : CollisionRule.Normal;
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawWireSphere(Vector3.zero, radius);
        }

        protected override Vector3 GetGizmosSize()
        {
            Vector3 v3 = new Vector3(1, 1, 1);
            return v3;
        }

        private void CollisionEnter(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            Debug.LogError(other);
        }

        private void CollisionExit(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
        }
    }
}
