namespace vwengame.bephysics
{
    using BEPUphysics.EntityStateManagement;
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
    }
}
