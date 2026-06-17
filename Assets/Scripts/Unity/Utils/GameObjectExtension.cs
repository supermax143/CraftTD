using UnityEngine;

/// <summary>
/// Методы расширения для GameObject
/// </summary>
public static class GameObjectExtension
{
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetLayerRecursively(layer);
        }
    }
}
