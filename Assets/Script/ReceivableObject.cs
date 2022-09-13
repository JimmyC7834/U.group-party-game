using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(InteractableObject))]
    public abstract class ReceivableObject : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;

        private void Awake()
        {
            _interactableObject = GetComponent<InteractableObject>();
            _interactableObject.SetInteractable(true);
            _interactableObject.OnInteracted += HandleInteract;
        }

        protected void HandleInteract(PlayerInteractControl interactor)
        {
            if (!interactor.pickingObject) return;
            ThrowableObject throwableObject = interactor.pickedObject.GetComponent<ThrowableObject>();
            if (!AcceptObject(throwableObject)) return;
            HandleReceive(interactor);
        }

        public abstract bool AcceptObject(ThrowableObject throwableObject);
        protected abstract void HandleReceive(PlayerInteractControl interactor);
    }
}