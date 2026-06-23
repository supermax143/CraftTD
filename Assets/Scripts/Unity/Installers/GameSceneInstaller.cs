using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Core.Application.DataStorage;
using Unity.Game;
using Unity.Presentation.HUD;
using UnityEngine;
using Zenject;

namespace Unity.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private GameController _gameController;
        [SerializeField]
        private FoodProduction _foodProduction;
        [SerializeField]
        private DropManager _dropManager;
        [SerializeField]
        private ResourceContainer[] _dropTargets;
        
        public override void InstallBindings()
        {
            Container.Bind<IGameController>().FromInstance(_gameController).AsSingle();
            Container.Bind<IFoodProduction>().FromInstance(_foodProduction).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelRewardAggregator>().AsSingle();
            Container.Bind<DropManager>().FromInstance(_dropManager).AsSingle();
            Container.Bind<IEnumerable<IDropTarget>>().FromInstance(_dropTargets).AsSingle();
        }
        
    }
}