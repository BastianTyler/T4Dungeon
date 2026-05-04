//public class CombatLogSystem
//{
//    private readonly GameLogSystem _log;

//    public CombatLogSystem(GameLogSystem log)
//    {
//        _log = log;
//    }

//    public void Log(string msg, bool waitForKey = false, int sleepMs = 0)
//    {
//        _log.AddCombat(msg);        // this fires OnLogAdded → ForceRender automatically

//        if (sleepMs > 0)
//            Thread.Sleep(sleepMs);

//        if (waitForKey)
//        {
//            Console.WriteLine("\n -- Press any key to continue -- ");
//            Console.ReadKey(true);
//            while (Console.KeyAvailable) Console.ReadKey(true);
//        }
//    }
//}