using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Core.Application.DataStorage;
using Exploration.Scripts.Controllers.ModelRender;
using Unity.Game;
using Unity.Infrastructure.Camera;
using Unity.Infrastructure.Effects;
using Unity.Infrastructure.VisualActions;
using Unity.Infrastructure.VisualActions.Factory;
using Unity.Infrastructure.Views;
using Unity.Presentation;
using Unity.Presentation.HUD;
using UnityEngine;
using Zenject;

namespace Unity.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private ResourceContainer[] _battleDropTargets;
        [SerializeField]
        private GameHUD _hud;
        
        public override void InstallBindings()
        {
            Container.Bind<IEnumerable<IDropTarget>>().FromInstance(_battleDropTargets).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelRewardAggregator>().AsSingle();
            Container.Bind<GameHUD>().FromInstance(_hud).AsSingle();
            
            BindController<GameController>();
            BindController<FoodProduction>();
            BindController<DropManager>();
            BindController<VisualEffectSpawnManager>();
            BindController<CameraController>();
            BindController<VisualActionFactories>();
            BindController<VisualActionsController>();
            BindController<ViewsController>();
        }
        
        private void BindController<TController>() where TController: Component
        {
            var instance = gameObject.GetComponentInChildren<TController>();
            Container.BindInterfacesAndSelfTo<TController>().FromInstance(instance);
        }
    }
}