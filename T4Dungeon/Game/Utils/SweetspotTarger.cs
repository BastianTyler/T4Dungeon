namespace T4Dungeon.Game.Models
{
    public struct SweetspotTarget
    {
        public float Center; // Position on the bar (0.0 to 1.0)
        public float Width;  // How wide the hit zone is
        public bool IsHit;   // Whether the player successfully tapped this one
    }
}