using System;
using UnityEngine;

namespace Environments.Common.Scripts
{
    public interface ITouchTarget
    {
        public event Action<ITouchTarget, Vector2> OnClick;

        public void HandleClick(Vector2 touchPosition);

    }
}