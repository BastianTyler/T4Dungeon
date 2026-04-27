namespace T4Dungeon.Game.Systems
{
    using T4Dungeon.Game.Core;

    public class TutorialManager
    {
        public TutorialState CurrentState { get; private set; } = TutorialState.None;
        public bool IsActive => CurrentState != TutorialState.None && CurrentState != TutorialState.Complete;

        public void Start() => CurrentState = TutorialState.StartExploration;

        public void Advance()
        {
            if (CurrentState < TutorialState.Complete)
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
                TutorialState.StartExploration => "Investigate the noise!",
                TutorialState.CombatFirstContact => "Block the attack!",
                TutorialState.DefendUsed => "Now counter-attack!",
                TutorialState.LootInventory => "Check your rewards!",
                _ => "That's not allowed right now."
            };
        }
        #endregion
    }
}