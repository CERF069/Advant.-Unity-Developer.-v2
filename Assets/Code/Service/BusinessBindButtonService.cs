using Code.Components.Business;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Service
{
    /// <summary>
    /// Service responsible for binding UI buttons to ECS events related to businesses.
    /// </summary>
    public class BusinessBindButtonService
    {
        /// <summary>
        /// Binds a button click to a business level-up request.
        /// </summary>
        /// <param name="button">The UI button to bind.</param>
        /// <param name="businessName">The name of the business to upgrade.</param>
        /// <param name="world">The ECS world instance.</param>
        public void BindButtonLevelUp(Button button, string businessName, EcsWorld world)
        {
            if (button == null || string.IsNullOrEmpty(businessName) || world == null)
                return;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                var entity = world.NewEntity();
                ref var request = ref entity.Get<BusinessPurchaseRequest>();
                request.BusinessName = businessName;

                Debug.Log($"[BusinessBindButtonService] Level-up button clicked for business: {businessName}");
            });
        }

        /// <summary>
        /// Binds a button click to a business upgrade purchase request.
        /// </summary>
        /// <param name="button">The UI button to bind.</param>
        /// <param name="businessName">The name of the business to upgrade.</param>
        /// <param name="indexUpgrade">The index of the upgrade.</param>
        /// <param name="world">The ECS world instance.</param>
        public void BindButtonUpgrade(Button button, string businessName, int indexUpgrade, EcsWorld world)
        {
            if (button == null || string.IsNullOrEmpty(businessName) || world == null)
                return;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                var entity = world.NewEntity();
                ref var request = ref entity.Get<BusinessUpgradePurchaseRequest>();
                request.BusinessName = businessName;
                request.UpgradeIndex = indexUpgrade;

                Debug.Log($"[BusinessBindButtonService] Upgrade button clicked for business: {businessName}, Upgrade Index: {request.UpgradeIndex}");
            });
        }
    }
}
