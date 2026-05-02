using T4Dungeon.Generated;

namespace T4Dungeon.Game.MiniGames
{
    internal interface IMiniGame
    {
        bool Run(MoveDef move);
        bool RunStep(SkillStep step);
    }
}