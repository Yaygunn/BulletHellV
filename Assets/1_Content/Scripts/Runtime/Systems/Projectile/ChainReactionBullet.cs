namespace BH.Runtime.Systems
{
    public class ChainReactionBullet : Projectile
    {
        // TODO: Implement this when we have enemies..

        protected override void HandleActivation()
        {
            ReturnToPool();
        }
    }
}