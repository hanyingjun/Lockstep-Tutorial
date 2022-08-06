namespace vwengame.bephysics
{
    using BEPUphysics.EntityStateManagement;
    using UnityEngine;

    public class PhyEntityCylinder : PhyEntityBase
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _height = 2.0f;

        public float radius { get { return _radius; } }
        public float height { get { return _height; } }

        protected override void DoAwake()
        {
            base.DoAwake();
            MotionState motionState = new MotionState()
            {
                Position = transform.position,
                Orientation = transform.rotation,
                LinearVelocity = BEPUutilities.Vector3.Zero,
                AngularVelocity = BEPUutilities.Vector3.Zero
            };
            entity = new BEPUphysics.Entities.Prefabs.Cylinder(motionState, height, radius, IsRigibody ? mass : 0);
        }

        protected override void DrawGizmos()
        {
            if (height >= 1)
            {
                Gizmos.DrawWireSphere(new Vector3(0, height - radius * 2, 0), 1);
                Gizmos.DrawWireSphere(new Vector3(0, -height + radius * 2, 0), 1);
            }
            else
            {
                // 中间
                Gizmos.DrawWireSphere(Vector3.zero, 1);
            }
        }

        protected override Vector3 GetGizmosSize()
        {
            return Vector3.one * (float)radius;
        }
    }
}
