using Core.Application.Models;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.ActionsData
{
    public class ChangeEpochActionData : IActionData
    {
        
        public bool ShowInversed { get; }
        public EpochModel EnemyEpoch { get; }
        
        public ChangeEpochActionData(bool showInversed, EpochModel enemyEpoch)
        {
            ShowInversed = showInversed;
            EnemyEpoch = enemyEpoch;
        }

    }
}