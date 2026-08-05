using Unity.Infrastructure.VisualActions.ActionsData;

namespace Unity.Infrastructure.VisualActions.Actions
{
    public class TeleportLandObjectVisual : VisualActionBase<TeleportLandObjectActionData>
    {
        /*[Inject] private ILandObjectsController _landObjectsController;
        [Inject] private ICharactersController _charactersController;
        [Inject] private ICellController _cellController;*/
        
        public override void Execute()
        {

            /*if (_charactersController.TryGetCharacter(Data.Id, out var character))
            {
                character.transform.position = 
                    _cellController.TilemapAPI.CellToWorld(Data.Cell, true);
                Complete();
                return;
            }

            _landObjectsController.TryGetObject(Data.Id, out var landObject);
            var pos = _cellController.TilemapAPI.CellToWorld(Data.Cell, false);
            landObject.transform.position = pos;*/
            Complete();
        }
        
        
    }
}