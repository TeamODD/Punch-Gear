using System;

using UnityEngine;

namespace PunchGear
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        private float _effectVolume;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (Instance)
            {
                return;
            }
            GameObject gameObject = new GameObject("Audio Manager", typeof(AudioManager));
            Instance = gameObject.GetComponent<AudioManager>();
            DontDestroyOnLoad(gameObject);
        }

        public static AudioManager Instance { get; private set; }

        public event Action OnLoad;

        public event Action<AudioClip> OnClipStart;

        public float Volume
        {
            get
            {
                return _audioSource.volume;
            }
            set
            {
                _audioSource.volume = value;
            }
        }

        public float EffectVolume
        {
            get
            {
                return _effectVolume;
            }
            set
            {
                if (value < 0 || value > 1f)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "Effect volume must be between 0 and 1");
                }
                _effectVolume = value;
            }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (!_audioSource)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void OnEnable()
        {
            _audioSource.loop = true;
        }

        private void Start()
        {
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
    }
}
