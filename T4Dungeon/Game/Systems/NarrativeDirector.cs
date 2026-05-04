using T4Dungeon.Game.Models;
using T4Dungeon.Game.Narrative;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.Systems;

public class NarrativeDirector
{
    public bool IsActive { get; private set; }
    public StoryContext Context { get; } = new();

    private INarrativeScript _script;
    public bool IsTutorial => _script is TutorialScript;

    public void Load(INarrativeScript script)
    {
        _script = script;
        IsActive = true;
        _script.Init(this);
    }
    public string ValidateChoice(string optionText)
    => IsActive ? _script.ValidateChoice(optionText, Context) : null;
    public void Stop() => IsActive = false;

    public List<string> FilterMenuOptions(string menuContext, List<string> options)
        => IsActive ? _script.FilterMenuOptions(menuContext, options, Context) : options;

    public void OnEvent(string eventName, object payload = null)
    {
        if (IsActive) _script.OnEvent(eventName, payload, Context);
    }

    public event Action<string> OnMapLoadRequested;
    public void RequestMapLoad(string path) => OnMapLoadRequested?.Invoke(path);

    private EnemyId? _enemyOverride;
    public void SetEnemyOverride(EnemyId id) => _enemyOverride = id;
    public EnemyId? ConsumeEnemyOverride() { var v = _enemyOverride; _enemyOverride = null; return v; }

    public event Action<string, bool> OnNarrativeMessage;
    public void Say(string msg, bool waitForKey = true) => OnNarrativeMessage?.Invoke(msg, waitForKey);

    public event Action<ItemId[]> OnStartingItemsRequested;
    public void SetStartingItems(ItemId[] items) => OnStartingItemsRequested?.Invoke(items);

    public event Action<EquiptSlot, ItemId> OnStartingEquipmentRequested;
    public void SetStartingEquipment(EquiptSlot slot, ItemId id) => OnStartingEquipmentRequested?.Invoke(slot, id);

    // Item drops forced by narrative
    public event Action<ItemId> OnItemDropRequested;
    public void DropItem(ItemId id) => OnItemDropRequested?.Invoke(id);

    // Treasure drops forced by narrative
    public event Action<ItemId> OnForcedTreasureDropRequested;
    public void ForceTreasureDrop(ItemId id) => OnForcedTreasureDropRequested?.Invoke(id);

    // Shop inventory override
    public event Action<ItemId[]> OnShopOverrideRequested;
    public void UseShopOverride(ItemId[] items) => OnShopOverrideRequested?.Invoke(items);

    // Tutorial completion
    public event Action OnTutorialComplete;
    public void CompleteTutorial()
    {
        Stop();
        OnTutorialComplete?.Invoke();
    }

    public event Action<List<CutsceneBeat>, Action> OnCutsceneRequested;
    public void PlayCutscene(List<CutsceneBeat> beats, Action onComplete)
        => OnCutsceneRequested?.Invoke(beats, onComplete);
}