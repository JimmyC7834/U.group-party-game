using Game.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float _speed = 4f;
        [SerializeField] private float _speedMultiplier = 1f;
        public Vector2 moveDir { get; private set; }
        public Vector2 facingDir { get; private set; }
        [SerializeField] private PlayerSO _playerSO;
        [SerializeField] private PlayerStats _stats;

        [Space] [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private Rigidbody2D _rigidbody;

        public event UnityAction<Vector2> OnMove;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            if (_stats != null)
            {
                _stats = GetComponent<PlayerStats>();
            }
            else
            {
                _stats = gameObject.AddComponent<PlayerStats>();
                // _stats.Killed += Kill;
            }
            _stats.Initialize(_playerSO);
            _stats.Killed += Kill;

            _inputReader.moveEvent += HandleMoveInput;
        }

        private void FixedUpdate()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            _rigidbody.MovePosition(
                _rigidbody.position + _speed * _speedMultiplier * Time.fixedDeltaTime * moveDir);
        }

        private void HandleMoveInput(Vector2 dir)
        {
            moveDir = dir;

            if (moveDir.magnitude == 0)
                return;

            facingDir = moveDir;
            OnMove?.Invoke(moveDir);
        }

        public void SetSpeedMultiplier(float value)
        {
            _speedMultiplier = value;
        }

        private void Kill()
        {
            // sent message to game manager to end game
            // or died and wait for respawn
            Debug.Log("Player is died!");
        }
    }
}