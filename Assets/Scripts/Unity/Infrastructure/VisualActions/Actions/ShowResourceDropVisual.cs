using Core.Application.Interfaces;
using Unity.Game;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Presentation.HUD;
using Zenject;

namespace Unity.Infrastructure.VisualActions.Actions
{
    public class ShowResourceDropVisual : VisualActionBase<ShowResourceDropActionData>, IBlocker
    {
        [Inject] private GameHUD _hud;
        [Inject] private DropManager _dropManager;

        public override void Execute()
        {
            _hud.BlockResourceUpdate(this, Data.Resource.Type);

            if (Data.IsUiDrop)
            {
                var pos = UnityEngine.Camera.main.ScreenToWorldPoint(Data.StartPosition);
                _dropManager.ShowUiDrop(Data.Resource, Data.IsTemp, pos, OnDropComplete);
            }
            else
            {
                _dropManager.ShowSceneDrop(Data.Resource, Data.IsTemp, Data.StartPosition, default, OnDropComplete);
            }
        }

        private void OnDropComplete()
        {
            _hud.UnBlockResourceUpdate(this, Data.Resource.Type);
            Complete();
        }
    }
}
