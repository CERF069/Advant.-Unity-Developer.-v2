using Code.Components.Business; 
using Code.UI;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.UI
{
    public sealed class BusinessUiSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilter<BusinessComponent, BusinessProgressComponent> _filter = null;
        private readonly EcsFilter<BusinessPurchaseRequest> _purchaseRequestFilter = null;
        private readonly EcsFilter<BusinessUpgradePurchaseRequest> _upgradePurchaseRequestFilter = null;
        private readonly EcsFilter<BusinessUiUpdateRequest> _uiUpdateRequestFilter = null;

        private readonly UiRegistry _uiRegistry;

        public BusinessUiSystem(UiRegistry uiRegistry)
        {
            _uiRegistry = uiRegistry;
        }

        public void Init()
        {
            foreach (var i in _filter)
            {
                ref var business = ref _filter.Get1(i);
                ref var progress = ref _filter.Get2(i);

                var uiBusiness = FindUiBusiness(business.Config.BusinessName);
                if (uiBusiness == null)
                    continue;

                UpdateUiForBusiness(uiBusiness, ref business, ref progress);
                uiBusiness.SetLevelUpButtonText(business.CalculateNextLevelCost());
            }
        }

        public void Run()
        {
            foreach (var k in _uiUpdateRequestFilter)
            {
                ref var uiUpdateRequest = ref _uiUpdateRequestFilter.Get1(k);

                var uiBusiness = FindUiBusiness(uiUpdateRequest.BusinessName);
                if (uiBusiness != null)
                {
                    foreach (var i in _filter)
                    {
                        ref var business = ref _filter.Get1(i);
                        ref var progress = ref _filter.Get2(i);

                        if (business.Config.BusinessName == uiUpdateRequest.BusinessName)
                        {
                            UpdateUiForBusiness(uiBusiness, ref business, ref progress);
                            uiBusiness.SetLevelUpButtonText(business.CalculateNextLevelCost());
                            break;
                        }
                    }
                }

                _uiUpdateRequestFilter.GetEntity(k).Destroy();
            }
            
            foreach (var i in _filter)
            {
                ref var business = ref _filter.Get1(i);
                ref var progress = ref _filter.Get2(i);

                var uiBusiness = FindUiBusiness(business.Config.BusinessName);
                if (uiBusiness != null)
                {
                    uiBusiness.SetProgress(progress.Progress);
                }
            }
        }

        private void UpdateUiForBusiness(UiBusiness uiBusiness, ref BusinessComponent business, ref BusinessProgressComponent progress)
        {
            uiBusiness.SetData(
                $"Бизнес {business.Config.BusinessName}",
                $"Уровень: {business.Level}",
                $"Доход: {business.CalculateIncome()}$",
                $"Стоимость улучшения: {business.CalculateNextLevelCost()}"
            );

            uiBusiness.SetUpgradeButtonText(
                1,
                business.Config.Upgrade1.Name,
                business.Config.Upgrade1.IncomeMultiplier,
                business.Config.Upgrade1.Price,
                business.HasUpgrade1
            );

            uiBusiness.SetUpgradeButtonText(
                2,
                business.Config.Upgrade2.Name,
                business.Config.Upgrade2.IncomeMultiplier,
                business.Config.Upgrade2.Price,
                business.HasUpgrade2
            );

            uiBusiness.SetProgress(progress.Progress);
        }

        private UiBusiness FindUiBusiness(string id)
        {
            foreach (var ui in _uiRegistry.Businesses)
            {
                if (ui.BusinessNameID == id)
                    return ui;
            }
            return null;
        }
    }
}
