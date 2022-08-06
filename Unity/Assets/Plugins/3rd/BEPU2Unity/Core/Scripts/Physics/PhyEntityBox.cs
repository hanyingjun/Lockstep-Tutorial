namespace vwengame.bephysics
{
    using BEPUphysics.EntityStateManagement;
    using UnityEngine;

    public class PhyEntityBox : PhyEntityBase
    {
        [SerializeField] Vector3 _size = new Vector3(1, 1, 1);

        public Vector3 Size
        {
            get { return Multiply(ref _size, transform.lossyScale); }
        }

        protected override void DoAwake()
        {
            base.DoAwake();
            MotionState motionState = new MotionState()
            {
                Position = transform.position,
                Orientation = transform.rotation,
                AngularVelocity = BEPUutilities.Vector3.Zero,
                LinearVelocity = BEPUutilities.Vector3.Zero
            };
            Vector3 whl = Size;
            entity = new BEPUphysics.Entities.Prefabs.Box(motionState, whl.x, whl.y, whl.z, IsRigibody ? mass : 0);
        }

        protected override Vector3 GetGizmosSize()
        {
            return Size;
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}
