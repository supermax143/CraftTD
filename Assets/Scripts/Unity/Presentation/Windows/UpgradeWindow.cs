using System;
using System.Collections.Generic;
using Core.Application.Models;
using Unity.Game;
using Unity.Presentation.Components;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows
{
    public class UpgradeWindow : WindowBase
    {
        [SerializeField]
        private List<UnitOpenItem> _unitsItems;
        
        [Inject] IMainModel _model;
        
        public void Initialize()
        {
            UpdateUnits();
        }

        private void UpdateUnits()
        {
            foreach (var unit in _unitsItems)
            {
                throw new NotImplementedException();
            }
        }
    }
}