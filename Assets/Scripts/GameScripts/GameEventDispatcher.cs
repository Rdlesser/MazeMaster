
public static class GameEventDispatcher
{
    public delegate void PlayerAtExitEvent();
    public static event PlayerAtExitEvent PlayerAtExit;
    
    public delegate void PlayerOnLavaEvent();
    public static event PlayerOnLavaEvent PlayerOnLava;

    public delegate void PlayerReachedStarEvent(int x, int y);
    public static event PlayerReachedStarEvent PlayerReachedStar;

    public static void DispatchPlayerAtExitEvent()
    {
        PlayerAtExit?.Invoke();
    }

    public static void DispatchPlayerOnLavaEvent()
    {
        PlayerOnLava?.Invoke();
    }

    public static void DispatchPlayerReachedStarEvent(int x, int y)
    {
        PlayerReachedStar?.Invoke(x, y);
    }
}
