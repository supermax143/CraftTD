using System;
using Unity.Settings;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(TowerView))]
    public class TowerController : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private TowerView _view;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;

        
        private void OnValidate()
        {
            _view = GetComponent<TowerView>();
            
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.Log($"{this.GetType().Name}: Can't find faction color {faction}");
                color = Color.purple;
            }
            _view.SetColor(color);
        }
    }
}