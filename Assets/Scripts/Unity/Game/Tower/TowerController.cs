using System;
using Unity.Settings;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(TowerView))]
    public class TowerController : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private TowerView _view;
        [SerializeField, HideInInspector]
        private AttackTarget _attackTarget;
        [SerializeField, HideInInspector]
        private HealthComponent _health;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public AttackTarget AttackTarget => _attackTarget;


        private void OnValidate()
        {
            _view = GetComponentInChildren<TowerView>();
            _health = GetComponentInChildren<HealthComponent>();
            _attackTarget = GetComponentInChildren<AttackTarget>();
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
            _attackTarget.SetFaction(_faction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.Log($"{this.GetType().Name}: Can't find faction color {faction}");
                color = Color.purple;
            }
            _view.SetColor(color);
        }
        public void Initialize()
        {
            _health.Initialize();
            _health.OnDeath += OnDeath;
            _attackTarget.Initialize(_health);
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}