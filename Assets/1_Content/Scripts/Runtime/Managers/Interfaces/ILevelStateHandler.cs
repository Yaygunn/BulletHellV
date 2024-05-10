using System;

namespace BH.Runtime.Managers
{
    public interface ILevelStateHandler
    {
        public LevelState CurrentLevelState { get; }

        public Action<LevelState> OnLevelStateChanged { get; }

        public void SetLevelState(LevelState newState);
    }
}