public enum TutorialState
{
    // SETUP
    None, TutStarts, TutInitializes, TutLoads,

    // NAVIGATION
    TutOpenMoveMenu, TutSelectDown, TutCombatEncounterStarts,

    // COMBAT CYCLE
    TutForceDefend, TutFailedDefend, TutSuccessfullyDefended,
    TutForceAttack, TutSuccessfullyAttacked,
    TutForceSkills, TutSelectedSkills, TutExplainedSkills,
    TutSuccessfullyUsedSkill, TutFailedToUseSkill,

    // POST-COMBAT / INVENTORY
    CombatOver, InventoryOpened, ExplainedInventory, TorchUsed,

    //TREASURE
    BackToMap, TreasureReached, NoticeToOpenEquipment,

    // EQUIPMENT
    SelectedEquipment, ExplainedEquipment,

    //SHOP
    ShopReached,
    //Done
    TutorialComplete
}