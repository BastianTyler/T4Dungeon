using T4Dungeon.Game.Models;
using T4Dungeon.Game.Narrative;
using T4Dungeon.Game.Systems;

public class StoryScript : INarrativeScript
{
    private NarrativeDirector _director;
    private StoryChapter _chapter = StoryChapter.Intro;

    public List<string> FilterMenuOptions(string menuContext, List<string> options, StoryContext ctx)
    {
        throw new NotImplementedException();
    }

    public void Init(NarrativeDirector director)
    {
        _director = director;
        PlayIntro();
    }

    public void OnEvent(string eventName, object payload, StoryContext ctx)
    {
        //throw new NotImplementedException();
    }

    public string ValidateChoice(string optionText, StoryContext ctx)
    {
        throw new NotImplementedException();
    }

    private void PlayIntro()
    {
        _director.PlayCutscene(new List<CutsceneBeat>
        {
            new CutsceneBeat { Text = "The room is small and cold...", WaitForKey = true },
            new CutsceneBeat { Text = "Your sister stirs in her bed.", WaitForKey = true },
            new CutsceneBeat { Text = "Sister: 'Promise me...'", WaitForKey = true },
        }, onComplete: () =>
        {
            _director.RequestMapLoad("Data/Maps/home.txt");
            _chapter = StoryChapter.Home;
        });
    }
}