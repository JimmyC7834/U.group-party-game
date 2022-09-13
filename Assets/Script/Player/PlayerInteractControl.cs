using UnityEngine;

namespace Game.Player
{
    public class PlayerInteractControl : MonoBehaviour
    {
        public PlayerControl player { get; private set; }
        [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("Interact Settings")] [SerializeField]
        private float interactDist = 1f;

        [Header("Pickup and Throw Settings")] [SerializeField]
        private Transform _pickUpTrans;

        [SerializeField] private float _pickUpHeight;
        [SerializeField] private float _putDownHeight;
        [SerializeField] private float _throwStrength;
        public ThrowableObject pickedObject { get; private set; }
        public bool pickingObject => pickedObject != null;

        private void Awake()
        {
            player = GetComponent<PlayerControl>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _inputReader.interactEvent += HandleInteractInput;
        }

        public void ThrowObject()
        {
            if (!pickingObject) return;
            player.SetSpeedMultiplier(1f);
            pickedObject.Throw(player.facingDir, _throwStrength * player.moveDir.magnitude, _putDownHeight);
            pickedObject = null;
        }

        public void SubmitObject()
        {
            if (!pickingObject) return;
            player.SetSpeedMultiplier(1f);
            pickedObject.Throw(Vector2.zero, 0, 0);
            pickedObject = null;
        }

        public void PickUpObject(ThrowableObject throwableObject)
        {
            if (pickingObject) return;
            throwableObject.PickUpBy(this, _pickUpTrans, _pickUpHeight);
            player.SetSpeedMultiplier(throwableObject.slowMultiplier);
            pickedObject = throwableObject;
        }

        private void HandleInteractInput()
        {
            // raycast and check for interactions
            RaycastHit2D[] hits = Physics2D.RaycastAll(_rigidbody.position, player.facingDir, interactDist);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_rigidbody.position + player.facingDir, interactDist);
            Debug.DrawRay(_rigidbody.position, player.facingDir * interactDist, Color.green, .1f);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider.isTrigger) return;
                InteractableObject interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
                ReceivableObject receivableObject = hit.collider.gameObject.GetComponent<ReceivableObject>();
                ThrowableObject throwableObject = hit.collider.gameObject.GetComponent<ThrowableObject>();

                // skip the collider if it's not interactable
                if (interactableObject == null) continue;

                // receivableObject has highest priority
                if (receivableObject != null && pickingObject && receivableObject.AcceptObject(pickedObject))
                {
                    receivableObject.GetComponent<InteractableObject>().Interact(this);
                    return;
                }

                // handle throwableObject if not holding any object
                if (throwableObject != null && !pickingObject)
                {
                    interactableObject.Interact(this);
                    return;
                }
            }

            //Throw object
            ThrowObject();
        }
    }
}