using System;
using Unity.Settings;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(UnitView))]
    public class UnitController : MonoBehaviour
    {
        [SerializeField] 
        private float _health;
        [SerializeField] 
        private UnitAttack _attack;
        [SerializeField, HideInInspector] 
        private UnitView _view;
        [SerializeField]
        private AttackTarget _attackTarget;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;
        private Faction _opponentFaction;
        
        public float Health => _health;
        public UnitAttack Attack => _attack;
        public Faction OpponentFaction => _opponentFaction;

        private void OnValidate()
        {
            _view = GetComponent<UnitView>();
        }


        public void SetFaction(Faction faction, Faction enemyFaction)
        {
            _faction = faction;
            _opponentFaction = enemyFaction;
            _attackTarget.SetFaction(_faction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.Log($"{this.GetType().Name}: Can't find faction color {faction}");
                color = Color.purple;
            }
            _view.SetColor(color);
        }
    }
}