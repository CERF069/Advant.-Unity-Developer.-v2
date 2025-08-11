using Code.Components.Player.Balance;
using Code.Service;
using Code.UI;
using Leopotam.Ecs;

namespace Code.Systems.Player.Balance
{
    /// <summary>
    /// Initializes player balance entity with saved value and links it to the UI.
    /// </summary>
    public sealed class PlayerBalanceInitSystem : IEcsInitSystem
    {
        private readonly UiRegistry _uiRegistry;
        private readonly EcsWorld _world;
        private readonly SaveLoadService _saveLoadService;

        public PlayerBalanceInitSystem(UiRegistry uiRegistry, EcsWorld world, SaveLoadService saveLoadService)
        {
            _uiRegistry = uiRegistry;
            _world = world;
            _saveLoadService = saveLoadService;
        }

        public void Init()
        {
            var entity = _world.NewEntity();

            // Initialize balance from save data
            ref var balance = ref entity.Get<PlayerBalance>();
            balance.Value = _saveLoadService.LoadBalance();

            // Link UI to entity
            ref var ui = ref entity.Get<PlayerBalanceUi>();
            ui.BalanceText = _uiRegistry.BalanceText;
        }
    }
}