using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Unity.Infrastructure.GameEvents;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units
{
    
    [UsedImplicitly]
    [UnitCategory("Custom")]
    [UnitTitle("WaitGameEvent")]
    public class WaitGameEventUnit : AsyncCustomUnit
    {
        private const string NAME = "name";
        private const string PARAMS = "params";

        private bool _isDone;
        
        [Inject] private GameEventsBus _gameEventsBus;
        
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal() {
            yield return ValueInput<string>(NAME, "");
            yield return ValueInput<string>(PARAMS, "");
        }
        

        protected override IEnumerator OnExecute(Flow flow)
        {
            var name = GetValue<string>(flow, NAME);
            var @params = GetValue<string>(flow, PARAMS);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(@params);
            var checkingEvent = new GameEvent(name, dict);
            _gameEventsBus.OnGameEvent += HandleGameEvent;
            yield return new WaitWhile(() => !_isDone);
            _gameEventsBus.OnGameEvent -= HandleGameEvent;
            
            void HandleGameEvent(GameEvent @event)
            {
                if (@event.Equal(checkingEvent))
                {
                    _isDone = true;
                }
            }
        }

       
    }
}