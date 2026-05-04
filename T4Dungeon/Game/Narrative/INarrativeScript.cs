using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;

namespace T4Dungeon.Game.Narrative;

public interface INarrativeScript
{
    void Init(NarrativeDirector director);
    List<string> FilterMenuOptions(string menuContext, List<string> options, StoryContext ctx);
    void OnEvent(string eventName, object payload, StoryContext ctx);
    string ValidateChoice(string optionText, StoryContext ctx);
}

