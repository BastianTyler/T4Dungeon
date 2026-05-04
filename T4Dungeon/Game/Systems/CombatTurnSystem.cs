using T4Dungeon.Game.Systems;

public class CombatTurnSystem
{
    private readonly CombatOrchestrator _orchestrator;

    public CombatTurnSystem(CombatOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public void PlayerTurn(Action action)
    {
        action?.Invoke();
        _orchestrator.ResolveAfterPlayer();
    }

}