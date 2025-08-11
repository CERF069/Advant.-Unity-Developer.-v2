using Code.Components.Business;
using Code.Components.Player.Balance;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.Business
{
    public sealed class BusinessPurchaseSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;

        private readonly EcsFilter<BusinessComponent> _businessFilter = null;
        private readonly EcsFilter<BusinessPurchaseRequest> _purchaseRequestFilter = null;
        private readonly EcsFilter<BusinessUpgradePurchaseRequest> _upgradePurchaseRequestFilter = null;
        
        private readonly EcsFilter<PlayerBalance> _balanceFilter = null;

        public void Run()
        {
            if (_balanceFilter.IsEmpty()) return;

            ref var playerBalance = ref _balanceFilter.Get1(0);

            foreach (var i in _purchaseRequestFilter)
            {
                ref var purchaseRequest = ref _purchaseRequestFilter.Get1(i);

                // Ищем бизнес по имени
                foreach (var j in _businessFilter)
                {
                    ref var business = ref _businessFilter.Get1(j);

                    if (business.Name == purchaseRequest.BusinessName)
                    { 
                        TryBuyLevelUp(ref playerBalance, ref business);
                    }
                }
                _purchaseRequestFilter.GetEntity(i).Destroy();
            }
            
            foreach (var i in _upgradePurchaseRequestFilter)
            {
                ref var purchaseRequest = ref _upgradePurchaseRequestFilter.Get1(i);

                // Ищем бизнес по имени
                foreach (var j in _businessFilter)
                {
                    ref var business = ref _businessFilter.Get1(j);

                    if (business.Name == purchaseRequest.BusinessName)
                    {
                        TryBuyUpgrade(ref playerBalance, ref business, purchaseRequest.UpgradeIndex);
                      
                    }
                }
                _upgradePurchaseRequestFilter.GetEntity(i).Destroy();
            }
        }
        private void TryBuyLevelUp(ref PlayerBalance playerBalance, ref BusinessComponent business)
        {
            int cost = business.CalculateNextLevelCost();

            if (playerBalance.Value >= cost)
            {
                var requestEntity = _world.NewEntity();
                ref var changeRequest = ref requestEntity.Get<PlayerBalanceChangeRequest>();
                changeRequest.Delta = -cost;

                business.Level++;

                // Создаем запрос на обновление UI
                var uiUpdateEntity = _world.NewEntity();
                ref var uiUpdateRequest = ref uiUpdateEntity.Get<BusinessUiUpdateRequest>();
                uiUpdateRequest.BusinessName = business.Name;

                Debug.Log($"Бизнес '{business.Name}' улучшен до уровня {business.Level}. Списано {cost} монет.");
            }
            else
            {
                Debug.Log($"Недостаточно средств для апгрейда '{business.Name}'. Нужно {cost}, есть {playerBalance.Value}.");
            }
        }

        private void TryBuyUpgrade(ref PlayerBalance playerBalance, ref BusinessComponent business, int upgradeIndex)
        {
            if (business.Level == 0) return;

            if (upgradeIndex != 1 && upgradeIndex != 2) return;

            if (upgradeIndex == 1 && business.HasUpgrade1) return;
            if (upgradeIndex == 2 && business.HasUpgrade2) return;

            int cost = upgradeIndex == 1 ? business.Config.Upgrade1.Price : business.Config.Upgrade2.Price;

            if (playerBalance.Value >= cost)
            {
                var requestEntity = _world.NewEntity();
                ref var changeRequest = ref requestEntity.Get<PlayerBalanceChangeRequest>();
                changeRequest.Delta = -cost;

                if (upgradeIndex == 1) business.HasUpgrade1 = true;
                else business.HasUpgrade2 = true;

                // Запрос на обновление UI
                var uiUpdateEntity = _world.NewEntity();
                ref var uiUpdateRequest = ref uiUpdateEntity.Get<BusinessUiUpdateRequest>();
                uiUpdateRequest.BusinessName = business.Name;

                Debug.Log($"Бизнес '{business.Name}' получил апгрейд {upgradeIndex}. Списано {cost} монет.");
            }
        }

    }
}
