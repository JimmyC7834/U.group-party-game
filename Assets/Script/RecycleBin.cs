using Game.Player;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(InteractableObject))]
    public class RecycleBin : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;

        private void Awake()
        {
            _interactableObject = GetComponent<InteractableObject>();
            _interactableObject.SetInteractable(true);
            _interactableObject.OnInteracted += HandleInteract;
        }

        private void HandleInteract(PlayerInteractControl interactor)
        {
            if (!interactor.pickingObject) return;
            ResourceObject resourceObject = interactor.pickedObject.GetComponent<ResourceObject>();
            if (resourceObject == null) return;
            interactor.SubmitObject();
            resourceObject.ReturnToPool();
        }
    }
}