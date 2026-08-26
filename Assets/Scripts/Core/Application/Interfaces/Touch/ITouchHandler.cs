using System.Collections.Generic;
using Unity.Infrastructure.Touch;

namespace Environments.Common.Scripts
{
    public interface ITouchHandler
    {
        //нужен для сортировки
        TouchHandlerType Type { get; }
        
        bool OnTouchBegin(IReadOnlyCollection<TouchData> touch);
        void OnTouchEnd(IReadOnlyCollection<TouchData> touch);
        void OnTouchMove(IReadOnlyCollection<TouchData> touch);
        bool TryConsumeClick(TouchData touch);
    }
}