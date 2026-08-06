using Core.Application.Models;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.ActionsData
{
    public class ShowResultActionData : IActionData
    {

        public float Delay { get; }
        public bool PlayerWin { get; }
        public bool ResetTower { get; }
        

        public ShowResultActionData(bool playerWin, bool resetTower, float delay)
        {
            PlayerWin = playerWin;
            ResetTower = resetTower;
            Delay = delay;
        }

    }
}
