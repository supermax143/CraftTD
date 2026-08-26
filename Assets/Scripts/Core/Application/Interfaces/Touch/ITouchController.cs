using System;
using UnityEngine;

namespace Environments.Common.Scripts
{
    public struct TouchData
    {
        public int TouchId;
        public Vector2 MousePosition;
        public Vector2 MoveDistance;
        
        public override string ToString() => $"id:{TouchId}; pos:{MousePosition}; delta:{MoveDistance}";
    }
    
    public interface ITouchController
    {
        event Action OnTouchWithNoHandlers;
        
        void AddHandler(ITouchHandler handler);
        void RemoveHandler(ITouchHandler handler);
        bool Blocked { get; }
        void Block(ITouchBlocker touchBlocker);
        void Unblock(ITouchBlocker touchBlocker);
    }
}