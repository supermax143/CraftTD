using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние завершения заклинания
    /// </summary>
    public class CompleteState : SpellState
    {
        public override void Enter()
        {
            _spell.Dispose();
        }
        

       
    }
}
