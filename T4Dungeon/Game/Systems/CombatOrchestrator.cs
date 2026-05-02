using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

public class CombatOrchestrator
{
    public Player Player { get; }
    public Enemy Enemy { get; }

    private readonly CombatRulesSystem _rules;
    private readonly EnemyActionSystem _enemyAI;
    private readonly MinigameSystem _minigames;
    private readonly GameLogSystem _log;
    private readonly LootSystem _loot;

    public Action? OnVictory;
    public Action? OnDefeat;

    public CombatOrchestrator(
        Player player,
        Enemy enemy,
        CombatRulesSystem rules,
        EnemyActionSystem enemyAI,
        MinigameSystem minigames,
        LootSystem loot,
        GameLogSystem log)
    {
        Player = player;
        Enemy = enemy;
        _rules = rules;
        _enemyAI = enemyAI;
        _minigames = minigames;
        _loot = loot;
        _log = log;

        //_log.Add($"Encountered {enemy.Name}", waitForKey: true);
    }

    public void PlayerAttack()
    {
        int dmg = _rules.CalculateAttack(Player, Enemy);
        Enemy.HP -= dmg;

        _log.Add($"You hit {TextColor.Yellow}{Enemy.Name}{TextColor.Reset} for {TextColor.Red}{dmg}{TextColor.Reset}", waitForKey: true);

        ResolveAfterPlayer();
    }

    public void PlayerDefend()
    {
        _rules.ApplyDefense(Player);
        _log.Add("You defend", sleepMs: 800);

        ResolveAfterPlayer();
    }

    public bool TryFlee()
    {
        return _rules.RollFlee(Player, Enemy);
    }

    public void UseSkill(SkillId id)
    {
        var skill = SkillDatabase.Skills[id];

        // Validate resources
        foreach (var cost in skill.ResourceCosts)
        {
            bool hasEnough = cost.ResourceType switch
            {
                "Mana" => Player.BaseMana >= cost.Amount,
                "Stamina" => Player.Stamina >= cost.Amount,
                "HP" => Player.HP > cost.Amount,
                _ => true
            };

            if (!hasEnough)
            {
                _log.Add($"{TextColor.Red}Not enough {cost.ResourceType}!{TextColor.Reset}");
                return;
            }
        }

        // Deduct resources
        foreach (var cost in skill.ResourceCosts)
        {
            if (cost.ResourceType == "Mana") Player.BaseMana -= cost.Amount;
            if (cost.ResourceType == "Stamina") Player.Stamina -= cost.Amount;
            if (cost.ResourceType == "HP") Player.HP -= cost.Amount;
        }

        // Run minigame steps
        bool success = _minigames.ExecuteSkill(skill, _log);

        if (success)
        {
            ApplySkillEffects(skill);
            _log.Add($"{TextColor.Green}Success!{TextColor.Reset} {skill.Name} executed.");
            ResolveAfterPlayer();
        }
        else
        {
            _log.Add($"{TextColor.Red}Skill failed! Resources wasted.{TextColor.Reset}");
        }
    }

    private void ApplySkillEffects(SkillDef skill)
    {
        switch (skill.SkillType)
        {
            case "Damage":
                int dmg = Player.Attack + skill.Value;
                Enemy.HP -= dmg;
                _log.Add($"Dealt {TextColor.Yellow}{dmg}{TextColor.Reset} damage to {TextColor.Red}{Enemy.Name}{TextColor.Reset}!");
                break;

            case "Healing":
                Player.HP = Math.Min(Player.MaxHP, Player.HP + skill.Value);
                _log.Add($"Restored {TextColor.Green}{skill.Value}{TextColor.Reset} HP!");
                break;

            case "Mana":
                Player.BaseMana += skill.Value;
                _log.Add($"Restored {TextColor.Cyan}{skill.Value}{TextColor.Reset} Mana!");
                break;
        }
    }

    public void ResolveAfterPlayer()
    {
        if (Enemy.HP <= 0)
        {
            _loot.ProcessLoot(Player, Enemy);
            _log.Add($"Defeated {Enemy.Name}", sleepMs: 800);
            OnVictory?.Invoke();
            return;
        }

        var move = _enemyAI.SelectMove(Enemy);

        _log.Add($"{TextColor.Yellow}{Enemy.Name}{TextColor.Reset} winds up {TextColor.Red}{move.Name}!{TextColor.Reset}", sleepMs: 2000);

        bool success = _minigames.Execute(move);

        if (success)
            _log.Add("You countered the attack!", waitForKey: true);
        else
        {
            _rules.ApplyEnemyDamage(Player, Enemy);
            _log.Add("You took damage!", waitForKey: true);
        }

        if (Player.HP <= 0)
            OnDefeat?.Invoke();
    }
}