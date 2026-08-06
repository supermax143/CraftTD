using Core.Application.Models;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.ActionsData
{
    public class ChangeEpochActionData : IActionData
    {
        
        public bool ShowInversed { get; }
        public bool ChangePlayerTower { get;}
        
        public ChangeEpochActionData(bool showInversed, bool changePlayerTower)
        {
            ShowInversed = showInversed;
            ChangePlayerTower = changePlayerTower;
        }

    }
}