using PunchGear.Attributes;

using UnityEngine;

namespace PunchGear
{
    [DisallowMultipleComponent]
    public class MainStage : MonoBehaviour
    {
        [SerializeField]
        private AudioClip bgmClip;

        [ReadOnlyField]
        [SerializeField]
        private TimerHandler timerHandler;

        private void Awake()
        {
            timerHandler = FindFirstObjectByType<TimerHandler>();
            if (timerHandler == null)
            {
                throw new UnassignedReferenceException("Could not find TimerHandler in the scene");
            }
        }

        private void Start()
        {
            timerHandler.StartTime = Time.time;
            AudioManager.Instance.Volume = 0.25f;
            if (bgmClip != null)
            {
                AudioManager.Instance.PlayLoop(bgmClip);
            }
            timerHandler.Enable();
        }

        private void OnDisable()
        {
            timerHandler.Disable();
        }
    }
}
