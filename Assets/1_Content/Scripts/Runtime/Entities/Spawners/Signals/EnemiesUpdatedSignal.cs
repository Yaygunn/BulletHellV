namespace BH.Runtime.Entities
{
    public struct EnemiesUpdatedSignal
    {
        public int TotalEnemies { get; }
        public int RemainingEnemies { get; }
        
        public EnemiesUpdatedSignal(int totalEnemies, int remainingEnemies)
        {
            TotalEnemies = totalEnemies;
            RemainingEnemies = remainingEnemies;
        }
    }
}