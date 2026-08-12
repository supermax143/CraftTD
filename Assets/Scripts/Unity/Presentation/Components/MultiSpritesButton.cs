using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Components
{
    public class MultiSpritesButton : Button
    {
        [SerializeField, HideInInspector]
        private Image[] _images;
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _images = GetComponentsInChildren<Image>();
        }
#endif
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