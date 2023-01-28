
public static class GameEventDispatcher
{
    public delegate void PlayerAtExitEvent();
    public static event PlayerAtExitEvent PlayerAtExit;
    
    public delegate void PlayerOnLavaEvent();
    public static event PlayerOnLavaEvent PlayerOnLava;

    public delegate void PlayerReachedStarEvent(float x, float y);
    public static event PlayerReachedStarEvent PlayerReachedStar;

    public static void DispatchPlayerAtExitEvent()
    {
        PlayerAtExit?.Invoke();
    }

    public static void DispatchPlayerOnLavaEvent()
    {
        PlayerOnLava?.Invoke();
    }

    public static void DispatchPlayerReachedStarEvent(float x, float y)
    {
        PlayerReachedStar?.Invoke(x, y);
    }
}
