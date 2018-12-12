namespace Managers
{
    public class GameStateManager : ManagerBase<GameStateManager>
    {
        public enum EnumGameState
        {
            RUNNING,
            PAUSED
        }

        private EnumGameState currentGameState;

        public EnumGameState CurrentGameState
        {
            get
            {
                return currentGameState;
            }
        }

        public bool IsState(EnumGameState state)
        {
            return currentGameState == state;
        }

        public void RequestPause()
        {
            currentGameState = EnumGameState.PAUSED;

            if (EventManager.OnGamePaused != null)
            {
                EventManager.OnGamePaused();
            }
        }

        public void RequestUnpause()
        {
            currentGameState = EnumGameState.RUNNING;

            if (EventManager.OnGameUnpaused != null)
            {
                EventManager.OnGameUnpaused();
            }
        }

        public void TogglePause()
        {
            if (currentGameState == EnumGameState.RUNNING)
            {
                RequestPause();
            }
            else if (currentGameState == EnumGameState.PAUSED)
            {
                RequestUnpause();
            }
        }
    }
}