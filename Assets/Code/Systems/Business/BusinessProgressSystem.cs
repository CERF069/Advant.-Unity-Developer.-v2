using Code.Components.Business;
using Code.Components.Player.Balance;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.Business
{
    /// <summary>
    /// Handles progress tracking for businesses, generating income when production cycles are completed.
    /// </summary>
    public sealed class BusinessProgressSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;

        // Businesses that don't have a progress component yet
        private readonly EcsFilter<BusinessComponent>.Exclude<BusinessProgressComponent> _newBusinesses = null;

        // Businesses that already have progress tracking
        private readonly EcsFilter<BusinessComponent, BusinessProgressComponent> _businessWithProgressFilter = null;

        public void Run()
        {
            // Add BusinessProgressComponent to new businesses
            foreach (var i in _newBusinesses)
            {
                var entity = _newBusinesses.GetEntity(i);

                ref var progress = ref entity.Get<BusinessProgressComponent>();
                progress.Timer = 0f;
                progress.Progress = 0f;

                Debug.Log($"[BusinessProgressSystem] Added progress tracking for entity {entity}.");
            }

            // Update progress for existing businesses
            foreach (var i in _businessWithProgressFilter)
            {
                ref var business = ref _businessWithProgressFilter.Get1(i);
                ref var progress = ref _businessWithProgressFilter.Get2(i);
                var entity = _businessWithProgressFilter.GetEntity(i);

                //Debug.Log($"[BusinessProgressSystem] Processing business '{business.Config.BusinessName}', Level: {business.Level}, Timer: {progress.Timer:F2}, Progress: {progress.Progress:P0}");

                // Skip inactive businesses
                if (business.Level <= 0)
                {
                    //Debug.Log($"[BusinessProgressSystem] Business '{business.Config.BusinessName}' is inactive (Level <= 0). Skipping update.");
                    continue;
                }

                // Update progress timer
                progress.Timer += Time.deltaTime;
                progress.Progress = Mathf.Clamp01(progress.Timer / business.Delay);

                //Debug.Log($"[BusinessProgressSystem] Updated progress for '{business.Config.BusinessName}': Timer={progress.Timer:F2}, Progress={progress.Progress:P0}");

                // If progress completed, generate income
                if (progress.Timer >= business.Delay)
                {
                    var income = (int)business.CalculateIncome();

                    Debug.Log($"[BusinessProgressSystem] Business '{business.Config.BusinessName}' completed a cycle. Income generated: +{income}");

                    var balanceEntity = _world.NewEntity();
                    ref var request = ref balanceEntity.Get<PlayerBalanceChangeRequest>();
                    request.Delta = income;

                    progress.Timer = 0f;
                    progress.Progress = 0f;
                }
            }
        }
    }
}
