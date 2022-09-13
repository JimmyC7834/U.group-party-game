using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InteractableObject))]
    public class ThrowableObject : FakeHeightObject
    {
        [SerializeField] private InteractableObject _interactable;

        [Header("Throw and Pick up values")] [Tooltip("Slow down the player to x%")]
        public float slowMultiplier;

        [SerializeField] private float putDownDist;
        [SerializeField] private float putDownHeight;
        [SerializeField] private float pickUpHeight;

        [Header("bounce values")] [SerializeField]
        protected float bounceSpeedThreshold;

        [SerializeField] protected float bounceSlowMultiplier;
        [SerializeField] protected float initialVerticalVelocity;
        [SerializeField] private SpriteRenderer _bodySprite;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private PlayerInteractControl picker;

        public event UnityAction OnThrown = delegate { };
        public event UnityAction OnPickedUp = delegate { };

        private void OnEnable()
        {
            if (bodyTransform == null || shadowTransform == null)
            {
                Debug.LogError($"missing bodyTransform or shadowTransform, please check your object");
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _interactable = GetComponent<InteractableObject>();
            _interactable.SetInteractable(true);
            _interactable.OnInteracted += HandleOnInteracted;
        }

        private void HandleOnInteracted(PlayerInteractControl interactor)
        {
            interactor.PickUpObject(this);
        }

        protected override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if (picker != null)
            {
                _rigidbody.position = picker.transform.position;
            }

            // make the sprite larger along its height
            _bodySprite.transform.localScale =
                Vector2.one * (1 + (bodyTransform.position.y - shadowTransform.position.y) / 5f);
        }

        protected override void CheckGroundHit()
        {
            if (bodyTransform.position.y <= transform.position.y && !IsGrounded)
            {
                if (groundVelocity.magnitude > bounceSpeedThreshold)
                {
                    Bounce();
                }
                else
                {
                    Land();
                }
            }
        }

        private void Land()
        {
            _bodySprite.sortingOrder = 0;
            bodyTransform.position = transform.position;
            groundVelocity = Vector2.zero;
            EnableGroundPhysics();
            IsGrounded = true;
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

        protected virtual void Bounce()
        {
            groundVelocity = groundVelocity * bounceSlowMultiplier;
            verticalVelocity = initialVerticalVelocity * bounceSlowMultiplier;
            EnableGroundPhysics();
        }

        public void Throw(Vector2 dir, float magnitude, float _initialHeight)
        {
            // put down the object if not moving
            if (magnitude == 0)
                transform.position += (Vector3) dir * putDownDist;

            transform.SetParent(null);
            DisableGroundPhysics();
            Launch(dir * magnitude, magnitude, _initialHeight);

            picker = null;
            initialVerticalVelocity = magnitude;
            _bodySprite.sortingOrder = 1;

            OnThrown.Invoke();
        }

        public void PickUpBy(PlayerInteractControl interactor, Transform pickUpTrans, float _height)
        {
            DisableGroundPhysics();

            transform.SetParent(pickUpTrans, true);

            picker = interactor;
            bodyTransform.position += Vector3.up * _height;
            _bodySprite.sortingOrder = 1;
            _isGrounded = true;

            OnPickedUp.Invoke();
        }
    }
}