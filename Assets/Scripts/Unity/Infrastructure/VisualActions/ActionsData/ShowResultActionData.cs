using Core.Application.Models;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.ActionsData
{
    public class ShowResultActionData : IActionData
    {

        public float Delay { get; }
        public bool PlayerWin { get; }
        

        public ShowResultActionData(bool playerWin, float delay)
        {
            PlayerWin = playerWin;
            Delay = delay;
        }

    }
}
