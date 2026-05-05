namespace T4Dungeon.Game.Models
{
    using T4Dungeon.Generated;

    public class Enemy
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int MaxHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int CurrentStageId { get; set; } = -1;
        public List<MoveDef> Moves { get; set; } = new();

        // Keep reference to def so stages are accessible at runtime
        public EnemyDef Def { get; private set; }

        public bool HasStages => Def.Stages != null && Def.Stages.Count > 0;

        public Enemy(EnemyId id)
        {
            var def = EnemyDatabase.Enemies[id];
            Def = def;
            Name = def.Name;
            HP = def.HP;
            MaxHp = def.MaxHp;
            Attack = def.Attack;
            Defense = 0;
            Moves = def.Moves;
        }

        public Enemy() : this(EnemyId.GreenSlime) { }
    }
}