using Lockstep.Game;
using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision2D
{
    public partial class EntityTest : BaseEntity
    {
        protected override void DoInitialize()
        {
            base.DoInitialize();
            AddComponent<CRigidbody>();
        }
    }

    public class TestRigidbody : MonoBehaviour
    {
        public EntityTest entity = null;
        public CRigidbody CRigidbody;
        public CTransform2D CTransform2D;

        public LFloat G;
        public LVector3 force;

        public LFloat MinSleepSpeed = new LFloat(true, 100);
        public LFloat FloorFriction = new LFloat(3);
        public LFloat Mass = LFloat.one;
        public LFloat resetYSpd = new LFloat(true, 100);
        private void Start()
        {
            entity = new EntityTest();
            entity.Initialize();
            entity.DoBindRef();

            CRigidbody = entity.GetComponent<CRigidbody>();
            CTransform2D = entity.GetComponent<CTransform2D>();
            CTransform2D.Pos3 = transform.position.ToLVector3();
            CRigidbody.DoStart();
        }

        private void Update()
        {
            CRigidbody.G = G;
            CRigidbody.MinSleepSpeed = MinSleepSpeed;
            CRigidbody.FloorFriction = FloorFriction;

            CRigidbody.Mass = Mass;
            CRigidbody.DoUpdate(Time.deltaTime.ToLFloat());
            transform.position = CTransform2D.Pos3.ToVector3();
        }

        public void AddImpulse()
        {
            CRigidbody.AddImpulse(force);
        }
        public void ResetSpeed(LFloat ySpeed)
        {
            CRigidbody.ResetSpeed(ySpeed);
        }
    }
}