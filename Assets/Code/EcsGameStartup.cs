using System.Collections.Generic;
using Code.Config.Business;
using Code.Systems.Business;
using Code.Components.Player.Balance;
using Code.Service;
using Code.Systems.Player.Balance;
using Code.Systems.UI;
using Code.UI;
using Leopotam.Ecs;
using UnityEngine;

namespace Code
{
    /// <summary>
    /// Interface for systems that should save data when the application exits.
    /// </summary>
    public interface ISaveOnExit
    {
        void SaveOnExit();
    }

    /// <summary>
    /// Entry point for ECS game initialization and systems management.
    /// </summary>
        public sealed class EcsGameStartup : MonoBehaviour
        {
            [Header("UI References")] [SerializeField]
            private UiRegistry _uiRegistry;

            [Header("Business Configurations")] [SerializeField]
            private List<BusinessConfig> _businessConfigs;

            // ECS core
            private EcsWorld _world;
            private EcsSystems _systems;

            // Services
            private SaveLoadService _saveLoadService;
            private BusinessBindButtonService _bindButtonService;

            // Registered systems
            private readonly List<IEcsSystem> _allSystems = new List<IEcsSystem>();

            #region Unity Lifecycle

            private void Awake()
            {
                _world = new EcsWorld();
                _systems = new EcsSystems(_world);

                _saveLoadService = new SaveLoadService();
                _bindButtonService = new BusinessBindButtonService();
            }

            private void Start()
            {
                AddSystems();
                AddOneFrameComponents();

                _systems.Init();
            }

            private void Update()
            {
                _systems?.Run();
            }

            private void OnApplicationPause(bool pause)
            {
                if (pause) // приложение уходит в фон
                {
                    if (_systems == null) return;

                    Debug.Log("Application paused — saving data.");

                    foreach (var system in _allSystems)
                    {
                        if (system is ISaveOnExit saveSystem)
                        {
                            saveSystem.SaveOnExit();
                        }
                    }

                    PlayerPrefs.Save(); // дополнительно сохранить PlayerPrefs
                }
            }

            private void OnApplicationQuit()
            {
                if (_systems == null) return;

                // Call save on exit for all systems that implement ISaveOnExit
                foreach (var system in _allSystems)
                {
                    if (system is ISaveOnExit saveSystem)
                    {
                        saveSystem.SaveOnExit();
                    }
                }

                PlayerPrefs.Save();
            }

            private void OnDestroy()
            {
                if (_systems != null)
                {
                    _systems.Destroy();
                    _systems = null;
                }

                if (_world != null)
                {
                    _world.Destroy();
                    _world = null;
                }
            }

            #endregion

            #region ECS Setup

            private void AddSystems()
            {
                _allSystems.Clear();

                // Business systems
                _allSystems.Add(new BusinessInitSystem(
                    _businessConfigs,
                    _world,
                    _saveLoadService,
                    _bindButtonService,
                    _uiRegistry));

                _allSystems.Add(new BusinessUiSystem(_uiRegistry));
                _allSystems.Add(new BusinessProgressSystem());
                _allSystems.Add(new BusinessPurchaseSystem());

                // Player balance systems
                _allSystems.Add(new PlayerBalanceInitSystem(_uiRegistry, _world, _saveLoadService));
                _allSystems.Add(new PlayerBalanceSystem());
                _allSystems.Add(new PlayerBalanceUiSystem());
                _allSystems.Add(new PlayerBalanceSaveSystem(_saveLoadService));

                // Add all to ECS pipeline
                foreach (var system in _allSystems)
                {
                    _systems.Add(system);
                }
            }

            private void AddOneFrameComponents()
            {
                _systems.OneFrame<PlayerBalanceChanged>();
            }

            #endregion
        }
    }
