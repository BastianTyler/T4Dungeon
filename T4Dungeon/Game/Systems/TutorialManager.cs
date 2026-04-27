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
    }
}