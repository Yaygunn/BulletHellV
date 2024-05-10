using BH.Runtime.Entities;

namespace BH.Runtime.Factories
{
    public interface IPLayerFactory
    {
        public PlayerController CreatePlayer();
    }
}