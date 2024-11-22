using UnityEngine;

namespace PunchGear
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance; //넌 뭐니
        public AudioSource audioSource;
        public AudioClip clip;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("AudioManager instance is null. Make sure it is in the scene.");
                }
                return instance;
            }
        }
        private void Awake()
        {
            // Singleton pattern
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // 이 오브젝트를 씬 전환 시 파괴되지 않도록 설정
            }
            else
            {
                Destroy(gameObject); // 이미 존재하는 인스턴스가 있을 경우 현재 오브젝트를 파괴
            }
        }
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            audioSource.clip = this.clip;
            audioSource.loop = true; // 반복 재생 설정
            audioSource.volume = 0.5f;
            audioSource.Play(); // 지속적으로 재생

            AudioPause(false);
            Time.timeScale = 1f;
        }

        public void AudioPause(bool isPaused)
        {
            audioSource.pitch = isPaused ? 0f : 1f;
        }
        public void SetVolume(float volume)
        {
            audioSource.volume = volume; // 0.0에서 1.0 사이의 값
        }
    }
}
