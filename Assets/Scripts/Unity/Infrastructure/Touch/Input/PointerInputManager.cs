using System;
using System.Collections.Generic;
using InputSamples.Controls;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Unity.Infrastructure.Touch.Input
{
    /// <summary>
    /// Input manager that interprets pen, mouse and touch input for mostly drag related controls.
    /// Passes pressure, tilt, twist and touch radius through to drawing components for processing.
    /// </summary>
    /// <remarks>
    /// Couple notes about the control setup:
    ///
    /// - Touch is split off from mouse and pen instead of just using `&lt;Pointer&gt;/position` etc.
    ///   in order to support multi-touch. If we just bind to <see cref="Touchscreen.position"/> and
    ///   such, we will correctly receive the primary touch but the primary touch only. So we put
    ///   bindings for pen and mouse separate to those from touch.
    /// - Mouse and pen are put into one composite. The expectation here is that they are not used
    ///   independently from another and thus don't need to be represented as separate pointer sources.
    ///   However, we could just as well have one <see cref="PointerInputComposite"/> for mice and
    ///   one for pens.
    /// - <see cref="InputAction.passThrough"/> is enabled on <see cref="PointerControls.PointerActions.point"/>.
    ///   The reason is that we want to source arbitrary many pointer inputs through one single actions.
    ///   Without pass-through, the default conflict resolution on actions would kick in and let only
    ///   one of the composite bindings through at a time.
    /// </remarks>
    public class PointerInputManager : MonoBehaviour
    {
        private static class Helpers
        {
            public const int LeftMouseInputId = PointerInputModule.kMouseLeftId;
            public const int PenInputId = int.MinValue;
        }
        
        /// <summary>
        /// Event fired when the user presses on the screen.
        /// </summary>
        public event Action<PointerInput, double> Pressed;

        /// <summary>
        /// Event fired as the user drags along the screen.
        /// </summary>
        public event Action<PointerInput, double> Dragged;

        /// <summary>
        /// Event fired when the user releases a press.
        /// </summary>
        public event Action<PointerInput, double> Released;

        private bool _dragging;
        private PointerControls _controls;

        // These are useful for debugging, especially when touch simulation is on.
        [SerializeField] private bool _useMouse = true;
        [SerializeField] private bool _usePen = true;
        [SerializeField] private bool _useTouch = true;

        protected virtual void Awake()
        {
            _controls = new PointerControls();
            var t = new TouchControl();
            _controls.pointer.Touch0.performed += OnAction;
            _controls.pointer.Touch0.canceled += OnAction;
            
            _controls.pointer.Touch1.performed += OnAction;
            _controls.pointer.Touch1.canceled += OnAction;
            
            SyncBindingMask();
        }

        protected virtual void OnEnable() => _controls?.Enable();

        protected virtual void OnDisable() => _controls?.Disable();

        private void OnAction(InputAction.CallbackContext context)
        {
            var control = context.control;
            var device = control.device;

            var isMouseInput = device is Mouse;
            var isPenInput = !isMouseInput && device is Pen;

            // Read our current pointer values.
            var drag = context.ReadValue<PointerInput>();
            
            if (isMouseInput)
                drag.InputId = Helpers.LeftMouseInputId;
            else if (isPenInput)
                drag.InputId = Helpers.PenInputId;

            var dragging = _idToState.TryGetValue(drag.InputId, out var res) && res;
                
            switch (drag.Contact)
            {
                case true when !dragging:
                    Pressed?.Invoke(drag, context.time);
                    _idToState[drag.InputId] = true;
                    break;
                case true:
                    Dragged?.Invoke(drag, context.time);
                    break;
                default:
                    Released?.Invoke(drag, context.time);
                    _idToState.Remove(drag.InputId);
                    break;
            }
        }

        private readonly Dictionary<int, bool> _idToState = new();

        private void SyncBindingMask()
        {
            if (_controls == null)
                return;

            if (_useMouse && _usePen && _useTouch)
            {
                _controls.bindingMask = null;
                return;
            }

            _controls.bindingMask = InputBinding.MaskByGroups(new[]
            {
                _useMouse ? "Mouse" : null,
                _usePen ? "Pen" : null,
                _useTouch ? "Touch" : null
            });
        }

        private void OnValidate()
        {
            SyncBindingMask();
        }
    }
}
