using System;
using System.Collections.Generic;
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
        
        public override void InstallBindings()
        {
            Container.BindInstance(_gameController).AsSingle();
            Container.BindInstance(_foodProduction).AsSingle();
        }
    }
}