using Core.Application.Models;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.ActionsData
{
    public class ShowResourceDropActionData : IActionData
    {
        public Vector2 StartPosition { get; init; }
        public Resource Resource { get; init; }
        public bool IsTemp { get; init; }
        public bool IsUiDrop { get; init; }

        public ShowResourceDropActionData()
        {
            
        }
        public ShowResourceDropActionData(Vector2 startPosition, Resource resource, bool isTemp, bool isUiDrop)
        {
            StartPosition = startPosition;
            Resource = resource;
            IsTemp = isTemp;
            IsUiDrop = isUiDrop;
        }
    }
}
