using System;
using System.Collections;
using Core.Application.DataStorage;
using Core.Application.Models;
using Unity.Utils.Time;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class FoodProduction : MonoBehaviour, IFoodProduction
    {
        public event Action OnFoodProductionStarted;
        public event Action OnFoodChanged;
        
        /*[Inject] private IDataStorage _dataStorage;
        [Inject] private GameStats _gameStats;*/
        [Inject] private IMainModel _mainModel;
        
        private EpochModel Epoch => _mainModel.Epoch;
        
        private int _foodCount;
        private float _foodProductionTime;
        private Timer timer = new Timer();
        private float _curProgress = 0;
        private bool _started = false;
        
        public int FoodCount => _foodCount;
        public float CurProgress => _curProgress;

        public bool Started => _started;



        public void StartProduction()
        {
            _foodProductionTime = 1 / Epoch.FoodProductionSpeed;
            OnFoodProductionStarted?.Invoke();
            StartCoroutine(Produce());
            _started = true;
        }

        public void StopProduction()
        {
            StopAllCoroutines();
            _started = false;
        }
        
        private IEnumerator Produce()
        {
            _curProgress = 0;
            timer.Start(_foodProductionTime);
            while (!timer.IsComplete)
            {
                _curProgress = timer.Progress;
                yield return  null;
            }

            _curProgress = 1;
            _foodCount++;
            OnFoodChanged?.Invoke();
            StartCoroutine(Produce());
        }


        public void WithdrawFood(int count)
        {
            if (count > _foodCount)
            {
                return;
            }
            _foodCount -= count;
            OnFoodChanged?.Invoke();
        }
    }
}