using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Interactable))]
    public class Throwable : MonoBehaviour
    {
        [SerializeField] private Interactable _interactable;
        [SerializeField] protected Transform _bodyTransform;
        [SerializeField] protected Transform _shadowTransform;

        [SerializeField] protected Vector2 _horizontalVelocity;
        [SerializeField] protected float _verticalVelocity;
        [SerializeField] protected bool _isGrounded;
        [SerializeField] protected float _gravity = -10f;

        [SerializeField] private float putDownDist;
        [SerializeField] private float putDownHeight;
        [SerializeField] private float pickUpHeight;

        private Collider _collider;
        private Rigidbody _rigidbody;
        private Holder _holder;

        public bool IsGrounded => _isGrounded;
        public Vector2 HorizontalVelocity => _horizontalVelocity;
        public float VerticalVelocity => _verticalVelocity;
        public float Height => transform.position.z;

        public event UnityAction<Holder> OnThrown = delegate { };
        public event UnityAction<Holder> OnHeld = delegate { };
        public event UnityAction<Holder> OnReleased = delegate { };

        public event UnityAction OnLaunched = delegate { };
        public event UnityAction OnGrounded = delegate { };
        public event UnityAction OnMidAir = delegate { };

        private void OnEnable()
        {
            if (_bodyTransform == null || _shadowTransform == null)
            {
                Debug.LogError($"missing bodyTransform or shadowTransform, please check your object");
                gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _interactable = GetComponent<Interactable>();
            _interactable.SetInteractable(true);
            _interactable.OnInteracted += HandleOnInteracted;
        }

        private void FixedUpdate()
        {
            // UpdatePhysics();
            CheckGroundHit();

            if (_holder != null)
                transform.position = _holder.HolderTrans.position;
        }

        public void SetVerticalVelocity(float value)
        {
            _verticalVelocity = value;
        }

        public void SetHorizontalVelocity(Vector2 value)
        {
            _horizontalVelocity = value;
        }

        private void HandleOnInteracted(Interactor interactor)
        {
            Holder holder = interactor.GetComponent<Holder>();
            if (holder == null || holder.IsHolding()) return;
            holder.Hold(this);
        }

        protected void UpdatePhysics()
        {
            if (IsGrounded) return;
            _verticalVelocity += _gravity * Time.deltaTime;
            _bodyTransform.position += _verticalVelocity * Time.deltaTime * Vector3.up;
            transform.position += new Vector3(
                _horizontalVelocity.x,
                _verticalVelocity,
                _horizontalVelocity.y) * Time.deltaTime;
        }

        protected void CheckGroundHit()
        {
            if (_bodyTransform.position.y <= transform.position.y &&
                !IsGrounded)
                Land();
        }

        private void Land()
        {
            transform.position = new Vector3(
                transform.position.x, 0f, transform.position.z);
            _bodyTransform.position = transform.position;
            _horizontalVelocity = Vector2.zero;
            EnableGroundPhysics();
            _isGrounded = true;
            OnGrounded.Invoke();
        }

        private void EnableGroundPhysics()
        {
            _collider.isTrigger = false;
            _interactable.SetInteractable(true);
            _rigidbody.WakeUp();
        }

        private void DisableGroundPhysics()
        {
            _collider.isTrigger = true;
            _interactable.SetInteractable(false);
            _rigidbody.Sleep();
        }

        /// <summary>
        /// Apply 3d force to the throwable
        /// </summary>
        public void Launch(Vector2 horizontalVelocity, float verticalVelocity, float initialHeight)
        {
            DisableGroundPhysics();

            _horizontalVelocity = horizontalVelocity;
            _verticalVelocity = verticalVelocity;
            _bodyTransform.position += Vector3.up * initialHeight;
            _isGrounded = false;

            _rigidbody.velocity =
                new Vector3(_horizontalVelocity.x, _verticalVelocity, _horizontalVelocity.y);

            OnLaunched.Invoke();
        }

        public void HoldBy(Holder holder)
        {
            DisableGroundPhysics();

            _isGrounded = true;
            _holder = holder;

            OnHeld.Invoke(holder);
        }


        public void ThrowBy(Holder holder)
        {
            OnThrown.Invoke(holder);
        }

        public void ReleaseBy(Holder holder)
        {
            _interactable.SetInteractable(true);
            _holder = null;
            OnReleased.Invoke(holder);
        }

        public void DisableInteractable()
        {
            _interactable.SetInteractable(false);
        }

        public float GetGravity() => _gravity;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
                Land();
        }
    }
}