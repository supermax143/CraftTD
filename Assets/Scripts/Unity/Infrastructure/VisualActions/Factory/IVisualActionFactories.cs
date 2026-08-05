using System;
using System.Collections.Generic;
using Unity.Infrastructure.VisualActions.Actions;
using Unity.Infrastructure.VisualActions.ActionsData;
using UnityEngine;

namespace Unity.Infrastructure.VisualActions.Factory
{
   public interface IVisualActionFactories
   {
      public Dictionary<Type, Func<IActionData, VisualActionBase>> GetFactories(GameObject gameObject);
   }
}