using UnityEngine;
using UnityEngine.SceneManagement;
using GamePush;

namespace GamePush.Initialization
{
    public class Init : MonoBehaviour
    {
        private async void Start()
        {
#if UNITY_WEBGL
            await GP_Init.Ready;
#endif
            SceneManager.LoadScene(1);
        }
    }
}
