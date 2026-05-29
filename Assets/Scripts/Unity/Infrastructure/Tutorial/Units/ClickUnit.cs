using System.Collections;
using System.Collections.Generic;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace Unity.Infrastructure.Tutorial.Units
{
    [UnitCategory("Custom")]
    [UnitTitle("Click")]
    public class ClickUnit : AsyncCustomUnit
    {
        protected const string TARGET = "target";
        protected const string SHOW_TINT = "showTint";
        protected const string OVERRIDE_CLICK = "overrideClick";

        [Inject] private ITutorialOverlayController _tutorialOverlay;
        
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield return ValueInput<TutorialTag>(TARGET);
            yield return ValueInput(SHOW_TINT, true);
            yield return ValueInput(OVERRIDE_CLICK, false);
        }

        protected override IEnumerator OnExecute(Flow flow)
        {
            var clickComplete = false;
            
            var tag = GetValue<TutorialTag>(flow, TARGET);
            var showTint = GetValue<bool>(flow, SHOW_TINT);
            var overlayAlpha = showTint ? -1 : 0;
            _tutorialOverlay.ShowOverlay(overlayAlpha);


            if (!TryGetButton(tag, out var button))
            {
                Debug.Log("Button not Found");
            }

            var canvas = button.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
            var raycaster = button.AddComponent<GraphicRaycaster>();
            var originEvent = button.onClick;

            button.onClick = new ButtonClickedEvent();
            button.onClick.AddListener(OnClick);

            while (!clickComplete)
            {
                yield return null;
            }

            _tutorialOverlay.HideOverlay();
            Object.Destroy(raycaster);
            Object.Destroy(canvas);
            button.onClick = originEvent;
            
            if (!GetValue<bool>(flow, OVERRIDE_CLICK))
            {
                button.onClick.Invoke();
            }
            
            void OnClick()
            {
                clickComplete = true;
            }
        }

        private bool TryGetButton(TutorialTag tag, out Button button)
        {
            button = tag.GetComponentInChildren<Button>();
            
            return button != null;
        }
    }
}