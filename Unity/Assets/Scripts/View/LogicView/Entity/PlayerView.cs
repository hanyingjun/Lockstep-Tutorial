namespace Lockstep.Game
{
    public class PlayerView : EntityView, IPlayerView
    {
        public Player Player;

        public override void BindEntity(BaseEntity e, BaseEntity oldEntity = null)
        {
            base.BindEntity(e, oldEntity);
            Player = e as Player;
        }
    }
}