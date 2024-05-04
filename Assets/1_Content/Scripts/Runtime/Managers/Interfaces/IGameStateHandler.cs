using System;

namespace BH.Runtime.Managers
{
    public interface IGameStateHandler
    {
        public GameState CurrentGameState { get; }

        public Action<GameState> OnGameStateChanged { get; }

        public void SetGameState(GameState gameState);
    }
}