using Game.Data;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Holder))]
    [RequireComponent(typeof(Interactor))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerSO _playerSO;
        [SerializeField] private InputReader _inputReader = default;

        [Header("Parameter")]
        [SerializeField] private float _interactDist;
        [SerializeField] private float _throwHeight;
        [SerializeField] private float _throwStrength;

        private Rigidbody2D _rigidbody;
        private Movement _movement;
        private Health _heath;
        private Holder _holder;
        private Interactor _interactor;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _movement = GetComponent<Movement>();
            _heath = GetComponent<Health>();
            _holder = GetComponent<Holder>();
            _interactor = GetComponent<Interactor>();

            _inputReader.EnablePlayerInput();
            _inputReader.interactEvent += DoInteract;
            _inputReader.moveEvent += _movement.SetDirection;
        }

        private void DoInteract()
        {
            bool wasHolding = _holder.IsHolding();

            Interactable interactable = _interactor.GetInteractable(
                _rigidbody.position, _movement.facingDir, _interactDist);
            _interactor.Interact(interactable);

            if (!_holder.IsHolding()) return;
            if (!wasHolding) return;

            // Throw
            _holder.Throw(
                _movement.facingDir,
                _throwStrength * _movement.moveDir.magnitude,
                _throwHeight);
            _holder.Release();
        }
    }
}