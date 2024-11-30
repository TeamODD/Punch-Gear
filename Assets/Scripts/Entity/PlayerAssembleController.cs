using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerAssembleController : MonoBehaviour
    {
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");
        private static readonly int DisassembleAnimation = Animator.StringToHash("Disassemble");
        private static readonly int AssembleAnimation = Animator.StringToHash("Assemble");

        private static AudioClip _disassembleAudioClip;
        private static AudioClip _disassembleMissAudioClip;
        private static AudioClip _assembleAudioClip;
        private static AudioClip _assembleMissAudioClip;

        [field: SerializeField]
        public float AssembleCooldown { get; private set; }

        private Animator _animator;

        private AssemblyPoint[] _assemblyPoints;
        private bool _disabled;

        private bool _isAssembleFrozen;
        private bool _isDisassembleFrozen;
        private Dictionary<IProjectile, IMouseInputAction> _mouseInputActionLookup;

        private Player _player;

#if UNITY_EDITOR
        private GUIStyle _style;
#endif

        private void Awake()
        {
            _player = GetComponent<Player>();
            _animator = GetComponent<Animator>();
            _assemblyPoints = FindObjectsByType<AssemblyPoint>(FindObjectsSortMode.None);
            _mouseInputActionLookup = new Dictionary<IProjectile, IMouseInputAction>();
            _isAssembleFrozen = false;
            _isDisassembleFrozen = false;
#if UNITY_EDITOR
            _style = new GUIStyle
            {
                fontSize = (int) (40.0f * (Screen.width / 1920f))
            };
#endif
            GloballyPlayerInputHandler.Instance.AddAction(new AnimationTransitionAction(this, _player));

            foreach (AssemblyPoint assemblyPoint in _assemblyPoints)
            {
                GloballyPlayerInputHandler.Instance.AddAction(new AudioEffectAction(assemblyPoint, this, _player));
            }
        }

        private void Start()
        {
            IProjectileLauncher launcher = ProjectileLauncher.Instance;
            launcher.OnProjectileCreated.AddListener(
                projectile =>
                {
                    IMouseInputAction action = new MouseInputAction(projectile, _player, this);
                    _mouseInputActionLookup[projectile] = action;
                    GloballyPlayerInputHandler.Instance.AddAction(action);
                });
            launcher.OnProjectileDestroyed.AddListener(
                projectile =>
                {
                    if (_disabled)
                    {
                        return;
                    }
                    if (!_mouseInputActionLookup.TryGetValue(projectile, out IMouseInputAction action))
                    {
                        return;
                    }
                    GloballyPlayerInputHandler.Instance.RemoveAction(action);
                    _mouseInputActionLookup.Remove(projectile);
                });
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _disabled = true;
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            GUI.Label(new Rect(50, 50, 200, 100), $"isAssembleFrozen: {_isAssembleFrozen}", _style);
            GUI.Label(new Rect(50, 100, 200, 100), $"isDisassembleFrozen: {_isDisassembleFrozen}", _style);
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void LoadAudioClips()
        {
            _disassembleAudioClip = Resources.Load<AudioClip>("Sound/분해");
            _assembleAudioClip = Resources.Load<AudioClip>("Sound/조립");
            _disassembleMissAudioClip = Resources.Load<AudioClip>("Sound/분해 실수");
            _assembleMissAudioClip = Resources.Load<AudioClip>("Sound/조립 실수");
            if (_disassembleAudioClip == null)
            {
                throw new NullReferenceException("Cannot find Audio clip in the path");
            }
            if (_assembleAudioClip == null)
            {
                throw new NullReferenceException("Cannot find Audio clip in the path");
            }
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
            _animator.SetTrigger(IdleAnimation);
        }

        private enum MouseAssembleAction
        {
            Assemble,
            Disassemble
        }

        private class AudioEffectAction : IMouseInputAction
        {
            private readonly PlayerAssembleController _assembleController;
            private readonly AssemblyPoint _assemblyPoint;
            private readonly Player _player;

            public AudioEffectAction(
                AssemblyPoint assemblyPoint,
                PlayerAssembleController assembleController,
                Player player)
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
                    if (_assemblyPoint.EntersProjectile
                     && _assemblyPoint.ProjectileTargets.Any(
                            projectile => projectile.State != ProjectileState.Disassembled))
                    {
                        AudioManager.Instance.Play(_disassembleAudioClip);
                    }
                    else
                    {
                        AudioManager.Instance.Play(_disassembleMissAudioClip);
                    }
                }
                else if (inputs == MouseInputs.Right)
                {
                    if (_assembleController._isAssembleFrozen)
                    {
                        return;
                    }
                    if (_assemblyPoint.EntersProjectile
                     && _assemblyPoint.ProjectileTargets.Any(
                            projectile => projectile.Disassembled && !projectile.Assembled))
                    {
                        AudioManager.Instance.Play(_assembleAudioClip);
                    }
                    else
                    {
                        AudioManager.Instance.Play(_assembleMissAudioClip);
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
                    _assembleController._animator.SetTrigger(DisassembleAnimation);
                    _assembleController.FreezeMouse(MouseAssembleAction.Disassemble);
                    _player.DisassemblyCooldownIndicator.StartIndicateCooldown(_assembleController.AssembleCooldown);
                }
                else if (inputs == MouseInputs.Right)
                {
                    if (_assembleController._isAssembleFrozen)
                    {
                        return;
                    }
                    _assembleController._animator.SetTrigger(AssembleAnimation);
                    _assembleController.FreezeMouse(MouseAssembleAction.Assemble);
                    _player.AssemblyCooldownIndicator.StartIndicateCooldown(_assembleController.AssembleCooldown);
                }
            }
        }

        private class MouseInputAction : IMouseInputAction
        {
            private readonly PlayerAssembleController _assembleController;
            private readonly Player _player;
            private readonly IProjectile _projectile;

            public MouseInputAction(IProjectile projectile, Player player, PlayerAssembleController assembleController)
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
