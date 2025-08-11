using System.Globalization;
using Code.Components.Player.Balance;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.Player.Balance
{
    sealed class PlayerBalanceUiSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerBalance, PlayerBalanceUi, PlayerBalanceChanged> _changedFilter = null;
        private readonly EcsFilter<PlayerBalance, PlayerBalanceUi> _allFilter = null;
        private bool _initialized = false;

        public void Run()
        {
            if (!_initialized)
            {
                foreach (var i in _allFilter)
                {
                    ref var balance = ref _allFilter.Get1(i);
                    ref var ui = ref _allFilter.Get2(i);

                    ui.BalanceText.text = $"{balance.Value.ToString(CultureInfo.InvariantCulture)}$";
                }
                _initialized = true;
            }
            foreach (var i in _changedFilter)
            {
                ref var balance = ref _changedFilter.Get1(i);
                ref var ui = ref _changedFilter.Get2(i);

                ui.BalanceText.text = $"{balance.Value.ToString(CultureInfo.InvariantCulture)}$";
                
            }
        }
    }

}
