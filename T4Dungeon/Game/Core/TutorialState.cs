namespace T4Dungeon.Game.Core
{
    public enum TutorialState
    {
        None,               // Tutorial not active
        StartExploration,   // step 0: Forcing movement
        CombatFirstContact, // step 1: First turn logic
        DefendUsed,         // step 2: Player has defended
        AttackTaught,       // step 3: Player must attack
        SkillsUnlocked,      // step 4: Skill usage tutorial
        LootInventory,      // step 5: Forced inventory open
        UsePotion,          // step 6: Using the health potion
        EquipShield,        // step 7: Forcing equipment menu
        Complete            // step 8: Handing control back to player
    }
}