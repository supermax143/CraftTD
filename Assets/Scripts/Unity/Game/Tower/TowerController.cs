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
        [FormerlySerializedAs("_attackTarget")] [SerializeField]
        private AttackTarget attackAttackTarget;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public AttackTarget AttackTarget => attackAttackTarget;


        private void OnValidate()
        {
            _view = GetComponent<TowerView>();
            
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
            attackAttackTarget.SetFaction(_faction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.Log($"{this.GetType().Name}: Can't find faction color {faction}");
                color = Color.purple;
            }
            _view.SetColor(color);
        }
    }
}