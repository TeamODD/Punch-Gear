using PunchGear.Entity;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace PunchGear
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip clip;

        private void Awake()
        {
            var obj = UnityEngine.Object.FindObjectsByType<AudioManager>(FindObjectsSortMode.None);
            if (obj.Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _audioSource.clip = this.clip;
            _audioSource.loop = true;
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }

        public void SetVolume(float volume)
        {
            if (volume >= 0f && volume <= 1f)
            {
                _audioSource.volume = volume; // 0.0에서 1.0 사이의 값
            }
            else
            {
                throw new ArgumentException(nameof(volume), "Volume must be between 0 and 1");
            }
        }
    }
}
