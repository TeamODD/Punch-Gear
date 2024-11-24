using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerAssembleController : MonoBehaviour
    {
        private static AudioClip DisassembleAudioClip;
        private static AudioClip DisassembleMissAudioClip;
        private static AudioClip AssembleAudioClip;
        private static AudioClip AssembleMissAudioClip;

        private Player _player;
        private Dictionary<Projectile, IMouseInputAction> _mouseInputActionLookup;

        private Animator _animator;

        [field: SerializeField]
        public float AssembleCooldown { get; private set; }

        private bool _isAssembleFrozen;
        private bool _isDisassembleFrozen;

        private AssemblyPoint[] _assemblyPoints;


#if UNITY_EDITOR
        private GUIStyle _style;
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void LoadAudioClips()
        {
            DisassembleAudioClip = Resources.Load<AudioClip>("Sound/분해");
            AssembleAudioClip = Resources.Load<AudioClip>("Sound/조립");
            DisassembleMissAudioClip = Resources.Load<AudioClip>("Sound/분해 실수");
            AssembleMissAudioClip = Resources.Load<AudioClip>("Sound/조립 실수");
            if (DisassembleAudioClip == null)
            {
                throw new NullReferenceException("Cannot find Audio clip in the path");
            }
            if (AssembleAudioClip == null)
            {
                throw new NullReferenceException("Cannot find Audio clip in the path");
            }
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _animator = GetComponent<Animator>();
            _assemblyPoints = FindObjectsByType<AssemblyPoint>(FindObjectsSortMode.None);
            _mouseInputActionLookup = new Dictionary<Projectile, IMouseInputAction>();
            _isAssembleFrozen = false;
            _isDisassembleFrozen = false;
#if UNITY_EDITOR
            _style = new GUIStyle();
            _style.fontSize = (int)(40.0f * (Screen.width / 1920f));
#endif
            GloballyPlayerInputHandler.Instance.AddAction(new AnimationTransitionAction(this, _player));

            foreach (AssemblyPoint assemblyPoint in _assemblyPoints)
            {
                GloballyPlayerInputHandler.Instance.AddAction(new AudioEffectAction(assemblyPoint, this, _player));
            }
        }

        private void Start()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            launcher.OnProjectileCreated.AddListener(projectile =>
            {
                IMouseInputAction action = new MouseInputAction(projectile, _player, this);
                _mouseInputActionLookup[projectile] = action;
                GloballyPlayerInputHandler.Instance.AddAction(action);
            });
            launcher.OnProjectileDestroyed.AddListener(projectile =>
            {
                IMouseInputAction action = _mouseInputActionLookup[projectile];
                GloballyPlayerInputHandler.Instance.RemoveAction(action);
                _mouseInputActionLookup.Remove(projectile);
            });
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.Label(new Rect(50, 50, 200, 100), $"isAssembleFrozen: {_isAssembleFrozen}", _style);
            GUI.Label(new Rect(50, 150, 200, 100), $"isDisassembleFrozen: {_isDisassembleFrozen}", _style);
        }
#endif

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void FreezeMouse(MouseAssembleAction assembleAction)
        {
            StartCoroutine(FreezeMouseCoroutine(assembleAction));
        }

        private IEnumerator FreezeMouseCoroutine(MouseAssembleAction assembleAction)
        {
            yield return new WaitForFixedUpdate();
            if (assembleAction == MouseAssembleAction.Assemble)
            {
                _isAssembleFrozen = true;
            }
            else
            {
                _isDisassembleFrozen = true;
            }
            yield return new WaitForSecondsRealtime(AssembleCooldown);
            if (assembleAction == MouseAssembleAction.Assemble)
            {
                _isAssembleFrozen = false;
            }
            else
            {
                _isDisassembleFrozen = false;
            }
            _animator.SetTrigger("Idle");
        }

        private enum MouseAssembleAction
        {
            Assemble,
            Disassemble
        }

        private class AudioEffectAction : IMouseInputAction
        {
            private readonly AssemblyPoint _assemblyPoint;
            private readonly PlayerAssembleController _assembleController;
            private readonly Player _player;

            public AudioEffectAction(AssemblyPoint assemblyPoint, PlayerAssembleController assembleController, Player player)
            {
                _assemblyPoint = assemblyPoint;
                _player = player;
                _assembleController = assembleController;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (_assemblyPoint.Position != _player.Position)
                {
                    return;
                }
                if (inputs == MouseInputs.Left)
                {
                    if (_assembleController._isDisassembleFrozen)
                    {
                        return;
                    }
                    if (_assemblyPoint.EntersProjectile && _assemblyPoint.ProjectileTargets.Any(projectile => !projectile.Disassembled))
                    {
                        AudioManager.Instance.Play(DisassembleAudioClip);
                    }
                    else
                    {
                        AudioManager.Instance.Play(DisassembleMissAudioClip);
                    }
                }
                else if (inputs == MouseInputs.Right)
                {
                    if (_assembleController._isAssembleFrozen)
                    {
                        return;
                    }
                    if (_assemblyPoint.EntersProjectile &&
                        _assemblyPoint.ProjectileTargets.Any(projectile => projectile.Disassembled && !projectile.Assembled))
                    {
                        AudioManager.Instance.Play(AssembleAudioClip);
                    }
                    else
                    {
                        AudioManager.Instance.Play(AssembleMissAudioClip);
                    }
                }
            }
        }

        private class AnimationTransitionAction : IMouseInputAction
        {
            private readonly PlayerAssembleController _assembleController;
            private readonly Player _player;

            public AnimationTransitionAction(PlayerAssembleController assembleController, Player player)
            {
                _assembleController = assembleController;
                _player = player;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (inputs == MouseInputs.Left)
                {
                    if (_assembleController._isDisassembleFrozen)
                    {
                        return;
                    }
                    _assembleController._animator.SetTrigger("Disassemble");
                    _assembleController.FreezeMouse(MouseAssembleAction.Disassemble);
                    _player.DisassemblyCooldownIndicator.StartIndicateCooldown(_assembleController.AssembleCooldown);
                }
                else if (inputs == MouseInputs.Right)
                {
                    if (_assembleController._isAssembleFrozen)
                    {
                        return;
                    }
                    _assembleController._animator.SetTrigger("Assemble");
                    _assembleController.FreezeMouse(MouseAssembleAction.Assemble);
                    _player.AssemblyCooldownIndicator.StartIndicateCooldown(_assembleController.AssembleCooldown);
                }
            }
        }

        private class MouseInputAction : IMouseInputAction
        {
            private readonly Projectile _projectile;
            private readonly Player _player;
            private readonly PlayerAssembleController _assembleController;

            public MouseInputAction(Projectile projectile, Player player, PlayerAssembleController assembleController)
            {
                _projectile = projectile;
                _player = player;
                _assembleController = assembleController;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (_projectile.Position != _player.Position)
                {
                    return;
                }
                if (inputs == MouseInputs.Left && !_assembleController._isDisassembleFrozen)
                {
                    _projectile.Disassemble();
                }
                else if (inputs == MouseInputs.Right && !_assembleController._isAssembleFrozen)
                {
                    _projectile.Assemble();
                }
            }
        }
    }
}
