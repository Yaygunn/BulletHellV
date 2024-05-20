namespace BH.Runtime.Entities
{
    public struct EnemiesUpdatedSignal
    {
        public int Wave { get; }
        public int TotalWaves { get; }
        public int TotalEnemies { get; }
        public int RemainingEnemies { get; }

        public EnemiesUpdatedSignal(int wave, int totalEnemies, int remainingEnemies, int totalWaves)
        {
            Wave = wave;
            TotalEnemies = totalEnemies;
            RemainingEnemies = remainingEnemies;
            TotalWaves = totalWaves;
        }
    }
}