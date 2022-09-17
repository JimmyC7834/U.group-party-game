using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerControl))]
    [RequireComponent(typeof(PlayerInteractControl))]
    public class PlayerAnim : MonoBehaviour
    {
        public static readonly int WalkFront = Animator.StringToHash("PlayerWalkFront");
        public static readonly int WalkSide = Animator.StringToHash("PlayerWalkSide");
        public static readonly int WalkBack = Animator.StringToHash("PlayerWalkBack");

        public static readonly int IdleFront = Animator.StringToHash("PlayerIdleFront");
        public static readonly int IdleSide = Animator.StringToHash("PlayerIdleSide");
        public static readonly int IdleBack = Animator.StringToHash("PlayerIdleBack");

        public static readonly int ThrowFront = Animator.StringToHash("PlayerThrowFront");
        public static readonly int ThrowSide = Animator.StringToHash("PlayerThrowSide");
        public static readonly int ThrowBack = Animator.StringToHash("PlayerThrowBack");

        public static readonly int PickingIdleFront = Animator.StringToHash("PlayerPickingIdleFront");
        public static readonly int PickingIdleSide = Animator.StringToHash("PlayerPickingIdleSide");
        public static readonly int PickingIdleBack = Animator.StringToHash("PlayerPickingIdleBack");

        public static readonly int PickingWalkFront = Animator.StringToHash("PlayerPickingWalkFront");
        public static readonly int PickingWalkSide = Animator.StringToHash("PlayerPickingWalkSide");
        public static readonly int PickingWalkBack = Animator.StringToHash("PlayerPickingWalkBack");

        private static readonly HashSet<int> _stateHashSet = new HashSet<int>()
        {
            WalkFront,
            WalkSide,
            WalkBack,
            PickingWalkFront,
            PickingWalkSide,
            PickingWalkBack
        };

        private static readonly Dictionary<Vector2, int> _walkAnimMap = new Dictionary<Vector2, int>()
        {
            {Vector2.down, WalkFront},
            {Vector2.up, WalkBack},
            {Vector2.left, WalkSide},
            {Vector2.right, WalkSide},
        };

        private static readonly Dictionary<Vector2, int> _pickingWalkAnimMap = new Dictionary<Vector2, int>()
        {
            {Vector2.down, PickingWalkFront},
            {Vector2.up, PickingWalkBack},
            {Vector2.left, PickingWalkSide},
            {Vector2.right, PickingWalkSide},
        };

        private static readonly Dictionary<Vector2, int> _throwAnimMap = new Dictionary<Vector2, int>()
        {
            {Vector2.down, ThrowFront},
            {Vector2.up, ThrowBack},
            {Vector2.left, ThrowSide},
            {Vector2.right, ThrowSide},
        };

        private static readonly Dictionary<Vector2, int> _idleAnimMap = new Dictionary<Vector2, int>()
        {
            {Vector2.down, IdleFront},
            {Vector2.up, IdleBack},
            {Vector2.left, IdleSide},
            {Vector2.right, IdleSide},
        };

        private static readonly Dictionary<Vector2, int> _pickingIdleAnimMap = new Dictionary<Vector2, int>()
        {
            {Vector2.down, PickingIdleFront},
            {Vector2.up, PickingIdleBack},
            {Vector2.left, PickingIdleSide},
            {Vector2.right, PickingIdleSide},
        };

        [SerializeField] private PlayerControl _playerControl;
        [SerializeField] private PlayerInteractControl _playerInteractControl;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _transitionDuration = 0f;

        private void Awake()
        {
            _playerControl = GetComponent<PlayerControl>();
            _playerInteractControl = GetComponent<PlayerInteractControl>();

            _playerControl.OnMove += HandlePlayerMovement;
            _playerControl.OnStop += HandlePlayerStop;

            _playerInteractControl.OnPickUp += HandlePlayerMovement;
            _playerInteractControl.OnThrow += HandlePlayerThrow;
        }

        private void HandlePlayerThrow()
        {
            if (Mathf.Abs(_playerControl.facingDir.x) != 0 &&
                Mathf.Abs(_playerControl.facingDir.y) != 0) return;

            Dictionary<Vector2, int> animMap = _throwAnimMap;
            _sprite.flipX = _playerControl.facingDir.x > 0;
            _animator.CrossFadeInFixedTime(animMap[_playerControl.facingDir], _transitionDuration);
        }

        private void HandlePlayerStop()
        {
            Dictionary<Vector2, int> animMap;
            if (_playerInteractControl.pickingObject)
            {
                animMap = _pickingIdleAnimMap;
            }
            else
            {
                animMap = _idleAnimMap;
            }

            _sprite.flipX = _playerControl.facingDir.x > 0;
            _animator.CrossFadeInFixedTime(animMap[_playerControl.facingDir], _transitionDuration);
        }

        private void HandlePlayerMovement()
        {
            if (Mathf.Abs(_playerControl.facingDir.x) != 0 &&
                Mathf.Abs(_playerControl.facingDir.y) != 0) return;

            Dictionary<Vector2, int> animMap;
            if (_playerInteractControl.pickingObject)
            {
                animMap = _pickingWalkAnimMap;
            }
            else
            {
                animMap = _walkAnimMap;
            }

            _sprite.flipX = _playerControl.facingDir.x > 0;
            _animator.CrossFadeInFixedTime(animMap[_playerControl.facingDir], _transitionDuration);
        }

        public void BackToIdle()
        {
            HandlePlayerMovement();
        }

        public void SwitchTo(int stateHash)
        {
            if (!_stateHashSet.Contains(stateHash)) return;
            _animator.CrossFade(stateHash, _transitionDuration);
        }
    }
}