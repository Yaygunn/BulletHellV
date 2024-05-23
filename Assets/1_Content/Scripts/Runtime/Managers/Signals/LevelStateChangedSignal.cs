namespace BH.Runtime.Managers
{
    public struct LevelStateChangedSignal
    {
        public LevelState LevelState { get; }
        
        public LevelStateChangedSignal(LevelState levelState)
        {
            LevelState = levelState;
        }
    }
}