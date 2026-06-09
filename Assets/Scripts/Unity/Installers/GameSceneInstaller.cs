using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Core.Application.DataStorage;
using Unity.Game;
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
        
        [Inject] ChronologyInfo _chronologyInfo;
        [Inject] IDataStorage _dataStorage;
        
        public override void InstallBindings()
        {
            Container.Bind<IGameController>().FromInstance(_gameController).AsSingle();
            Container.Bind<IFoodProduction>().FromInstance(_foodProduction).AsSingle();
        }
        
    }
}