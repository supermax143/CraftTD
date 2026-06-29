namespace Environments.Land.Scripts.Runtime.GUI
{
    public interface IRenderModelsSpawnHandler
    {
        public void OnModelsStartSpawn();
        public void OnModelsEndSpawn();
        
        public void OnModelsStartDestroy();
        public void OnModelsEndDestroy();
        
    }
}