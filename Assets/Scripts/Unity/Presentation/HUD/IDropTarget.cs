using Core.Application.Models;
using UnityEngine;

namespace Unity.Presentation.HUD
{
    public interface IDropTarget
    {
        RectTransform GetTargetRect();
        void AddResource(Resource resource);
    }
}