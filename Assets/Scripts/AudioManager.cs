using System;
using UnityEngine;

namespace PunchGear
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        private AudioSource _audioSource;

        private float _effectVolume;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance)
            {
                return;
            }
            GameObject gameObject = new GameObject("Audio Manager", typeof(AudioManager));
            _instance = gameObject.GetComponent<AudioManager>();
            DontDestroyOnLoad(gameObject);
        }

        public static AudioManager Instance => _instance;

        public event Action OnLoad;

        public event Action<AudioClip> OnClipStart;

        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }

        public float EffectVolume
        {
            get => _effectVolume;
            set
            {
                if (value < 0 || value > 1f)
                {
                    throw new ArgumentOutOfRangeException("effect volume", value, "effect volume must be between 0 and 1");
                }
                _effectVolume = value;
            }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _audioSource.loop = true;
            _audioSource.volume = 0.5f;
            _effectVolume = 0.5f;
            OnLoad?.Invoke();
        }

        public void PlayLoop(AudioClip clip, bool forceStop = true)
        {
            if (_audioSource.isPlaying && forceStop)
            {
                _audioSource.Stop();
            }
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Play(AudioClip clip, float volume)
        {
            float realScaleVolume = volume / _audioSource.volume;
            _audioSource.PlayOneShot(clip, realScaleVolume);
            OnClipStart?.Invoke(clip);
        }

        public void Play(AudioClip clip)
        {
            Play(clip, _effectVolume);
        }

        public void SetVolume(float volume)
        {
            if (volume < 0f || volume > 1f)
            {
                throw new ArgumentException(nameof(volume), "Volume must be between 0 and 1");
            }
            _audioSource.volume = volume; // 0.0에서 1.0 사이의 값
        }
    }
}
