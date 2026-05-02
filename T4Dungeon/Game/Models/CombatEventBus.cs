public class CombatEventBus
{
    public event Action<string, bool> OnLog;
    public event Action OnStateChanged;

    public void Log(string msg, bool important = false)
        => OnLog?.Invoke(msg, important);

    public void NotifyUpdate()
        => OnStateChanged?.Invoke();
}