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
    CombatOver, InventoryOpened, ExplainedInventory, UsedItem,

    // EQUIPMENT
    SelectedEquipment, ExplainEquipment, ExplainedEquipment,
    SelectedArmorTab, ShieldEquipped
}