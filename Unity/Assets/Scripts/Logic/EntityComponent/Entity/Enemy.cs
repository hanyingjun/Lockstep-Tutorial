using System;

namespace Lockstep.Game
{
    [Serializable]
    public partial class Enemy : Entity
    {
        public CBrain brain { get { return this.GetComponent<CBrain>(); } }

        protected override void DoInitialize()
        {
            base.DoInitialize();
            AddComponent<CBrain>();
        }

        //protected override void BindRef()
        //{
        //    base.BindRef();
        //    RegisterComponent(brain);
        //    moveSpd = 2;
        //    turnSpd = 150;
        //}
    }
}