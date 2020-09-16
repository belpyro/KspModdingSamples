using UnityEngine;

namespace KspModdingSample
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class KspMusicAddon : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Ksp debug started");
        }
    }
}