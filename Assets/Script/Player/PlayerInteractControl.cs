using UnityEngine;

namespace Game.Player
{
    public class PlayerInteractControl : MonoBehaviour
    {
        [SerializeField] public PlayerControl player;
        [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("Pickup and Throw")]
        public Transform pickedTrans;
        public float throwStrength = default;
        [SerializeField] private ThrowableObject pickedObject;
        [SerializeField] private float interactDist = default;

        private void OnEnable()
        {
            player = GetComponent<PlayerControl>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _inputReader.interactEvent += HandleInteractInput;
        }

        public void HandleObjectThrown()
        {
            pickedObject.OnThrown -= HandleObjectThrown;
            player.SetSpeedMultiplier(1);
            pickedObject = null;
        }

        public void PickUpObject(ThrowableObject throwableObject)
        {
            throwableObject.OnThrown += HandleObjectThrown;
            player.SetSpeedMultiplier(throwableObject.slowMultiplier);
            pickedObject = throwableObject;
        }

        private void HandleInteractInput()
        {
            // raycast and check for interactions
            RaycastHit2D[] hits = Physics2D.RaycastAll(_rigidbody.position, player.facingDir, interactDist);
            Debug.DrawRay(_rigidbody.position, player.facingDir * interactDist, Color.green, .1f);

            InteractableObject.InteractInfo info = InteractableObject.InteractInfo.From(this, pickedObject);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                // handle interaction
                InteractableObject interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
                if (interactableObject != null)
                {
                    // don't interact with picked object
                    if (pickedObject == null || pickedObject.interactable != interactableObject)
                    {
                        interactableObject.Interact(info);
                        return;
                    }
                }

            }

            // if the only thing to interact is the picked object (if picked), interact
            pickedObject?.interactable.Interact(info);
        }
    }
}