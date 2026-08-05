using System;
using Unity.Infrastructure.VisualActions.ActionsData;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.Actions
{
    public abstract class VisualActionBase : MonoBehaviour
    {
        public event Action<VisualActionBase> OnComplete;
        
        public abstract void Initialize(IActionData data);
        
        public abstract void Execute();

        protected void Complete()
        {
            OnComplete?.Invoke(this);
            Destroy(this);
        }

    }
    
    public abstract class VisualActionBase<TData> : VisualActionBase where TData : IActionData
    {
        protected TData Data { get; private set; }

        public sealed override void Initialize(IActionData data)
        {
            if (data is not TData castedData)
                throw new InvalidOperationException(
                    $"Invalid data type. Got {data.GetType().Name}, expected {typeof(TData).Name}");
            
            Data = castedData;
            OnInitialized();
        }

        protected virtual void OnInitialized() {}
    }
    
}