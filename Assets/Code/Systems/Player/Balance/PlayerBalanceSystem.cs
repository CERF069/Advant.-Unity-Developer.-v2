using Code.Components.Player.Balance;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.Player.Balance
{
    /// <summary>
    /// Handles balance change requests for the player.
    /// Processes all incoming change requests, updates balance, and triggers UI refresh.
    /// </summary>
    public sealed class PlayerBalanceSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerBalance> _balanceFilter = null;
        private readonly EcsFilter<PlayerBalanceChangeRequest> _changeRequestFilter = null;

        public void Run()
        {
            if (_balanceFilter.IsEmpty() || _changeRequestFilter.IsEmpty())
                return;

            foreach (var balanceIdx in _balanceFilter)
            {
                ref var balance = ref _balanceFilter.Get1(balanceIdx);
                ref var balanceEntity = ref _balanceFilter.GetEntity(balanceIdx);

                foreach (var reqIdx in _changeRequestFilter)
                {
                    ref var requestEntity = ref _changeRequestFilter.GetEntity(reqIdx);
                    ref var request = ref _changeRequestFilter.Get1(reqIdx);

                    float newValue = balance.Value + request.Delta;

                    // Prevent negative balance
                    if (newValue < 0)
                    {
                        Debug.LogWarning($"[BalanceSystem] Balance change denied: insufficient funds (Current: {balance.Value}, Attempted change: {request.Delta})");
                        requestEntity.Del<PlayerBalanceChangeRequest>();
                        continue;
                    }

                    // Apply change if value is different
                    if (!Mathf.Approximately(newValue, balance.Value))
                    {
                        Debug.Log($"[BalanceSystem] Balance updated: {balance.Value} -> {newValue} (Change: {request.Delta})");
                        balance.Value = newValue;
                        balanceEntity.Get<PlayerBalanceChanged>(); // Triggers UI update
                    }

                    // Mark request as processed
                    requestEntity.Del<PlayerBalanceChangeRequest>();
                }
            }
        }
    }
}
