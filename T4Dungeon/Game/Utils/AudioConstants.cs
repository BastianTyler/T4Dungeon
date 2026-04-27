namespace T4Dungeon.Game.Utils;

/// <summary>
/// All music track filenames. Files live in Resources/Audio/Music/.
/// Use these constants with AudioManager.PlayMusic() so filenames
/// are never scattered as magic strings throughout the codebase.
/// </summary>
public static class Music
{
    // ─── Ambience / Exploration ───────────────────────────────────
    /// <summary>Main menu / title screen. Dark, atmospheric, slow.</summary>
    public const string TitleScreen = "title_screen.mp3";

    /// <summary>General dungeon exploration. Tense, low drone, looping.</summary>
    public const string DungeonAmbient = "dungeon_ambient.mp3";

    /// <summary>Deeper floors. Darker and more oppressive than ambient.</summary>
    public const string DeepDungeon = "deep_dungeon.mp3";

    // ─── Combat ───────────────────────────────────────────────────
    /// <summary>Standard combat music. Energetic, driving beat.</summary>
    public const string Combat = "combat.mp3";

    /// <summary>Boss or elite combat. More intense than standard combat.</summary>
    public const string CombatIntense = "combat_intense.mp3";

    // ─── Special Screens ──────────────────────────────────────────
    /// <summary>Shop / merchant screen. Lighter, slightly whimsical.</summary>
    public const string Shop = "shop.mp3";

    /// <summary>Victory / win screen.</summary>
    public const string Victory = "victory.mp3";

    /// <summary>Game over / death screen.</summary>
    public const string GameOver = "game_over.mp3";
}

/// <summary>
/// All sound effect filenames. Files live in Resources/Audio/Sfx/.
/// Use these constants with AudioManager.PlaySfx().
/// </summary>
public static class Sfx
{
    // ─── UI / Menu ────────────────────────────────────────────────
    /// <summary>Selecting a valid menu option.</summary>
    public const string MenuSelect = "menu_select.wav";

    /// <summary>Attempting an unimplemented or invalid option.</summary>
    public const string MenuDeny = "menu_deny.wav";

    /// <summary>Moving between screens or opening a submenu.</summary>
    public const string MenuNavigate = "menu_navigate.wav";

    // ─── Combat — Player ──────────────────────────────────────────
    /// <summary>Player lands an attack hit.</summary>
    public const string PlayerAttack = "player_attack.wav";

    /// <summary>Player successfully counters an enemy move.</summary>
    public const string CounterSuccess = "counter_success.wav";

    /// <summary>Player fails to counter — takes damage.</summary>
    public const string CounterFail = "counter_fail.wav";

    /// <summary>Player uses a potion or consumable item.</summary>
    public const string UsePotion = "use_potion.wav";

    /// <summary>Player successfully flees from combat.</summary>
    public const string Flee = "flee.wav";

    // ─── Combat — Enemy ───────────────────────────────────────────
    /// <summary>Enemy announces an attack move.</summary>
    public const string EnemyAttack = "enemy_attack.wav";

    /// <summary>Enemy is defeated.</summary>
    public const string EnemyDeath = "enemy_death.wav";

    /// <summary>Player dies.</summary>
    public const string PlayerDeath = "player_death.wav";

    // ─── Minigames ────────────────────────────────────────────────
    /// <summary>Timed press — bar starts filling. Short whoosh.</summary>
    public const string TimerStart = "timer_start.wav";

    /// <summary>Mash minigame — each successful key press registers.</summary>
    public const string MashTick = "mash_tick.wav";

    /// <summary>Sweet spot — marker bouncing sound (subtle, looping tick).</summary>
    public const string SweetSpotTick = "sweetspot_tick.wav";

    /// <summary>Any minigame succeeds. Sharp positive chime.</summary>
    public const string MinigameWin = "minigame_win.wav";

    /// <summary>Any minigame fails. Low thud or buzz.</summary>
    public const string MinigameFail = "minigame_fail.wav";

    // ─── Exploration ──────────────────────────────────────────────
    /// <summary>Player steps onto a new cell.</summary>
    public const string Footstep = "footstep.wav";

    /// <summary>Player opens a treasure chest.</summary>
    public const string TreasureOpen = "treasure_open.wav";

    /// <summary>Player picks up gold.</summary>
    public const string GoldPickup = "gold_pickup.wav";

    /// <summary>Player finds the floor exit.</summary>
    public const string ExitFound = "exit_found.wav";

    /// <summary>Player enters the shop.</summary>
    public const string ShopEnter = "shop_enter.wav";

    /// <summary>Player buys an item.</summary>
    public const string ShopBuy = "shop_buy.wav";

    /// <summary>Player can't afford an item.</summary>
    public const string ShopDeny = "shop_deny.wav";

    /// <summary>Player equips a weapon or armor piece.</summary>
    public const string Equip = "equip.wav";
}