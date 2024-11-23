using System;
using UnityEngine;

namespace PunchGear
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        private static AudioManager _instance; //넌 뭐니

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            _instance = FindFirstObjectByType<AudioManager>();
            if(!_instance) // 오디오 매니저가 없으면 실행
            {
                DontDestroyOnLoad(_instance);
            }
        }


        //private void Start()
        //{
        //    audioSource = GetComponent<AudioSource>();

        //    audioSource.clip = this.clip;
        //    audioSource.loop = true; // 반복 재생 설정
        //    audioSource.volume = 0.5f;
        //    audioSource.Play(); // 지속적으로 재생

        //    AudioPause(false);
        //    Time.timeScale = 1f;
        //}

        //public void AudioPause(bool isPaused)
        //{
        //    audioSource.pitch = isPaused ? 0f : 1f;
        //}
        //public void SetVolume(float volume)
        //{
        //    audioSource.volume = volume; // 0.0에서 1.0 사이의 값
        //}
    }
}
