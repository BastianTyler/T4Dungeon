namespace T4Dungeon.Game.Systems
{
    using T4Dungeon.Game.Core;

    public class TutorialManager
    {
        public TutorialState CurrentState { get; private set; } = TutorialState.None;

        // Fixed: Replaced 'Complete' with 'ShieldEquipped'
        public bool IsActive => CurrentState != TutorialState.None && CurrentState != TutorialState.ShieldEquipped;

        // Fixed: Replaced 'StartExploration' with 'TutOpenMoveMenu'
        public void Start() => CurrentState = TutorialState.TutOpenMoveMenu;

        public void Advance()
        {
            // Fixed: Replaced 'Complete' with 'ShieldEquipped'
            if (CurrentState < TutorialState.ShieldEquipped)
            {
                CurrentState++;
            }
        }

        public void SetState(TutorialState state) => CurrentState = state;

        #region TUTORIAL CONTENT
        public string GetYellMessage()
        {
            return CurrentState switch
            {
                // Fixed: Replaced old states with the new sequence names
                TutorialState.TutOpenMoveMenu => "Open the Move menu!",
                TutorialState.TutSelectDown => "Investigate the noise! You have to go Down!",
                TutorialState.TutForceDefend => "Block the attack! Press 'D'!",
                TutorialState.TutForceAttack => "Get in there and attack! You can't win this fight with just defence!",
                TutorialState.TutForceSkills => "Try using a Skill to end this quickly!",
                TutorialState.TutExplainedSkills => "Don't back out now—choose a skill to use!",
                TutorialState.CombatOver => "Check your rewards! Open your inventory.",
                _ => "Follow the instructions!"
            };
        }
        #endregion
    }
}