using Lockstep.Math;

namespace Lockstep.Game
{
    // add by han at 2022.07.25
    public partial class CJumper : Component
    {
        static readonly LFloat g = new LFloat(true, 9800);
        public Player player { get { return (Player)entity; } }
        private PlayerInput input { get { return player.input; } }

        public bool isMoveUp = false;

        public override void DoUpdate(LFloat deltaTime)
        {
            if (!player.rigidbody.isOnFloor)
                return;

            if (input.isSpeedUp)
            {
                player.rigidbody.AddImpulse(new LVector3(0, 5, 0));
            }
        }
    }
}
