namespace Unity.Infrastructure.GameEvents
{
    public class SceneEvent : GameEvent
    {
        public enum Type
        {
            None,
            StartLoad,
            Opened,
        }

        public Type _type = Type.None;
        
        public SceneEvent( Type type, string sceneName)
        {
            _type = type;
            Name = "scene_event";
            _params[nameof(type)] = type.ToString();
            _params[nameof(sceneName)] = sceneName;
        }
    }
}