namespace vwengame.bephysics
{
    using UnityEngine;
    using UnityEngine.Serialization;

    public class PhyCharacterController : PhyEntityBase
    {
        [SerializeField] private float height = 1.7f;
        [SerializeField] private float radius = 0.6f;
        [SerializeField] float margin = 0.1f;
        [SerializeField] new float mass = 10;
        [FormerlySerializedAs("行走最大坡度")] [SerializeField] float maximumTractionSlope = 0.8f;
        [FormerlySerializedAs("静止最大坡度")] [SerializeField] float maximumSupportSlope = 1.3f;
        [FormerlySerializedAs("移速")] [SerializeField] float standingSpeed = 3;
        [FormerlySerializedAs("蹲下移速")] [SerializeField] float crouchingSpeed = 2;
        [FormerlySerializedAs("爬行移速")] [SerializeField] float prongSpeed = 1;
        [FormerlySerializedAs("最大牵引力")] [SerializeField] float tractionForce = 1000;
        [SerializeField] float slidingSpeed = 6;
        [SerializeField] float slidingForce = 50;
        [SerializeField] float airSpeed = 1;
        [SerializeField] float airForce = 250;
        [SerializeField] float jumpSpeed = 4.5f;
        [SerializeField] float slidingJumpSpeed = 3;
        [SerializeField] float maximumGlueForce = 5000;

        public BEPUutilities.Vector2 HorizontalMoveDir
        {
            get
            {
                return controller.HorizontalMotionConstraint.MovementDirection;
            }
            set
            {
                controller.HorizontalMotionConstraint.MovementDirection = value;
            }
        }

        public BEPUphysics.Character.CharacterController controller { get; private set; }

        protected override void DoAwake()
        {
            base.DoAwake();
            controller = new BEPUphysics.Character.CharacterController(transform.position, height, height * 0.7f, height * 0.3f, radius,
                margin, mass, maximumTractionSlope, maximumSupportSlope, standingSpeed, crouchingSpeed, prongSpeed, tractionForce, slidingSpeed,
                slidingForce, airSpeed, airForce, jumpSpeed, slidingJumpSpeed, maximumGlueForce);
        }

        protected override void DoLateUpdate()
        {
            this.transform.position = this.controller.Body.Position;
            this.transform.rotation = this.controller.Body.Orientation;
        }

        protected override Vector3 GetGizmosSize()
        {
            return Vector3.one;
        }

        protected override void DrawGizmos()
        {
            Gizmos.DrawLine(Vector3.right * radius + Vector3.down * height / 2, Vector3.right * radius + Vector3.up * height / 2);
            Gizmos.DrawLine(-Vector3.right * radius + Vector3.down * height / 2, -Vector3.right * radius + Vector3.up * height / 2);
            Gizmos.DrawLine(Vector3.forward * radius + Vector3.down * height / 2, Vector3.forward * radius + Vector3.up * height / 2);
            Gizmos.DrawLine(-Vector3.forward * radius + Vector3.down * height / 2, -Vector3.forward * radius + Vector3.up * height / 2);
        }
    }
}
