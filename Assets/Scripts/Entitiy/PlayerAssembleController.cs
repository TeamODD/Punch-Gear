using System.Collections.Generic;
using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerAssembleController : MonoBehaviour
    {
        private Player _player;
        private Dictionary<Projectile, IMouseInputAction> _mouseInputActionLookup;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _mouseInputActionLookup = new Dictionary<Projectile, IMouseInputAction>();
        }

        private void Start()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            launcher.OnProjectileCreated.AddListener(projectile =>
            {
                IMouseInputAction action = new MouseInputAction(projectile, _player);
                _mouseInputActionLookup[projectile] = action;
                GloballyPlayerInputHandler.Instance.AddAction(action);
                Debug.Log("Projectile launched");
            });
            launcher.OnProjectileDestroyed.AddListener(projectile =>
            {
                IMouseInputAction action = _mouseInputActionLookup[projectile];
                GloballyPlayerInputHandler.Instance.RemoveAction(action);
                _mouseInputActionLookup.Remove(projectile);
            });
        }

        private class MouseInputAction : IMouseInputAction
        {
            private readonly Projectile _projectile;
            private readonly Player _player;

            public MouseInputAction(Projectile projectile, Player player)
            {
                _projectile = projectile;
                _player = player;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (_projectile.Position != _player.Position)
                {
                    return;
                }
                if (inputs == MouseInputs.Left)
                {
                    _projectile.Disassemble();
                }
                else if (inputs == MouseInputs.Right)
                {
                    _projectile.Assemble();
                }
            }
        }
    }
}
