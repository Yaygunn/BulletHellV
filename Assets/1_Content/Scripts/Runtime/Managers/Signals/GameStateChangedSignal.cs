namespace BH.Runtime.Managers
{
    public struct GameStateChangedSignal
    {
        public GameState GameState { get; }
        
        public GameStateChangedSignal(GameState gameState)
        {
            GameState = gameState;
        }
    }
}