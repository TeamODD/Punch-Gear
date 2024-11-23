using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerAssembleController : MonoBehaviour
    {
        private Player _player;
        private Dictionary<Projectile, IMouseInputAction> _mouseInputActionLookup;

        [field: SerializeField]
        public float AssembleCooldown { get; private set; }

        private bool _isAssembleFrozen;
        private bool _isDisassembleFrozen;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _mouseInputActionLookup = new Dictionary<Projectile, IMouseInputAction>();
            _isAssembleFrozen = false;
            _isDisassembleFrozen = false;
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
                Debug.Log("Assemble is frozen");
            }
            else
            {
                _isDisassembleFrozen = true;
                Debug.Log("Disassemble is frozen");
            }
            yield return new WaitForSecondsRealtime(AssembleCooldown);
            if (assembleAction == MouseAssembleAction.Assemble)
            {
                _isAssembleFrozen = false;
                Debug.Log("Assemble is normal");
            }
            else
            {
                _isDisassembleFrozen = false;
                Debug.Log("Disassemble is normal");
            }
        }

        private enum MouseAssembleAction
        {
            Assemble,
            Disassemble
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
                    _assembleController.FreezeMouse(MouseAssembleAction.Disassemble);
                }
                else if (inputs == MouseInputs.Right && !_assembleController._isAssembleFrozen)
                {
                    _projectile.Assemble();
                    _assembleController.FreezeMouse(MouseAssembleAction.Assemble);
                }
            }
        }
    }
}
