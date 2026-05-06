using T4Dungeon.Game.Models;
using T4Dungeon.Game.Narrative;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

public class TutorialScript : INarrativeScript
{
    private NarrativeDirector _director;
    private TutorialState _state = TutorialState.None;

    public void Init(NarrativeDirector director)
    {
        _director = director;

        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string mapPath = Path.Combine(baseDir, "Data", "Maps", "tutorial_map.txt");

        if (!File.Exists(mapPath))
        {
            director.Say($"Error: Map not found at {mapPath}", true);
            return;
        }

        director.SetStartingItems(new[] { ItemId.IronSword });
        director.SetStartingEquipment(EquiptSlot.Weapon, ItemId.IronSword);
        director.SetEnemyOverride(EnemyId.GreenSlime);
        director.RequestMapLoad(mapPath);

        SetState(TutorialState.TutOpenMoveMenu);
        director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Tutorial loaded. Use the {TextColor.Info}Move{TextColor.Reset} menu to navigate.", false);
    }

    private void SetState(TutorialState state)
    {
        _state = state;
    }

    public List<string> FilterMenuOptions(string menuContext, List<string> options, StoryContext ctx)
    {
        return menuContext switch
        {
            "MainMenu" => _state switch
            {
                TutorialState.TutOpenMoveMenu => options.Where(o => o == "Move").ToList(),
                TutorialState.CombatOver => options.Where(o => o == "Open Inventory").ToList(),
                TutorialState.TorchUsed => options.Where(o => o == "Move").ToList(),
                _ => options
            },

            "MoveMenu" => _state switch
            {
                TutorialState.TutSelectDown => options
                    .Where(o => o == "Down" || o == "Back")
                    .ToList(),
                _ => options
            },

            "CombatMenu" => _state switch
            {
                TutorialState.TutForceDefend => options.Where(o => o == "Defend").ToList(),
                TutorialState.TutFailedDefend => options.Where(o => o == "Defend").ToList(),
                TutorialState.TutForceAttack or TutorialState.TutSuccessfullyDefended => options.Where(o => o is "Attack" or "Defend").ToList(),
                TutorialState.TutForceSkills or TutorialState.TutSuccessfullyAttacked => options.Where(o => o is "Attack" or "Defend" or "Skills").ToList(),
                _ => options
            },

            "InventoryMenu" => _state switch
            {
                TutorialState.InventoryOpened => options.Where(o => o.Contains("Torch")).ToList(),
                _ => options
            },

            "EquipmentMenu" => _state switch
            {
                TutorialState.NoticeToOpenEquipment => options.Where(o => o is "Armor" or "Back").ToList(),
                _ => options
            },

            _ => options
        };
    }

    public string? ValidateChoice(string optionText, StoryContext ctx)
    {
        return _state switch
        {
            TutorialState.TutOpenMoveMenu when optionText != "Move"
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Open the {TextColor.Info}Move{TextColor.Reset} menu!",

            TutorialState.TutSelectDown when optionText != "Down" && optionText != "Back"
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}You have to go {TextColor.Info}Down!{TextColor.Reset} Investigate the rustling...",

            TutorialState.TutForceDefend when optionText != "Defend"
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Block the attack! Press {TextColor.Info}Defend!{TextColor.Reset}",

            TutorialState.TutForceAttack when optionText == "Defend"
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Get in there and use an {TextColor.Info}Attack!{TextColor.Reset} Defence won't win this fight.",

            TutorialState.TutForceAttack when optionText != "Attack"
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Get in there and use an {TextColor.Info}Attack!{TextColor.Reset}",

            TutorialState.TutForceSkills when optionText != "Skills" 
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Try using a {TextColor.Info}Skill{TextColor.Reset} to end this quickly!",
            TutorialState.TutSelectedSkills when optionText == "Back"
                => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Don't back out! Use {TextColor.Info}Skill{TextColor.Reset}Cleave to end the fight.",

            TutorialState.CombatOver when optionText != "Open Inventory"
                => "Check your rewards! Open your inventory.",

            TutorialState.InventoryOpened when optionText == "Back"
                => "Use the Torch first! You need it to see the path ahead.",

            TutorialState.InventoryOpened when !optionText.Contains("Torch")
                => "Use the Torch! Ignore the other items for now.",

            _ => null 
        };
    }

    public void OnEvent(string eventName, object payload, StoryContext ctx)
    {
        switch (eventName)
        {
            case "move_menu_opened":
                if (_state == TutorialState.TutOpenMoveMenu)
                {
                    SetState(TutorialState.TutSelectDown);
                    _director.Say($"{TextColor.Magenta}TUTORIAL: {TextColor.Reset}Investigate the noise! Head {TextColor.Info}Down.{TextColor.Reset}", false);
                }
                break;

            case "combat_started":
                if (_state == TutorialState.TutSelectDown)
                {
                    SetState(TutorialState.TutForceDefend);
                    _director.Say($"{TextColor.Magenta}TUTORIAL: {TextColor.Reset}Ah, a Slime appeared! Select {TextColor.Info}Defend{TextColor.Reset} to protect yourself!", false);
                }
                break;

            case "player_defended_success":
                if (_state == TutorialState.TutForceDefend || _state == TutorialState.TutFailedDefend)
                {
                    SetState(TutorialState.TutSuccessfullyDefended);
                    _director.Say($"{TextColor.Magenta}TUTORIAL: {TextColor.Reset}Great job on blocking that blow, not fight back by selecting {TextColor.Info}Attack!{TextColor.Reset}", false);
                    SetState(TutorialState.TutForceAttack);
                }
                break;

            case "player_defended_failed":
                if (_state == TutorialState.TutForceDefend || _state == TutorialState.TutFailedDefend)
                {
                    SetState(TutorialState.TutFailedDefend);
                    _director.Say($"{TextColor.Magenta}TUTORIAL: {TextColor.Reset}You missed the timing! Try to {TextColor.Info}Defend{TextColor.Reset} again.", false);
                    SetState(TutorialState.TutForceDefend);
                }
                break;

            case "player_attacked":
                if (_state == TutorialState.TutForceAttack)
                {
                    SetState(TutorialState.TutSuccessfullyAttacked);
                    _director.Say($"{TextColor.Magenta}TUTORIAL: {TextColor.Reset}Nice hit! Get ready to defend, enemies will get a chance to counter attack after your turn.", true);
                    SetState(TutorialState.TutForceSkills);
                }
                break;

            case "player_tried_defend_when_forced_attack":
                _director.Say("Get in there and attack! Defence won't win this fight.", true);
                break;

            case "after_attack":
                if (_state == TutorialState.TutForceSkills)
                {
                    _director.Say($"{TextColor.Magenta}TUTORIAL: {TextColor.Reset}Now that you got the basics down, I unlocked the {TextColor.Info}Skills{TextColor.Reset} option. Select {TextColor.Info}Skills{TextColor.Reset}, and finish this fight!", true);
                }
                break;

            case "skill_menu_opened":
                if (_state == TutorialState.TutForceSkills)
                {
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}This is the skill menu. Your equiptment will grant you skills to use. Each has their own minigame's and timing.", true);
                    SetState(TutorialState.TutSelectedSkills);
                }
                break;

            case "skill_used_failed":
                if (_state == TutorialState.TutSelectedSkills || _state == TutorialState.TutForceSkills)
                {
                    SetState(TutorialState.TutForceSkills);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Don't worry, I've restored your resources. Try to use {TextColor.Info}Cleave{TextColor.Reset} again.", true);
                }
                break;

            case "skill_used_successfully":
                if (_state == TutorialState.TutForceSkills || _state == TutorialState.TutSelectedSkills)
                {
                    SetState(TutorialState.TutSuccessfullyUsedSkill);
                }
                break;

            case "combat_victory":
                if (_state == TutorialState.TutSuccessfullyUsedSkill)
                {
                    SetState(TutorialState.CombatOver);

                    _director.DropItem(ItemId.Torch);
                }
                break;

            case "returned_to_exploration":
                if (_state == TutorialState.CombatOver)
                {
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Well done! A torch dropped — check your inventory.", true);
                }
                break;

            case "inventory_opened":
                if (_state == TutorialState.CombatOver)
                {
                    SetState(TutorialState.InventoryOpened);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Use the Torch to light up the area!", false);
                }
                break;

            case "torch_used":
                if (_state == TutorialState.InventoryOpened)
                {
                    SetState(TutorialState.TorchUsed);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Excellent! Go Back to the map and keep exploring.", true);
                }
                break;

            case "treasure_entered":
                if (_state == TutorialState.TorchUsed || _state == TutorialState.BackToMap)
                {
                    SetState(TutorialState.TreasureReached);
                    _director.ForceTreasureDrop(ItemId.WoodenShield);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}You found gear! Open Equipment to put it on.", true);
                    SetState(TutorialState.NoticeToOpenEquipment);
                }
                break;

            case "equipment_opened":
                if (_state == TutorialState.NoticeToOpenEquipment)
                {
                    SetState(TutorialState.SelectedEquipment);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}You have three slots: Weapon, Armor, Shield.", true);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Equipment boosts your stats and grants skills.", true);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Select Armor to equip your new shield!", true);
                    SetState(TutorialState.ExplainedEquipment);
                }
                break;

            case "shop_entered":
                if (_state >= TutorialState.ExplainedEquipment)
                {
                    SetState(TutorialState.ShopReached);
                    _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Welcome to the Shop! Spend your gold on gear.", true);
                    _director.UseShopOverride(new[] { ItemId.HealthPotion, ItemId.LeatherArmor, ItemId.ManaPotion });
                }
                break;

            case "exit_reached":
                SetState(TutorialState.TutorialComplete);
                _director.Say($"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}You found the exit! Tutorial complete.", true);
                _director.Stop();
                _director.CompleteTutorial();
                break;
        }
    }

    public string GetYellMessage()
    {
        return _state switch
        {
            TutorialState.TutOpenMoveMenu => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Open the Move menu!",
            TutorialState.TutSelectDown => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}You have to go Down!",
            TutorialState.TutForceDefend => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Block the attack! Press Defend!",
            TutorialState.TutForceAttack => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Get in there and attack!",
            TutorialState.TutForceSkills => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Try using a Skill!",
            _ => $"{TextColor.Tutorial}TUTORIAL: {TextColor.Reset}Follow the instructions!"
        };
    }
}