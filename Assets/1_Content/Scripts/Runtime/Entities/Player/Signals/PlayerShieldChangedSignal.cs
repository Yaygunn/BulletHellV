namespace BH.Runtime.Entities
{
    public struct PlayerShieldChangedSignal
    {
        public int MaxShield { get; }
        public int CurrentShield { get; }

        public PlayerShieldChangedSignal(int maxShield, int currentShield)
        {
            MaxShield = maxShield;
            CurrentShield = currentShield;
        }
    }
}