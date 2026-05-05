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
    private readonly NarrativeDirector _narrativeDirector;

    public Action? OnVictory;
    public Action? OnDefeat;

    private int defenceBonus = 5;

    public CombatOrchestrator(
        Player player,
        Enemy enemy,
        CombatRulesSystem rules,
        EnemyActionSystem enemyAI,
        MinigameSystem minigames,
        LootSystem loot,
        GameLogSystem log,
        NarrativeDirector narrativeDirector)
    {
        Player = player;
        Enemy = enemy;
        _rules = rules;
        _enemyAI = enemyAI;
        _minigames = minigames;
        _loot = loot;
        _log = log;
        _narrativeDirector = narrativeDirector;
    }

    public void PlayerAttack()
    {
        int dmg = _rules.CalculateAttack(Player, Enemy);
        Enemy.HP -= dmg;

        _log.Add($"You hit {TextColor.Yellow}{Enemy.Name}{TextColor.Reset} for {TextColor.Red}{dmg}{TextColor.Reset}", waitForKey: true);
        _playerDefendedThisTurn = false;
        _narrativeDirector.OnEvent("player_attacked");

        ResolveAfterPlayer();
    }

    private bool _playerDefendedThisTurn = false;
    public void PlayerDefend()
    {
        _rules.ApplyDefense(Player, defenceBonus);
        _log.Add("You defend", sleepMs: 800);

        _playerDefendedThisTurn = true;

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
            _narrativeDirector.OnEvent("skill_used_successfully");
            ResolveAfterPlayer();
        }
        else
        {
            _log.Add($"{TextColor.Red}Skill failed! Resources wasted.{TextColor.Reset}");

            #region TUTORIAL CONTENT
            if (_narrativeDirector.IsActive && _narrativeDirector.IsTutorial)
            {
                // Refund the exact costs spent
                foreach (var cost in skill.ResourceCosts)
                {
                    if (cost.ResourceType == "Mana") Player.BaseMana += cost.Amount;
                    else if (cost.ResourceType == "Stamina") Player.Stamina += cost.Amount;
                }

                _log.Add("Momentum lost! Focus and try again.", waitForKey: true);
                _narrativeDirector.OnEvent("skill_used_failed");
            }
            #endregion
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
            _loot.ProcessLoot(Player, Enemy, _narrativeDirector);
            _log.Add($"Defeated {Enemy.Name}", sleepMs: 800);
            _narrativeDirector.OnEvent("combat_victory");
            OnVictory?.Invoke();
            return;
        }

        _enemyAI.CheckStageTransition(Enemy, _log);
        var move = _enemyAI.SelectMove(Enemy);

        _log.Add($"{TextColor.Yellow}{Enemy.Name}{TextColor.Reset} winds up {TextColor.Red}{move.Name}!{TextColor.Reset}", sleepMs: 2000);

        bool success = _minigames.Execute(move, _log);

        if (success)
        {
            _log.Add("You countered the attack!", waitForKey: true);
            #region TUTORIAL CONTENT
            if (_playerDefendedThisTurn)
                _narrativeDirector.OnEvent("player_defended_success");
            else
                _narrativeDirector.OnEvent("minigame_success");
            #endregion
        }
        else
        {
            _rules.ApplyEnemyDamage(Player, Enemy);
            _log.Add("You took damage!", waitForKey: true);
            if (_playerDefendedThisTurn)
                _narrativeDirector.OnEvent("player_defended_failed");
            else
                _narrativeDirector.OnEvent("minigame_failed");
        }
        _narrativeDirector.OnEvent("after_attack");

        if (_playerDefendedThisTurn)
        {
            _rules.RemoveDefence(Player, defenceBonus);
        }
        _playerDefendedThisTurn = false;

        if (Player.HP <= 0)
            OnDefeat?.Invoke();
    }

    public void UseItem(ItemId id)
    {
        var item = ItemDatabase.Items[id];
        bool success = true;

        if (item.GrantedSkills.Count > 0)
        {
            var skillId = item.GrantedSkills[0];
            var skill = SkillDatabase.Skills[skillId];

            success = _minigames.ExecuteSkill(skill, _log);
        }

        if (success)
        {
            var result = InventorySystem.UseItem(Player, id, Enemy, null);
            if (!string.IsNullOrEmpty(result.Message))
                _log.Add(result.Message);

            _narrativeDirector.OnEvent("item_used_success");
        }
        else
        {
            _log.Add($"{TextColor.Red}You fumbled the {item.Name}!{TextColor.Reset}");
        }

        ResolveAfterPlayer();
    }
}