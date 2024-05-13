namespace BH.Runtime.Systems
{
    public class BasicBullet : Projectile
    {
        protected override void HandleActivation()
        {
            ReturnToPool();
        }
    }
}