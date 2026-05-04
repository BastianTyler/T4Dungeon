using T4Dungeon.Game.Core;
using T4Dungeon.Game.Models;
using T4Dungeon.Game.Systems;
using T4Dungeon.Game.Utils;
using T4Dungeon.Generated;

namespace T4Dungeon.Game.States
{
    internal class ShopState : IGameState
    {
        private readonly StateMachine _fsm;
        private readonly InputSystem _input;
        private readonly GameLogSystem _log;
        private readonly MapManager _mapMan;
        private ShopSlot.ShopInstance _currentShop;
        private Player _player;
        private UIContext _ui;

        public Action? OnExit;

        public ShopState(StateMachine fsm, InputSystem input, GameLogSystem log, MapManager mapMan, Player player, NarrativeDirector narrativeDirector) 
        { 
            _fsm = fsm;
            _input = input;
            _log = log;
            _mapMan = mapMan;
            _player = player;
        }

        public void StartShop()
        {
            _currentShop = new ShopSlot.ShopInstance();
            _currentShop.GenerateInventory();

            SetShopWelcomeMenu();
        }

        public void Enter() { _log.OnLogAdded += ForceRender; }

        public void Update()
        {
            if (_ui == null) return;
            ForceRender();
            int choice = _input.GetSelection(_ui.Options.Count);
            _ui.Options[choice].Action?.Invoke();
        }

        public void Exit() { _log.OnLogAdded -= ForceRender; }

        private void SetShopWelcomeMenu()
        {
            _ui = MenuFactory.CreateShopWelcomeMenu(
                onBrowse: SetShopBuyMenu,
                onLeave: () => OnExit?.Invoke()
            );
        }

        private void SetShopBuyMenu()
        {
            _ui = MenuFactory.CreateShopBuyMenu(
                _currentShop,
                onBuy: BuyItem,
                onBack: SetShopWelcomeMenu
            );
        }

        private void BuyItem(ShopSlot slot)
        {
            if (_currentShop.PurchaseItem(slot, _player)) 
            {
                _log.Add($"Purchased {ItemDatabase.Items[slot.ItemId].Name}!", true);
                SetShopBuyMenu();
            }
            else
            {
                _log.Add("Not enough gold!", true);
            }
        }

        private void ForceRender()
        {
            //ConsoleRenderer.Render(_mapMan, _ui, _log.Active.ToList(), null, false, false, null);
            ConsoleRenderer.Render(GameStateType.Shop, _ui, _log.Active.ToList(), _player, false, _mapMan, null);
        }
    }
}