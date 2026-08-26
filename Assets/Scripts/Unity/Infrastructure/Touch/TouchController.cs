using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Interfaces.Windows;
using Environments.Common.Scripts;
using ModestTree;
using Unity.Infrastructure.Touch.Input;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Touch
{
    /// <summary>
    /// Контроллер тачей, ипользуется для определения тачей на карте. в него передаются
    /// хэндлеры и ждут соответствующего тач-события
    /// </summary>
    public class TouchController : MonoBehaviour, ITouchController
    {
        
        [SerializeField, HideInInspector]
        private PointerInputManager _pointerInputManager;

        [Inject] private IWindowsController _windowsController;
        
        private readonly List<ITouchHandler> _activeHandlers = new();
        private readonly List<ITouchHandler> _handlers = new();
        private readonly Dictionary<int, TouchData> _idToTouchData = new();

        private bool _moved = false;
        
        private readonly HashSet<ITouchBlocker> _tokens = new();
        
        public bool Blocked => _tokens.Count > 0;
        public event Action OnTouchWithNoHandlers = delegate {  };

        public void Block(ITouchBlocker touchBlocker)
        {
            if (!_tokens.Add(touchBlocker))
                return;

            RemoveListeners();
        }

        public void Unblock(ITouchBlocker touchBlocker)
        {
            if (!_tokens.Remove(touchBlocker))
            {
                return;
            }

            if (_tokens.Count > 0)
            {
                return;
            }
            
            AddListeners();
        }

        private void OnValidate()
        {
            _pointerInputManager = GetComponent<PointerInputManager>();
        }

        private void Start()
        {
            _windowsController.SetTouchController(this);
            _activeHandlers.Clear();
            AddListeners();
        }

        private void AddListeners()
        {
            _pointerInputManager.Pressed += PressHandler;
            _pointerInputManager.Released += ReleasedHandler;
            _pointerInputManager.Dragged += DraggedHandler;
        }

        private void RemoveListeners()
        {
            _pointerInputManager.Pressed -= PressHandler;
            _pointerInputManager.Released -= ReleasedHandler;
            _pointerInputManager.Dragged -= DraggedHandler;
        }
        
        private void PressHandler(PointerInput input, double time)
        {
            if (Blocked)
            {
                return;
            }
            if (_idToTouchData.ContainsKey(input.InputId))
            {
                return;
            }
            
            _activeHandlers.Clear();
            
            AddTouchData(input);
            foreach (var handler in _handlers)
            {
                _activeHandlers.Add(handler);
                if (handler.OnTouchBegin(_idToTouchData.Values))
                {
                    return;
                }
            }
            
            OnTouchWithNoHandlers();
        }
        
        private void DraggedHandler(PointerInput input, double time)
        {
            if (Blocked)
            {
                return;
            }
            if (!_idToTouchData.TryGetValue(input.InputId, out var touchData))
            {
                return;
            }

            if (input.Position == touchData.MousePosition)
            {
                return;
            }
            
            UpdateTouchData(input);
            _moved = true;
            foreach (var handler in _activeHandlers)
            {
                handler.OnTouchMove(_idToTouchData.Values);
            }
        }

        private void ReleasedHandler(PointerInput input, double time)
        {
            if (Blocked)
            {
                return;
            }
            if (!_idToTouchData.TryGetValue(input.InputId, out var touchData))
            {
                return;
            }
            
            foreach (var handler in _activeHandlers)
            {
                handler.OnTouchEnd(_idToTouchData.Values);
                if (_moved || _idToTouchData.Count > 1)
                    continue;
                
                if (handler.TryConsumeClick(touchData))
                    break;
            }
            
            _idToTouchData.Remove(input.InputId);
            
            if (_idToTouchData.IsEmpty())
            {
                _moved = false;
            }
            
            _activeHandlers.Clear();
        }


        private void AddTouchData(PointerInput input)
        {
            _idToTouchData[input.InputId] = new TouchData()
            {
                TouchId = input.InputId,
                MousePosition = input.Position
            };
        }

        private void UpdateTouchData(PointerInput input)
        {
            if (!_idToTouchData.TryGetValue(input.InputId, out var touchData))
            {
                return;
            }
            
            _idToTouchData[input.InputId] = new TouchData()
            {
                TouchId = input.InputId,
                MousePosition = input.Position,
                MoveDistance = touchData.MousePosition - input.Position
            };
            
        }
        
        public void AddHandler(ITouchHandler handler)
        {
            if (_handlers.Contains(handler))
            {
                return; 
            }
            
            _handlers.Add(handler);
            _handlers.Sort(new TouchHandlerComparer());
        }

        public void RemoveHandler(ITouchHandler handler)
        {
            if (!_handlers.Contains(handler))
            {
               return; 
            }
            
            _handlers.Remove(handler);
            _handlers.Sort(new TouchHandlerComparer());
        }
    }
}