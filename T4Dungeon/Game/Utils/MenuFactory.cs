using System;
using System.Collections.Generic;
using System.Linq;
using T4Dungeon.Game.Models;
using T4Dungeon.Generated;
using static T4Dungeon.Game.Models.ShopSlot;

namespace T4Dungeon.Game.Utils
{
    public static class MenuFactory
    {
        public static UIContext CreateMainMenu(Action onMove, Action onEquip, Action onInv, Action onExit)
        {
            return new UIContext
            {
                Options = new List<MenuOption>
                {
                    new MenuOption { Text = "Move", Action = onMove },
                    new MenuOption { Text = "Equipment", Action = onEquip },
                    new MenuOption { Text = "Open Inventory", Action = onInv },
                    new MenuOption { Text = "Interact", Action = null, IsImplemented = false },
                    new MenuOption { Text = "Exit Game", Action = onExit }
                }
            };
        }

        public static UIContext CreateMoveMenu(Action up, Action down, Action left, Action right, Action back)
        {
            return new UIContext
            {
                Options = new List<MenuOption>
                {
                    new MenuOption { Text = "Up", Action = up },
                    new MenuOption { Text = "Down", Action = down },
                    new MenuOption { Text = "Left", Action = left },
                    new MenuOption { Text = "Right", Action = right },
                    new MenuOption { Text = "Back", Action = back }
                }
            };
        }

        public static UIContext CreateInventoryMenu(Player player, Action<ItemId> onUse, Action onBack)
        {
            var ui = new UIContext { Options = new List<MenuOption>() };

            foreach (var item in player.Inventory.Items)
            {
                var def = ItemDatabase.Items[item.ItemId];
                ui.Options.Add(new MenuOption
                {
                    Text = $"{def.Name} (x{item.Amount})",
                    Action = () => onUse(item.ItemId),
                    IsImplemented = def.IsConsumable
                });
            }

            ui.Options.Add(new MenuOption { Text = "Back", Action = onBack });
            return ui;
        }

        public static UIContext CreateCombatMenu(Action onAttack, Action onDefend, Action onFlee, Action onInv)
        {
            return new UIContext
            {
                Options = new List<MenuOption>
                {
                    new MenuOption { Text = "Attack", Action = onAttack },
                    new MenuOption { Text = "Defend", Action = onDefend },
                    new MenuOption { Text = "Attempt Flee", Action = onFlee },
                    new MenuOption { Text = "Open Inventory", Action = onInv }
                }
            };
        }

        public static UIContext CreateEquipmentMenu(Player player, Action<EquiptSlot> onSelectSlot, Action onBack)
        {
            var ui = new UIContext { Options = new List<MenuOption>() };
            var slots = new[] { EquiptSlot.Weapon, EquiptSlot.Armor, EquiptSlot.Accessory };

            foreach (var slot in slots)
            {
                player.Equipment.TryGetValue(slot, out var id);
                string itemName = id.HasValue ? ItemDatabase.Items[id.Value].Name : "Empty";
                ui.Options.Add(new MenuOption { Text = $"{slot}: {itemName}", Action = () => onSelectSlot(slot) });
            }

            ui.Options.Add(new MenuOption { Text = "Back", Action = onBack });
            return ui;
        }

        public static UIContext CreateItemSelectMenu(Player player, EquiptSlot slot, Action<ItemId> onEquip, Action onBack)
        {
            var ui = new UIContext { Options = new List<MenuOption>() };
            var validItems = player.Inventory.Items
                .Select(i => ItemDatabase.Items[i.ItemId])
                .Where(def => def.Slot == slot)
                .Take(9).ToList();

            foreach (var item in validItems)
            {
                ui.Options.Add(new MenuOption { Text = item.Name, Action = () => onEquip(item.Id) });
            }

            ui.Options.Add(new MenuOption { Text = "Back", Action = onBack });
            return ui;
        }

        public static UIContext CreateShopWelcomeMenu(Action onBrowse, Action onLeave)
        {
            return new UIContext
            {
                Options = new List<MenuOption>
                {
                    new MenuOption { Text = "Browse Wares", Action = onBrowse },
                    new MenuOption { Text = "Leave Shop (Merchant will leave)", Action = onLeave }
                }
            };
        }

        public static UIContext CreateShopBuyMenu(ShopInstance shop, Action<ShopSlot> onBuy, Action onBack)
        {
            var ui = new UIContext { Options = new List<MenuOption>() };

            foreach (var slot in shop.Inventory)
            {
                var item = ItemDatabase.Items[slot.ItemId];
                string status = slot.IsSold ? "[SOLD OUT]" : $"{slot.Price}g";
                if (slot.IsDiscounted && !slot.IsSold) status += " %SALE%";

                ui.Options.Add(new MenuOption
                {
                    Text = $"{item.Name.PadRight(18)} | {status}",
                    Action = () => onBuy(slot),
                    IsImplemented = !slot.IsSold
                });
            }

            ui.Options.Add(new MenuOption { Text = "Back", Action = onBack });
            return ui;
        }
    }
}