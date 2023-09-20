using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 4f;
        [SerializeField] private float _speedMultiplier = 1f;
        public Vector2 moveDir { get; private set; }
        public Vector2 facingDir { get; private set; }

        [SerializeField] private Rigidbody2D _rigidbody;

        public event UnityAction<Vector2> OnMove = delegate { };

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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

        public void SetDirection(Vector2 dir)
        {
            moveDir = dir;
            if (moveDir.magnitude == 0) return;
            facingDir = moveDir;
            OnMove.Invoke(moveDir);
        }

        public void SetSpeedMultiplier(float value)
        {
            _speedMultiplier = value;
        }
    }
}