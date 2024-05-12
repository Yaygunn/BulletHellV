namespace BH.Runtime.Systems
{
    public struct UpgradeOption
    {
        public UpgradeType Type { get; private set; }
        public ProjectileType ProjectileType { get; private set; }
        public string Description { get; private set; }
        
        public UpgradeOption(UpgradeType type, ProjectileType projectileType, string description)
        {
            Type = type;
            ProjectileType = projectileType;
            Description = description;
        }
    }
}