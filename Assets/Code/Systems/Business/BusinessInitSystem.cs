using System.Collections.Generic;
using Code.Components.Business;
using Code.Config.Business;
using Code.Service;
using Code.UI;
using Leopotam.Ecs;
using UnityEngine;
namespace Code.Systems.Business
{
    public sealed class BusinessInitSystem : IEcsInitSystem, ISaveOnExit
    {
        private readonly EcsWorld _world;
        private readonly List<BusinessConfig> _businessConfigs;
        private readonly SaveLoadService _saveLoadService;
        private readonly BusinessBindButtonService _bindButtonService;
        private readonly UiRegistry _uiRegistry;

        private readonly Dictionary<string, EcsEntity> _businessEntities = new Dictionary<string, EcsEntity>();
        public IReadOnlyDictionary<string, EcsEntity> BusinessEntities => _businessEntities;

        public BusinessInitSystem(List<BusinessConfig> businessConfigs, EcsWorld world, SaveLoadService saveLoadService
            , BusinessBindButtonService businessBindButtonService
            , UiRegistry uiRegistry)
        {
            _businessConfigs = businessConfigs;
            _world = world;
            _saveLoadService = saveLoadService;
            _bindButtonService = businessBindButtonService;
            
            _uiRegistry = uiRegistry;
        }

        public void Init()
        {
            foreach (var config in _businessConfigs)
            {
                if (config == null) continue;

                if (_businessEntities.ContainsKey(config.BusinessName))
                {
                    continue;
                }

                var entity = _world.NewEntity();

                ref var businessComponent = ref entity.Get<BusinessComponent>();
                ref var progressComponent = ref entity.Get<BusinessProgressComponent>();

                if (!_saveLoadService.LoadBusiness(config.BusinessName, out businessComponent, out progressComponent, config))
                {
                    businessComponent = new BusinessComponent
                    {
                        Config = config,
                        Level = config.StartLevel,
                        HasUpgrade1 = false,
                        HasUpgrade2 = false
                    };

                    progressComponent.Timer = 0f;
                    progressComponent.Progress = 0f;
                }

                _businessEntities[config.BusinessName] = entity;
            }

            for (int i = 0; i < _uiRegistry.Businesses.Length; i++)
            {
                var businessUI = _uiRegistry.Businesses[i];
                _bindButtonService.BindButtonLevelUp(businessUI.LevelUpButton, businessUI.BusinessNameID, _world);
                
                _bindButtonService.BindButtonUpgrade(businessUI.UpgradeButton1,businessUI.BusinessNameID,1,  _world);
                _bindButtonService.BindButtonUpgrade(businessUI.UpgradeButton2,businessUI.BusinessNameID,2,  _world);
            }
        }

        public void SaveOnExit()
        {
            Debug.Log("=== Saving business ===");

            foreach (var kvp in _businessEntities)
            {
                ref var business = ref kvp.Value.Get<BusinessComponent>();
                ref var progress = ref kvp.Value.Get<BusinessProgressComponent>();

                _saveLoadService.SaveBusiness(business, progress);
            }

            PlayerPrefs.Save();
        }
    }
}
