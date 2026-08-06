using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Components
{
    public class MultiSpritesButton : Button
    {
        [SerializeField, HideInInspector]
        private Image[] _images;

        protected override void OnValidate()
        {
            base.OnValidate();
            _images = GetComponentsInChildren<Image>();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            foreach (var image in _images)
            {
                var color = state == SelectionState.Disabled ? colors.disabledColor : colors.normalColor;
                image.color = color;
            }
            base.DoStateTransition(state, instant);
        }
    }
}