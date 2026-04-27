using T4Dungeon.Generated;
using System;
using System.Collections.Generic;
using System.Linq;

namespace T4Dungeon.Game.Models;

public class Player
{
    public int MaxHP { get; set; } = 100;
    public int HP { get; set; } = 100;
    public int BaseAttack { get; set; } = 10;
    public int BaseDefense { get; set; } = 5;
    public int MaxMana { get; set; } = 20;
    public int BaseMana { get; set; } = 0;
    public int MaxStamina { get; set; } = 50;
    public int Stamina { get; set; } = 50;
    public int Gold { get; set; } = 0;
    public bool IsDead => HP <= 0;
    public bool IsDefending { get; set; }
    public Inventory Inventory { get; } = new Inventory();

    public Dictionary<EquiptSlot, ItemId?> Equipment { get; } = new()
    {
        { EquiptSlot.Weapon, null },
        { EquiptSlot.Armor, null },
        { EquiptSlot.Accessory, null }
    };

    // --- Calculated Stats ---
    // These recalculate automatically whenever they are "read"
    public int Attack => BaseAttack + GetGearBonus(item => item.AttackBonus);
    public int Defense => BaseDefense + GetGearBonus(item => item.DefenseBonus);

    // Helper to sum up bonuses from all equipped slots
    private int GetGearBonus(Func<ItemDef, int> statSelector)
    {
        return Equipment.Values
            .Where(id => id.HasValue)
            .Select(id => ItemDatabase.Items[id.Value])
            .Sum(statSelector);
    }

    public void TakeDamage(int dmg)
    {
        // Use the calculated 'Defense' (Base + Gear)
        int final = Math.Max(1, dmg - Defense);
        HP -= final;
    }
}