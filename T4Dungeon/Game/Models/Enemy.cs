namespace T4Dungeon.Game.Models
{
    using T4Dungeon.Generated;

    public class Enemy
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }

        public Enemy(EnemyId id)
        {
            var def = EnemyDatabase.Enemies[id];
            Name = def.Name;
            HP = def.HP;
            Attack = def.Attack;
        }

        public Enemy() : this(EnemyId.Slime) { }
    }
}