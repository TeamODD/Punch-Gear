using UnityEngine;

namespace PunchGear
{
    public class Main : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Debug.Log("Main class loaded on the scene");
        }
    }
}
