using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerPickControl : MonoBehaviour
    {
        [SerializeField] private PlayerControl player;
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("Pickup and Throw")]
        [SerializeField] private Transform pickedTrans;
        [SerializeField] private ThrowableObject pickedObject;
        [SerializeField] private float pickUpHeight;
        [SerializeField] private float putDownHeight;
        [SerializeField] private float throwStrength;
        [SerializeField] private float interactDist;

        private void OnEnable()
        {
            player = GetComponent<PlayerControl>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _inputReader.interactEvent += HandleInteractInput;
        }

        private void HandleInteractInput()
        {
            // throw if picking object
            if (pickedObject != null)
            {
                pickedObject.Throw(player.facingDir, throwStrength * player.moveDir.magnitude, putDownHeight);
                player.SetSpeedMultiplier(1);
                pickedObject = null;
                return;
            }

            // raycast and check for interactions
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, player.facingDir, interactDist);
            Debug.DrawRay(_rigidbody.position, player.facingDir * interactDist, Color.green, .1f);
            if (hit)
            {
                // handle throwable's pickup
                ThrowableObject throwableObject = hit.collider.gameObject.GetComponent<ThrowableObject>();
                if (throwableObject != null && pickedObject == null && throwableObject.IsGrounded)
                {
                    throwableObject.PickUpBy(pickedTrans, pickUpHeight);
                    pickedObject = throwableObject;
                    player.SetSpeedMultiplier(pickedObject.slowMultiplier);
                }
            }
        }
    }
}