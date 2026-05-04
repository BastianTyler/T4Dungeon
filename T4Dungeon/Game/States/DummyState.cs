using System;
using T4Dungeon.Game.States;

public class DummyState : IGameState
{
    public void Enter()
    {
        Console.WriteLine("Entered Dummy State");
    }

    public void Update()
    {
        Console.WriteLine("Running...");
        Console.ReadKey();
    }

    public void Exit()
    {
        Console.WriteLine("Exiting Dummy State");
    }
}