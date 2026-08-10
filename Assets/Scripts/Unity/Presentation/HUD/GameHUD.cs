using System;
using Unity.Presentation.Components;
using UnityEngine;

namespace Unity.Presentation.HUD
{
    public class GameHUD : MonoBehaviour
    {
        public enum State
        {
            Idle, 
            Battle
        }

        private GameHUDAnimatorController _animatorController;
        
        private State _state = State.Idle;

        private void OnValidate()
        {
            _animatorController = GetComponentInChildren<GameHUDAnimatorController>();
        }


        public void ShowBattleView()
        {
            _state = State.Battle;
            _animatorController.SetIsBattleState(true);
        }
        
        public void ShowIdleView()
        {
            _state = State.Idle;
            _animatorController.SetIsBattleState(false);
        }
        
    }
}