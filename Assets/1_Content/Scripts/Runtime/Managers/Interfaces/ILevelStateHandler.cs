using System;

namespace BH.Runtime.Managers
{
    public interface ILevelStateHandler
    {
        public LevelState CurrentLevelState { get; }

        public event Action<LevelState> OnLevelStateChanged;

        public void SetLevelState(LevelState newState);
    }
}