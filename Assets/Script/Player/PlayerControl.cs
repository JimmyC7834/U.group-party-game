using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private float speed = default;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] public Vector2 moveDir;
        [SerializeField] public Vector2 facingDir = Vector2.up;

        // [SerializeField] private float keepDiagonalDirTime = default;
        // [SerializeField] private float keepDiagonalDirTimer = 0;
        // [SerializeField] private bool keepingDiagonalDir = false;

        [Space]
        [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] public UnityAction<Vector2> OnMove;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            // keepDiagonalDirTimer = keepDiagonalDirTime;

            _inputReader.moveEvent += HandleMoveInput;
        }

        private void FixedUpdate()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            _rigidbody.MovePosition(_rigidbody.position + speed * speedMultiplier * moveDir * Time.fixedDeltaTime);

            // keepingDiagonalDir = !(keepDiagonalDirTimer <= 0);
            // if (keepingDiagonalDir)
            //     keepDiagonalDirTimer -= Time.fixedDeltaTime;

        }

        private void HandleMoveInput(Vector2 dir)
        {
            moveDir = dir;

            if (moveDir.magnitude == 0)
                return;

            // if (Mathf.Abs(facingDir.x) == Mathf.Abs(facingDir.y))
            // {
            //     if (!keepingDiagonalDir)
            //     {
            //         transform.rotation = Quaternion.LookRotation(Vector3.back, moveDir);
            //         facingDir = moveDir;
            //         keepDiagonalDirTimer = keepDiagonalDirTime;
            //         keepingDiagonalDir = true;
            //     }
            // }
            // else
            // {
            // }
            facingDir = moveDir;
            OnMove?.Invoke(moveDir);
        }

        public void SetSpeedMultiplier(float value)
        {
            speedMultiplier = value;
        }
    }
}