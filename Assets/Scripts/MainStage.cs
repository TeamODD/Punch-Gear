using UnityEngine;

namespace PunchGear
{
    public class MainStage : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _bgmClip;

        private void Start()
        {
            AudioManager.Instance.Volume = 0.25f;
            if (_bgmClip != null)
            {
                AudioManager.Instance.PlayLoop(_bgmClip);
            }
        }
    }
}
