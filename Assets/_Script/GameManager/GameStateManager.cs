using System;
public enum GameStates
{
    OnGame,
    OnWin
}

public static class GameStateManager
{
    public static event Action<GameStates> OnGameStateChanged;
    static GameStates _currentState;
    
    public static void ChangeGameState(GameStates state)
    {
        _currentState = state;
        OnGameStateChanged?.Invoke(state);
    }
}