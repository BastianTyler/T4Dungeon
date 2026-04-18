
namespace T4Dungeon.Game.Models;

public class Player
{
    public int HP { get; set; } = 100;
    public bool IsDead => HP <= 0;

    public int Attack { get; set; } = 10;
    public int Defense { get; set; } = 5;

    public void TakeDamage(int dmg)
    {
        int final = Math.Max(0, dmg - Defense);
        HP -= final;
    }
}
