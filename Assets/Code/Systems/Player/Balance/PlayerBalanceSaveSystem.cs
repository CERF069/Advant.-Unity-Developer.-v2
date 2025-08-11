using Code.Components.Player.Balance;
using Code.Service;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.Player.Balance
{
    /// <summary>
    /// Saves player balance to persistent storage on change or on application exit.
    /// Uses cooldown to avoid excessive disk writes.
    /// </summary>
    public sealed class PlayerBalanceSaveSystem : IEcsRunSystem, ISaveOnExit
    {
        private readonly SaveLoadService _saveLoadService;
        private readonly EcsFilter<PlayerBalance, PlayerBalanceChanged> _changedBalanceFilter = null;

        private const float SaveCooldown = 1f; // Delay before saving to prevent spam
        private float _timer;
        private bool _needSave;
        private float _lastBalanceValue;

        public PlayerBalanceSaveSystem(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Run()
        {
            // Check for balance changes
            foreach (var i in _changedBalanceFilter)
            {
                ref var balance = ref _changedBalanceFilter.Get1(i);

                _needSave = true;
                _lastBalanceValue = balance.Value;

                // Remove "changed" marker to avoid duplicate processing
                _changedBalanceFilter.GetEntity(i).Del<PlayerBalanceChanged>();
            }

            // Handle delayed save
            if (_needSave)
            {
                _timer += Time.deltaTime;
                if (_timer >= SaveCooldown)
                {
                    SaveBalance();
                }
            }
        }

        /// <summary>
        /// Saves the balance instantly on application exit.
        /// </summary>
        public void SaveOnExit()
        {
            if (_needSave)
            {
                SaveBalance();
            }
        }

        /// <summary>
        /// Performs the actual save and resets save flags.
        /// </summary>
        private void SaveBalance()
        {
            _saveLoadService.SaveBalance(_lastBalanceValue);
            Debug.Log($"[BalanceSaveSystem] Balance saved: {_lastBalanceValue}");
            _timer = 0f;
            _needSave = false;
        }
    }
}
