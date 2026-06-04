using System;
using System.Collections;
using Core.Application.DataStorage;
using Unity.Utils.Time;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class FoodProduction : MonoBehaviour
    {
        public event Action OnFoodProductionStarted;
        public event Action OnFoodProduced;
        
        [Inject] private IDataStorage _dataStorage;

        private int _curFoodCount;
        private float _foodProductionTime;
        private Timer timer = new Timer();
        private float _curProgress = 0;
        
        public int CurrentFoodCount => _curFoodCount;
        public float CurProgress => _curProgress;

        public void StartProduction()
        {
            _foodProductionTime = 1 / _dataStorage.FoodProductionPerSecond;
            OnFoodProductionStarted?.Invoke();
            StartCoroutine(Produce());
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
            _curFoodCount++;
            OnFoodProduced?.Invoke();
            StartCoroutine(Produce());
        }
        
        
        
        
    }
}