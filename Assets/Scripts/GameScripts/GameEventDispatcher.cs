
public static class GameEventDispatcher
{
    public delegate void PlayerAtExitEvent();
    public static event PlayerAtExitEvent PlayerAtExit;
    
    public delegate void PlayerOnLavaEvent();
    public static event PlayerOnLavaEvent PlayerOnLava;

    public delegate void PlayerReachedStarEvent(int index);
    public static event PlayerReachedStarEvent PlayerReachedStar;

    public delegate void PlayerTookStepEvent();

    public static event PlayerTookStepEvent PlayerTookStep;

    public delegate void GameEndEvent(bool isVictorious);

    public static event GameEndEvent GameEnd;
    
    public delegate void StartAgainEvent();

    public static event StartAgainEvent StartAgain;

    public static void DispatchPlayerAtExitEvent()
    {
        PlayerAtExit?.Invoke();
    }

    public static void DispatchPlayerOnLavaEvent()
    {
        PlayerOnLava?.Invoke();
    }

    public static void DispatchPlayerReachedStarEvent(int index)
    {
        PlayerReachedStar?.Invoke(index);
    }

    public static void DispatchPlayerTookStepEvent()
    {
        PlayerTookStep?.Invoke();
    }

    public static void DispatchGameEndEvent(bool isVictorious)
    {
        GameEnd?.Invoke(isVictorious);
    }

    public static void DispatchStartAgainEvent()
    {
        StartAgain?.Invoke();
    }
}
